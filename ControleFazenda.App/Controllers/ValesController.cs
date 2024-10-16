using AutoMapper;
using ControleFazenda.App.ViewModels;
using ControleFazenda.Business.Entidades;
using ControleFazenda.Business.Entidades.Enum;
using ControleFazenda.Business.Interfaces;
using ControleFazenda.Business.Interfaces.Servicos;
using ControleFazenda.Business.Servicos;
using ControleFazenda.Data.Context;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

namespace ControleFazenda.App.Controllers
{
    public class ValesController : BaseController
    {
        private readonly IValeServico _valeServico;
        private readonly IColaboradorServico _colaboradorServico;
        private readonly IMapper _mapper;
        private readonly UserManager<Usuario> _userManager;
        private readonly ContextoPrincipal _context;
        private readonly ILogAlteracaoServico _logAlteracaoServico;

        public ValesController(IMapper mapper,
                                  IValeServico valeServico,
                                  IColaboradorServico colaboradorServico,
                                  UserManager<Usuario> userManager,
                                  ContextoPrincipal context,
                                  ILogAlteracaoServico logAlteracaoServico,
                                  INotificador notificador) : base(notificador)
        {
            _mapper = mapper;
            _valeServico = valeServico;
            _colaboradorServico = colaboradorServico;
            _userManager = userManager;
            _context = context;
            _logAlteracaoServico = logAlteracaoServico;
        }

        [Route("lista-de-vales")]
        public async Task<IActionResult> Index()
        {
            Usuario? user = await _userManager.GetUserAsync(User);
            var vales = await _valeServico.ObterTodosComColaborador();
            var valesVM = _mapper.Map<List<ValeVM>>(vales);
            var valesFazenda = new List<ValeVM>();
            if (user != null)
            {
                foreach (var item in valesVM)
                {
                    Usuario? usuario = await _userManager.FindByIdAsync(item.UsuarioCadastroId.ToString());
                    if (usuario?.Fazenda == user.Fazenda)
                        valesFazenda.Add(item);
                }
                return View(valesFazenda);
            }
            else
                return View(new List<ValeVM>());
        }

        [Route("editar-vale/{id}")]
        public async Task<IActionResult> Editar(Guid Id)
        {
            var valeVM = new ValeVM();
            if (Id != Guid.Empty)
            {
                var vale = await _valeServico.ObterPorIdComColaborador(Id);
                if (vale == null) return NotFound();

                valeVM = _mapper.Map<ValeVM>(vale);
                valeVM.UsuarioCadastro = await _userManager.FindByIdAsync(valeVM.UsuarioCadastroId.ToString());
                valeVM.UsuarioAlteracao = await _userManager.FindByIdAsync(valeVM.UsuarioAlteracaoId.ToString());
                if (valeVM.Valor < 0)
                    valeVM.Valor *= -1;
                valeVM = await PopularColaboradores(valeVM);
            }
            else
                valeVM = await PopularColaboradores(valeVM);

            return View(valeVM);
        }

        [Route("editar-vale/{id:guid}")]
        [HttpPost]
        public async Task<IActionResult> Editar(Guid Id, ValeVM valeVM)
        {
            if (Id != valeVM.Id) return NotFound();
            if (!ModelState.IsValid)
            {
                var errors = ModelState.ToDictionary(kvp => kvp.Key, kvp => kvp.Value?.Errors.Select(e => e.ErrorMessage).ToList());
                return Json(new { success = false, errors, isModelState = true });
            }

            Usuario? user = await _userManager.GetUserAsync(User);
            Vale vale;

            if (user != null)
            {
                using var transaction = await _context.Database.BeginTransactionAsync();
                try
                {
                    if (Id != Guid.Empty)
                    {
                        var valeClone = await _valeServico.ObterPorIdComColaborador(valeVM.Id);
                        valeVM.DataAlteracao = DateTime.Now;
                        vale = _mapper.Map<Vale>(valeVM);
                        vale.UsuarioAlteracaoId = Guid.Parse(user.Id);
                        if (vale.Situacao == Situacao.Inativo)
                            vale.Valor *= -1;
                        await _logAlteracaoServico.CompararAlteracoes(valeClone, vale, Guid.Parse(user.Id), $"Vale[{vale.Id}]");
                        await _valeServico.Atualizar(vale);

                    }
                    else
                    {
                        vale = _mapper.Map<Vale>(valeVM);
                        vale.UsuarioCadastroId = Guid.Parse(user.Id);
                        vale.Numero = await _valeServico.ObterNumeroUltimoVale() + 1;
                        if (vale.Situacao == Situacao.Inativo)
                            vale.Valor *= -1;
                        await _valeServico.Adicionar(vale);
                    }

                    if (!OperacaoValida())
                    {
                        await transaction.RollbackAsync();
                        List<string> errors = new List<string>();
                        errors = _notificador.ObterNotificacoes().Select(x => x.Mensagem).ToList();
                        return Json(new { success = false, errors });
                    }
                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    return Json(new { success = false, errors = ex.Message });
                }

                return Json(new { success = true });
            }
            return View(valeVM);
        }

        [HttpPost]
        [Route("excluir-vale/{id:guid}")]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> Deletar(Guid id)
        {
            var vale = await _valeServico.ObterPorIdComColaborador(id);
            IdentityUser? user = await _userManager.GetUserAsync(User);
            if (vale == null) return NotFound();
            if (user == null) return NotFound();

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                await _logAlteracaoServico.RegistrarLogDiretamente($"Registro: {vale.Colaborador?.RazaoSocial} excluído.", Guid.Parse(user.Id), $"Vale[{vale.Id}]");
                await _valeServico.Remover(id);
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception(ex.Message);
            }

            if (!OperacaoValida()) return View(vale);

            return RedirectToAction("Index");
        }

        [HttpPost]
        [Route("cancelar-vale/{id:guid}")]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> Cancelar(Guid id)
        {
            var vale = await _valeServico.ObterPorIdComColaborador(id);
            IdentityUser? user = await _userManager.GetUserAsync(User);
            if (vale == null) return NotFound();
            if (user == null) return NotFound();

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                vale.Situacao = Situacao.Inativo;
                await _logAlteracaoServico.RegistrarLogDiretamente($"Registro: {vale.Colaborador?.RazaoSocial} cancelado.", Guid.Parse(user.Id), $"Vale[{vale.Id}]");
                await _valeServico.Atualizar(vale);
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception(ex.Message);
            }

            if (!OperacaoValida()) return View(vale);

            return RedirectToAction("Index");
        }

        private async Task<ValeVM> PopularColaboradores(ValeVM vale)
        {
            var colaboradores = await _colaboradorServico.Buscar(x => x.Situacao == Situacao.Ativo);
            var colaboradoresVM = _mapper.Map<IEnumerable<ColaboradorVM>>(colaboradores);
            colaboradoresVM = colaboradoresVM.OrderBy(x => x.RazaoSocial);
            vale.Colaboradores = colaboradoresVM;

            return vale;
        }

        [Route("imprimir-vale/{id:guid}")]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> ImprimirVale(Guid id)
        {
            var vale = await _valeServico.ObterPorIdComColaborador(id);
            var valeVM = _mapper.Map<ValeVM>(vale);

            using (var memoryStream = new MemoryStream())
            {
                // Cria o documento PDF
                var document = new Document(PageSize.A4, 50, 50, 25, 25);
                var writer = PdfWriter.GetInstance(document, memoryStream);
                document.Open();

                // Adiciona título ao PDF
                var table = new PdfPTable(4);
                table.WidthPercentage = 100;
                table.SpacingBefore = 20;
                table.SpacingAfter = 20;

                // Define fontes para os textos
                var titleFont = FontFactory.GetFont("Arial", 18, Font.BOLD);
                var estiloPadrao = FontFactory.GetFont("Arial", 12, Font.NORMAL);
                var estiloNumValor = FontFactory.GetFont("Arial", 14, Font.BOLD);
                var estiloNomes = FontFactory.GetFont("Arial", 14, Font.BOLD);

                // Adiciona o título à tabela, ocupando 2 colunas
                var titleCell = new PdfPCell(new Phrase("VALE", titleFont))
                {
                    Colspan = 4, // A célula ocupa as 2 colunas da tabela
                    HorizontalAlignment = Element.ALIGN_CENTER, // Centraliza o texto
                    BorderWidth = 1f, // Adiciona borda à célula
                    Padding = 10f, // Adiciona espaço dentro da célula
                                   //SpacingAfter = 10f // Espaçamento após a célula
                };
                table.AddCell(titleCell);

                // Adiciona as células à tabela
                table.AddCell(new PdfPCell(new Phrase("Número", estiloNumValor)) { BorderWidth = 1f });
                table.AddCell(new PdfPCell(new Phrase(valeVM.Numero.ToString(), estiloNumValor)) { BorderWidth = 1f });

                table.AddCell(new PdfPCell(new Phrase("Valor", estiloNumValor)) { BorderWidth = 1f });
                table.AddCell(new PdfPCell(new Phrase(valeVM.Valor.ToString("C"), estiloNumValor)) { BorderWidth = 1f });

                table.AddCell(new PdfPCell(new Phrase("Colaborador", estiloNumValor)) { BorderWidth = 1f });
                table.AddCell(new PdfPCell(new Phrase(valeVM.Colaborador?.RazaoSocial?.ToUpper(), estiloPadrao)) { BorderWidth = 1f, Colspan = 3 });

                table.AddCell(new PdfPCell(new Phrase("Data", estiloNumValor)) { BorderWidth = 1f });
                table.AddCell(new PdfPCell(new Phrase(valeVM.Data.ToShortDateString(), estiloPadrao)) { BorderWidth = 1f, Colspan = 3 });

                table.AddCell(new PdfPCell(new Phrase("Valor ", estiloNumValor)) { BorderWidth = 1f, Colspan = 1 });
                table.AddCell(new PdfPCell(new Phrase(valeVM.ValorPorExtenso, estiloPadrao)) { BorderWidth = 1f, Colspan = 3 });

                table.AddCell(new PdfPCell(new Phrase("Autorizado por ", estiloNumValor)) { BorderWidth = 1f, Colspan = 1 });
                table.AddCell(new PdfPCell(new Phrase(valeVM.AutorizadoPor?.ToUpper(), estiloPadrao)) { BorderWidth = 1f, Colspan = 3 });

                table.AddCell(new PdfPCell(new Phrase("Assinatura ", estiloNumValor)) { BorderWidth = 1f, Colspan = 1 });
                table.AddCell(new PdfPCell(new Phrase(" ", estiloPadrao)) { BorderWidth = 1f, Colspan = 3 });

                // Adiciona a tabela ao documento
                document.Add(table);

                // Fecha o documento
                document.Close();

                // Retorna o PDF para exibição inline no navegador
                var pdfBytes = memoryStream.ToArray();
                var contentDisposition = new ContentDispositionHeaderValue("inline")
                {
                    FileName = "Vale.pdf"
                };

                Response.Headers[HeaderNames.ContentDisposition] = contentDisposition.ToString();
                Response.ContentType = "application/pdf";

                return new FileContentResult(pdfBytes, "application/pdf");

            }
            
        }
    }
}

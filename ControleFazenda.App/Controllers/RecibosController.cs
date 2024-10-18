using AutoMapper;
using ControleFazenda.App.ViewModels;
using ControleFazenda.Business.Entidades;
using ControleFazenda.Business.Entidades.Enum;
using ControleFazenda.Business.Interfaces;
using ControleFazenda.Business.Interfaces.Servicos;
using ControleFazenda.Data.Context;
using iTextSharp.text.pdf;
using iTextSharp.text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Net.Http.Headers;
using ControleFazenda.App.Extensions;
using ControleFazenda.Business.Servicos;

namespace ControleFazenda.App.Controllers
{
    public class RecibosController : BaseController
    {
        private readonly IReciboServico _reciboServico;
        private readonly IColaboradorServico _colaboradorServico;
        private readonly IMapper _mapper;
        private readonly UserManager<Usuario> _userManager;
        private readonly ContextoPrincipal _context;
        private readonly ILogAlteracaoServico _logAlteracaoServico;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public RecibosController(IMapper mapper,
                                  IReciboServico reciboServico,
                                  IColaboradorServico colaboradorServico,
                                  UserManager<Usuario> userManager,
                                  ContextoPrincipal context,
                                  ILogAlteracaoServico logAlteracaoServico,
                                  IWebHostEnvironment webHostEnvironment,
                                  INotificador notificador) : base(notificador)
        {
            _mapper = mapper;
            _reciboServico = reciboServico;
            _colaboradorServico = colaboradorServico;
            _userManager = userManager;
            _context = context;
            _logAlteracaoServico = logAlteracaoServico;
            _webHostEnvironment = webHostEnvironment;
        }

        [Route("lista-de-recibos")]
        public async Task<IActionResult> Index()
        {
            Usuario? user = await _userManager.GetUserAsync(User);
            var recibos = await _reciboServico.ObterTodosComColaborador();
            var recibosVM = _mapper.Map<List<ReciboVM>>(recibos);
            var recibosFazenda = new List<ReciboVM>();
            if (user != null && user.AcessoTotal == false)
            {
                foreach (var item in recibosVM)
                {
                    Usuario? usuario = await _userManager.FindByIdAsync(item.UsuarioCadastroId.ToString());
                    if (usuario?.Fazenda == user.Fazenda)
                        recibosFazenda.Add(item);
                }
                return View(recibosFazenda);
            }
            else
            {
                recibos = await _reciboServico.ObterTodos();
                recibosVM = _mapper.Map<List<ReciboVM>>(recibos);
                return View(recibosVM);
            }
        }

        [Route("editar-recibo/{id}")]
        public async Task<IActionResult> Editar(Guid Id)
        {
            var reciboVM = new ReciboVM();
            if (Id != Guid.Empty)
            {
                var recibo = await _reciboServico.ObterPorIdComColaborador(Id);
                if (recibo == null) return NotFound();

                reciboVM = _mapper.Map<ReciboVM>(recibo);
                reciboVM.UsuarioCadastro = await _userManager.FindByIdAsync(reciboVM.UsuarioCadastroId.ToString());
                reciboVM.UsuarioAlteracao = await _userManager.FindByIdAsync(reciboVM.UsuarioAlteracaoId.ToString());
                reciboVM = await PopularColaboradores(reciboVM);
            }
            else
                reciboVM = await PopularColaboradores(reciboVM);

            return View(reciboVM);
        }

        [Route("editar-recibo/{id:guid}")]
        [HttpPost]
        public async Task<IActionResult> Editar(Guid Id, ReciboVM reciboVM)
        {
            if (Id != reciboVM.Id) return NotFound();
            if (!ModelState.IsValid)
            {
                var errors = ModelState.ToDictionary(kvp => kvp.Key, kvp => kvp.Value?.Errors.Select(e => e.ErrorMessage).ToList());
                return Json(new { success = false, errors, isModelState = true });
            }

            Usuario? user = await _userManager.GetUserAsync(User);
            Recibo recibo;

            if (user != null)
            {
                using var transaction = await _context.Database.BeginTransactionAsync();
                try
                {
                    if (Id != Guid.Empty)
                    {
                        var reciboClone = await _reciboServico.ObterPorIdComColaborador(reciboVM.Id);
                        reciboVM.DataAlteracao = DateTime.Now;
                        recibo = _mapper.Map<Recibo>(reciboVM);
                        recibo.UsuarioAlteracaoId = Guid.Parse(user.Id);

                        await _logAlteracaoServico.CompararAlteracoes(reciboClone, recibo, Guid.Parse(user.Id), $"Recibo[{recibo.Id}]");
                        await _reciboServico.Atualizar(recibo);

                    }
                    else
                    {
                        recibo = _mapper.Map<Recibo>(reciboVM);
                        recibo.UsuarioCadastroId = Guid.Parse(user.Id);
                        recibo.Numero = await _reciboServico.ObterNumeroUltimoRecibo() + 1;
                        recibo.Fazenda = user.Fazenda;
                        await _reciboServico.Adicionar(recibo);
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
            return View(reciboVM);
        }

        [HttpPost]
        [Route("excluir-recibo/{id:guid}")]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> Deletar(Guid id)
        {
            var recibo = await _reciboServico.ObterPorIdComColaborador(id);
            IdentityUser? user = await _userManager.GetUserAsync(User);
            if (recibo == null) return NotFound();
            if (user == null) return NotFound();

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                await _logAlteracaoServico.RegistrarLogDiretamente($"Registro: {recibo.Colaborador?.RazaoSocial} excluído.", Guid.Parse(user.Id), $"Recibo[{recibo.Id}]");
                await _reciboServico.Remover(id);
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception(ex.Message);
            }

            if (!OperacaoValida()) return View(recibo);

            return RedirectToAction("Index");
        }

        private async Task<ReciboVM> PopularColaboradores(ReciboVM recibo)
        {
            Usuario? user = await _userManager.GetUserAsync(User);
            var colaboradores = await _colaboradorServico.Buscar(x => x.Situacao == Situacao.Ativo && x.Fazenda == user.Fazenda);
            var colaboradoresVM = _mapper.Map<IEnumerable<ColaboradorVM>>(colaboradores);
            colaboradoresVM = colaboradoresVM.OrderBy(x => x.RazaoSocial);
            recibo.Colaboradores = colaboradoresVM;

            return recibo;
        }

        [Route("imprimir-recibo/{id:guid}")]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> ImprimirRecibo(Guid id)
        {
            var recibo = await _reciboServico.ObterPorIdComColaborador(id);
            var reciboVM = _mapper.Map<ReciboVM>(recibo);
            var user = await _userManager.GetUserAsync(User);
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
                var titleCell = new PdfPCell(new Phrase("Recibo de Pagamento", titleFont))
                {
                    Colspan = 4, // A célula ocupa as 2 colunas da tabela
                    HorizontalAlignment = Element.ALIGN_CENTER, // Centraliza o texto
                    BorderWidth = 1f, // Adiciona borda à célula
                    Padding = 10f, // Adiciona espaço dentro da célula
                    //SpacingAfter = 10f // Espaçamento após a célula
                };
                table.AddCell(titleCell);

                // Adiciona as células à tabela
                table.AddCell(new PdfPCell(new Phrase("Número:", estiloNumValor)) { BorderWidth = 1f });
                table.AddCell(new PdfPCell(new Phrase(reciboVM.Numero.ToString(), estiloNumValor)) { BorderWidth = 1f });

                table.AddCell(new PdfPCell(new Phrase("Valor:", estiloNumValor)) { BorderWidth = 1f });
                table.AddCell(new PdfPCell(new Phrase(reciboVM.Valor.ToString("C"), estiloNumValor)) { BorderWidth = 1f });

                table.AddCell(new PdfPCell(new Phrase("Recebi(emos) de ", estiloPadrao)) { BorderWidth = 1f, Colspan = 1 });
                table.AddCell(new PdfPCell(new Phrase("Haroldo de Sá Quartim Barbosa", estiloNomes)) { BorderWidth = 1f, Colspan = 3 });

                table.AddCell(new PdfPCell(new Phrase("Endereço ", estiloPadrao)) { BorderWidth = 1f, Colspan = 1 });
                table.AddCell(new PdfPCell(new Phrase(user.Fazenda.GetEnumDisplayName(), estiloNomes)) { BorderWidth = 1f, Colspan = 3 });

                table.AddCell(new PdfPCell(new Phrase(" ", estiloPadrao)) { BorderWidth = 1f, Colspan = 1 });
                table.AddCell(new PdfPCell(new Phrase(" ", estiloPadrao)) { BorderWidth = 1f, Colspan = 3 });

                table.AddCell(new PdfPCell(new Phrase("A importância de ", estiloPadrao)) { BorderWidth = 1f, Colspan = 1 });
                table.AddCell(new PdfPCell(new Phrase(reciboVM.ValorPorExtenso, estiloPadrao)) { BorderWidth = 1f, Colspan = 3 });

                table.AddCell(new PdfPCell(new Phrase("Referente:", estiloPadrao)) { BorderWidth = 1f, Colspan = 1 });
                table.AddCell(new PdfPCell(new Phrase(reciboVM.Referente, estiloPadrao)) { BorderWidth = 1f, Colspan = 3 });

                table.AddCell(new PdfPCell(new Phrase(" ", estiloPadrao)) { BorderWidth = 1f, Colspan = 1 });
                table.AddCell(new PdfPCell(new Phrase(" ", estiloPadrao)) { BorderWidth = 1f, Colspan = 3 });

                table.AddCell(new PdfPCell(new Phrase("Cheque Nº:", estiloPadrao)) { BorderWidth = 1f });
                table.AddCell(new PdfPCell(new Phrase(reciboVM.NumeroCheque, estiloPadrao)) { BorderWidth = 1f });

                table.AddCell(new PdfPCell(new Phrase("Agência:", estiloPadrao)) { BorderWidth = 1f });
                table.AddCell(new PdfPCell(new Phrase(reciboVM.NumeroCheque, estiloPadrao)) { BorderWidth = 1f });

                table.AddCell(new PdfPCell(new Phrase("Banco:", estiloPadrao)) { BorderWidth = 1f, Colspan = 1 });
                table.AddCell(new PdfPCell(new Phrase(reciboVM.NumeroCheque, estiloPadrao)) { BorderWidth = 1f, Colspan = 3 });

                table.AddCell(new PdfPCell(new Phrase(" ", estiloPadrao)) { BorderWidth = 1f, Colspan = 1 });
                table.AddCell(new PdfPCell(new Phrase(" ", estiloPadrao)) { BorderWidth = 1f, Colspan = 3 });

                table.AddCell(new PdfPCell(new Phrase(reciboVM.Colaborador?.RazaoSocial, estiloPadrao)) { BorderWidth = 1f, Colspan = 2, HorizontalAlignment = Element.ALIGN_CENTER });
                table.AddCell(new PdfPCell(new Phrase(reciboVM.Data.ToLongDateString(), estiloPadrao)) { BorderWidth = 1f, Colspan = 2, HorizontalAlignment = Element.ALIGN_CENTER });

                table.AddCell(new PdfPCell(new Phrase(reciboVM.Colaborador?.DocumentoFormatado, estiloPadrao)) { BorderWidth = 1f, Colspan = 2, HorizontalAlignment = Element.ALIGN_CENTER });
                table.AddCell(new PdfPCell(new Phrase("Ass.", estiloPadrao)) { BorderWidth = 1f, Colspan = 2 });
                
                // Adiciona a tabela ao documento
                document.Add(table);

                // Adiciona um rodapé ao PDF
                //var footer = new Paragraph("Obrigado pelo pagamento!", contentFont)
                //{
                //    Alignment = Element.ALIGN_CENTER,
                //    SpacingBefore = 20
                //};
                //document.Add(footer);

                // Fecha o documento
                document.Close();

                // Retorna o PDF para exibição inline no navegador
                var pdfBytes = memoryStream.ToArray();
                var contentDisposition = new ContentDispositionHeaderValue("inline")
                {
                    FileName = "Recibo.pdf"
                };

                Response.Headers[HeaderNames.ContentDisposition] = contentDisposition.ToString();
                Response.ContentType = "application/pdf";

                return new FileContentResult(pdfBytes, "application/pdf");
               
            }
        }

        //public SelectList CarregaColaboradores(int? colaboradorId = null)
        //{
        //    var user = _userManager.GetUserAsync(User);
        //    var colaboradores = _context.Colaboradores
        //        .Where(x=> x.Fazenda == user.Result.Fazenda)
        //                                .OrderBy(c => c.RazaoSocial)  // Ordenar pelo nome se necessário
        //                                .ToList();
        //    return new SelectList(colaboradores, "Id", "RazaoSocial", colaboradorId);
        //}
    }
}
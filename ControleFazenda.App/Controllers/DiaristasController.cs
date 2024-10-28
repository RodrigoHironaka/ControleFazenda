using AutoMapper;
using ControleFazenda.Business.Entidades;
using ControleFazenda.Business.Interfaces.Servicos;
using ControleFazenda.Business.Interfaces;
using ControleFazenda.Data.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ControleFazenda.App.ViewModels;
using ControleFazenda.Business.Entidades.Enum;
using ControleFazenda.Business.Servicos;
using ControleFazenda.App.Extensions;
using iTextSharp.text.pdf;
using iTextSharp.text;
using Microsoft.Net.Http.Headers;

namespace ControleFazenda.App.Controllers
{
    public class DiaristasController : BaseController
    {
        private readonly IDiaristaServico _diaristaServico;
        private readonly IDiariaServico _diariaServico;
        private readonly IColaboradorServico _colaboradorServico;
        private readonly IMapper _mapper;
        private readonly UserManager<Usuario> _userManager;
        private readonly ContextoPrincipal _context;
        private readonly ILogAlteracaoServico _logAlteracaoServico;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public DiaristasController(IMapper mapper,
                                  IDiaristaServico diaristaServico,
                                  IDiariaServico diariaServico,
                                  IColaboradorServico colaboradorServico,
                                  UserManager<Usuario> userManager,
                                  ContextoPrincipal context,
                                  ILogAlteracaoServico logAlteracaoServico,
                                  IWebHostEnvironment webHostEnvironment,
                                  INotificador notificador) : base(notificador)
        {
            _mapper = mapper;
            _diaristaServico = diaristaServico;
            _diariaServico = diariaServico;
            _colaboradorServico = colaboradorServico;
            _userManager = userManager;
            _context = context;
            _logAlteracaoServico = logAlteracaoServico;
            _webHostEnvironment = webHostEnvironment;
        }

        [Route("lista-de-diaristas")]
        public async Task<IActionResult> Index()
        {
            Usuario? user = await _userManager.GetUserAsync(User);
            var diaristas = await _diaristaServico.ObterTodosComColaborador();
            var diaristasVM = _mapper.Map<List<DiaristaVM>>(diaristas);
            var diaristasFazenda = new List<DiaristaVM>();
            if (user != null && user.AcessoTotal == false)
            {
                foreach (var item in diaristasVM)
                {
                    Usuario? usuario = await _userManager.FindByIdAsync(item.UsuarioCadastroId.ToString());
                    if (usuario?.Fazenda == user.Fazenda)
                        diaristasFazenda.Add(item);
                }

                return View(diaristasFazenda);
            }
            else
            {
                return View(diaristasVM);
            }
        }

        [Route("editar-diarista/{id}")]
        public async Task<IActionResult> Editar(Guid Id)
        {
            var diaristaVM = new DiaristaVM();
            if (Id != Guid.Empty)
            {
                var diarista = await _diaristaServico.ObterPorIdComColaborador(Id);
                if (diarista == null) return NotFound();

                diaristaVM = _mapper.Map<DiaristaVM>(diarista);
                diaristaVM.UsuarioCadastro = await _userManager.FindByIdAsync(diaristaVM.UsuarioCadastroId.ToString());
                diaristaVM.UsuarioAlteracao = await _userManager.FindByIdAsync(diaristaVM.UsuarioAlteracaoId.ToString());
                diaristaVM = await PopularColaboradores(diaristaVM);
            }
            else
            {
                diaristaVM = await PopularColaboradores(diaristaVM);
            }

            return View(diaristaVM);
        }

        [Route("editar-diarista/{id:guid}")]
        [HttpPost]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> Editar(Guid Id, DiaristaVM diaristaVM)
        {
            if (Id != diaristaVM.Id) return NotFound();
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return Json(new { success = false, errors, isModelState = true });
            }

            Usuario? user = await _userManager.GetUserAsync(User);
            Diarista diarista;

            if (user != null)
            {
                using var transaction = await _context.Database.BeginTransactionAsync();
                try
                {
                    if (Id != Guid.Empty)
                    {
                        var diaristaClone = await _diaristaServico.ObterPorIdComColaborador(Id);
                        diarista = await _diaristaServico.ObterPorIdComColaborador(Id);
                        foreach (var item in diarista.Diarias)
                            await _diariaServico.Remover(item.Id);

                        diaristaVM.DataAlteracao = DateTime.Now;
                        diarista = _mapper.Map<Diarista>(diaristaVM);
                        diarista.UsuarioAlteracaoId = Guid.Parse(user.Id);
                        await _logAlteracaoServico.CompararAlteracoes(diaristaClone, diarista, Guid.Parse(user.Id), $"Diarista[{diarista.Id}]");
                        await _diaristaServico.Atualizar(diarista);
                    }
                    else
                    {
                        diarista = _mapper.Map<Diarista>(diaristaVM);
                        diarista.UsuarioCadastroId = Guid.Parse(user.Id);
                        diarista.Fazenda = user.Fazenda;
                        await _diaristaServico.Adicionar(diarista);
                    }

                    if (!OperacaoValida())
                    {
                        await transaction.RollbackAsync();
                        List<string> errors = new List<string>();
                        errors = _notificador.ObterNotificacoes().Select(x => x.Mensagem).ToList();
                        return Json(new { success = false, errors });
                    }
                    await transaction.CommitAsync();
                    return Json(new { success = true });
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    return Json(new { success = false, errors = ex.Message });
                }
            }
            return View(diaristaVM);
        }

        [HttpPost]
        [Route("excluir-diarista/{id:guid}")]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> Deletar(Guid id)
        {
            var diarista = await _diaristaServico.ObterPorIdComColaborador(id);
            if (diarista == null) return NotFound();
            foreach (var item in diarista.Diarias)
                await _diariaServico.Remover(item.Id);

            IdentityUser? user = await _userManager.GetUserAsync(User);

            if (user == null) return NotFound();

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                await _logAlteracaoServico.RegistrarLogDiretamente($"Registro: {diarista.Colaborador?.RazaoSocial} excluído.", Guid.Parse(user.Id), $"Diarista[{diarista.Id}]");
                await _diaristaServico.Remover(id);
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception(ex.Message);
            }

            if (!OperacaoValida())
            {
                await transaction.RollbackAsync();
                var errors = _notificador.ObterNotificacoes().Select(x => x.Mensagem).ToList();
                return Json(new { success = false, errors });
            }

            return RedirectToAction("Index");
        }

        private async Task<DiaristaVM> PopularColaboradores(DiaristaVM diarista)
        {
            Usuario? user = await _userManager.GetUserAsync(User);
            var colaboradores = await _colaboradorServico.Buscar(x => x.Situacao == Situacao.Ativo && x.Fazenda == user.Fazenda);
            var colaboradoresVM = _mapper.Map<IEnumerable<ColaboradorVM>>(colaboradores);
            colaboradoresVM = colaboradoresVM.OrderBy(x => x.RazaoSocial);
            diarista.Colaboradores = colaboradoresVM;

            return diarista;
        }

        [Route("imprimir-diarias/{id:guid}")]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> ImprimirDiarias(Guid id)
        {
            var diarista = await _diaristaServico.ObterPorIdComColaborador(id);
            var diaristaVM = _mapper.Map<DiaristaVM>(diarista);
            var user = await _userManager.GetUserAsync(User);
            using (var memoryStream = new MemoryStream())
            {
                // Cria o documento PDF
                var document = new Document(PageSize.A4, 50, 50, 25, 25);
                var writer = PdfWriter.GetInstance(document, memoryStream);
                document.Open();

                // Adiciona título ao PDF
                var table = new PdfPTable(6);
                table.WidthPercentage = 100;
                table.SpacingBefore = 20;
                table.SpacingAfter = 20;

                // Define fontes para os textos
                var titleFont = FontFactory.GetFont("Arial", 18, Font.BOLD);
                var estiloPadrao = FontFactory.GetFont("Arial", 12, Font.NORMAL);
                var estiloNumValor = FontFactory.GetFont("Arial", 14, Font.BOLD);
                var estiloNomes = FontFactory.GetFont("Arial", 14, Font.BOLD);

                // Adiciona o título à tabela, ocupando 2 colunas
                var titleCell = new PdfPCell(new Phrase($"{user.Fazenda.ToString()} - Controle de Horas Diarista", titleFont))
                {
                    Colspan = 6, // A célula ocupa as 2 colunas da tabela
                    HorizontalAlignment = Element.ALIGN_CENTER, // Centraliza o texto
                    BorderWidth = 1f, // Adiciona borda à célula
                    Padding = 10f, // Adiciona espaço dentro da célula
                    //SpacingAfter = 10f // Espaçamento após a célula
                };
                table.AddCell(titleCell);

                // Adiciona as células à tabela
                table.AddCell(new PdfPCell(new Phrase("Colaborador:", estiloNumValor)) { BorderWidth = 1f, Colspan = 2 });
                table.AddCell(new PdfPCell(new Phrase(diaristaVM.Colaborador.RazaoSocial, estiloPadrao)) { BorderWidth = 1f, Colspan = 4 });

                table.AddCell(new PdfPCell(new Phrase("Mês/Ano:", estiloNumValor)) { BorderWidth = 1f, Colspan = 2 });

                var primeiroMesDaLista = diaristaVM.Diarias.OrderBy(x => x.Data)?.First()?.Data.Value.Month;
                var ultimoMesDaLista = diaristaVM.Diarias?.OrderBy(x => x.Data).Last()?.Data.Value.Month;
                var mesAno = string.Empty;
                if (primeiroMesDaLista == ultimoMesDaLista)
                    mesAno = $"{diaristaVM.Diarias?.First()?.Data.Value.Month}/{diaristaVM.Diarias?.First()?.Data.Value.Year}";
                else
                    mesAno = $"{primeiroMesDaLista}/{diaristaVM.Diarias?.OrderBy(x => x.Data).First()?.Data.Value.Year} e {ultimoMesDaLista}/{diaristaVM.Diarias?.OrderBy(x => x.Data).Last()?.Data.Value.Year}";

                table.AddCell(new PdfPCell(new Phrase(mesAno, estiloPadrao)) { BorderWidth = 1f, Colspan = 4 });

                table.AddCell(new PdfPCell(new Phrase("Descrição:", estiloNumValor)) { BorderWidth = 1f, Colspan = 2 });
                table.AddCell(new PdfPCell(new Phrase(diaristaVM.Descricao, estiloPadrao)) { BorderWidth = 1f, Colspan = 4 });

                string[] cabecalhos = { "Data", "Hr Entrada", "Hr Saída", "Hr Entrada", "Hr Saída", "Valor" };
                foreach (var cabecalho in cabecalhos)
                {
                    table.AddCell(new PdfPCell(new Phrase(cabecalho, estiloNumValor))
                    {
                        BorderWidth = 1f,
                        HorizontalAlignment = Element.ALIGN_CENTER,
                        Padding = 5f
                    });
                }

                foreach (var item in diaristaVM.Diarias)
                {
                    table.AddCell(new PdfPCell(new Phrase(item.Data.Value.ToShortDateString(), estiloPadrao))
                    {
                        BorderWidth = 1f,
                        HorizontalAlignment = Element.ALIGN_CENTER
                    });

                    table.AddCell(new PdfPCell(new Phrase(item.EntradaManha.Value.ToString("hh\\:mm"), estiloPadrao))
                    {
                        BorderWidth = 1f,
                        HorizontalAlignment = Element.ALIGN_CENTER
                    });

                    table.AddCell(new PdfPCell(new Phrase(item.SaidaManha.Value.ToString("hh\\:mm"), estiloPadrao))
                    {
                        BorderWidth = 1f,
                        HorizontalAlignment = Element.ALIGN_CENTER
                    });

                    table.AddCell(new PdfPCell(new Phrase(item.EntradaTarde.Value.ToString("hh\\:mm"), estiloPadrao))
                    {
                        BorderWidth = 1f,
                        HorizontalAlignment = Element.ALIGN_CENTER
                    });

                    table.AddCell(new PdfPCell(new Phrase(item.SaidaTarde.Value.ToString("hh\\:mm"), estiloPadrao))
                    {
                        BorderWidth = 1f,
                        HorizontalAlignment = Element.ALIGN_CENTER
                    });

                    table.AddCell(new PdfPCell(new Phrase(item.Valor.ToString("N2"), estiloPadrao))
                    {
                        BorderWidth = 1f,
                        HorizontalAlignment = Element.ALIGN_CENTER
                    });
                }
                table.AddCell(new PdfPCell(new Phrase("Ass.", estiloNumValor)) { BorderWidth = 1f, Colspan = 5 });
                table.AddCell(new PdfPCell(new Phrase(diaristaVM.TotalDiarias, estiloPadrao)) { BorderWidth = 1f, Colspan = 1, HorizontalAlignment = Element.ALIGN_CENTER, });
                
                // Adiciona a tabela ao documento
                document.Add(table);

                // Fecha o documento
                document.Close();

                // Retorna o PDF para exibição inline no navegador
                var pdfBytes = memoryStream.ToArray();
                var contentDisposition = new ContentDispositionHeaderValue("inline")
                {
                    FileName = "Diarias.pdf"
                };

                Response.Headers[HeaderNames.ContentDisposition] = contentDisposition.ToString();
                Response.ContentType = "application/pdf";

                return new FileContentResult(pdfBytes, "application/pdf");

            }
        }
    }
}

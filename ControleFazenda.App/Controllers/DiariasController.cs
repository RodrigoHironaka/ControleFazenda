using AutoMapper;
using ControleFazenda.App.ViewModels;
using ControleFazenda.Business.Entidades;
using ControleFazenda.Business.Interfaces.Servicos;
using ControleFazenda.Business.Interfaces;
using ControleFazenda.Data.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ControleFazenda.Business.Entidades.Enum;
using ControleFazenda.App.Extensions;
using ControleFazenda.Business.Servicos;
using iTextSharp.text.pdf;
using iTextSharp.text;
using Microsoft.Net.Http.Headers;

namespace ControleFazenda.App.Controllers
{
    public class DiariasController : BaseController
    {
        private readonly IDiariaServico _diariaServico;
        private readonly IColaboradorServico _colaboradorServico;
        private readonly IMapper _mapper;
        private readonly UserManager<Usuario> _userManager;
        private readonly ContextoPrincipal _context;
        private readonly ILogAlteracaoServico _logAlteracaoServico;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public DiariasController(IMapper mapper,
                                  IDiariaServico diariaServico,
                                  IColaboradorServico colaboradorServico,
                                  UserManager<Usuario> userManager,
                                  ContextoPrincipal context,
                                  ILogAlteracaoServico logAlteracaoServico,
                                  IWebHostEnvironment webHostEnvironment,
                                  INotificador notificador) : base(notificador)
        {
            _mapper = mapper;
            _diariaServico = diariaServico;
            _colaboradorServico = colaboradorServico;
            _userManager = userManager;
            _context = context;
            _logAlteracaoServico = logAlteracaoServico;
            _webHostEnvironment = webHostEnvironment;
        }

        [Route("lista-de-diarias")]
        public async Task<IActionResult> Index()
        {
            Usuario? user = await _userManager.GetUserAsync(User);
            var diarias = await _diariaServico.ObterTodosComColaborador();
            var diariasVM = _mapper.Map<List<DiariaVM>>(diarias);
            var diariasFazenda = new List<DiariaVM>();
            if (user != null && user.AcessoTotal == false)
            {
                foreach (var item in diariasVM)
                {
                    Usuario? usuario = await _userManager.FindByIdAsync(item.UsuarioCadastroId.ToString());
                    if (usuario?.Fazenda == user.Fazenda)
                        diariasFazenda.Add(item);
                }
                return View(diariasFazenda);
            }
            else
            {
                //diarias = await _diariaServico.ObterTodos();
                //diariasVM = _mapper.Map<List<DiariaVM>>(diarias);
                return View(diariasVM);
            }
        }

        [Route("editar-diaria/{id}")]
        public async Task<IActionResult> Editar(Guid Id)
        {
            var diariaVM = new DiariaVM();
            if (Id != Guid.Empty)
            {
                var diaria = await _diariaServico.ObterPorIdComColaborador(Id);
                if (diaria == null) return NotFound();

                diariaVM = _mapper.Map<DiariaVM>(diaria);
                diariaVM.UsuarioCadastro = await _userManager.FindByIdAsync(diariaVM.UsuarioCadastroId.ToString());
                diariaVM.UsuarioAlteracao = await _userManager.FindByIdAsync(diariaVM.UsuarioAlteracaoId.ToString());
                diariaVM = await PopularColaboradores(diariaVM);
            }
            else
                diariaVM = await PopularColaboradores(diariaVM);

            return View(diariaVM);
        }

        [Route("editar-diaria/{id:guid}")]
        [HttpPost]
        public async Task<IActionResult> Editar(Guid Id, DiariaVM diariaVM)
        {
            if (Id != diariaVM.Id) return NotFound();
            if (!ModelState.IsValid)
            {
                var errors = ModelState.ToDictionary(kvp => kvp.Key, kvp => kvp.Value?.Errors.Select(e => e.ErrorMessage).ToList());
                return Json(new { success = false, errors, isModelState = true });
            }

            Usuario? user = await _userManager.GetUserAsync(User);
            Diaria diaria;

            if (user != null)
            {
                using var transaction = await _context.Database.BeginTransactionAsync();
                try
                {
                    bool periodoInteiro = true;
                    Int32 horasTrabalhadas = 0;
                    if (diariaVM.EntradaManha != null && diariaVM.EntradaManha > TimeSpan.Zero &&
                        diariaVM.SaidaManha != null && diariaVM.SaidaManha > TimeSpan.Zero &&
                        diariaVM.EntradaTarde != null && diariaVM.EntradaTarde > TimeSpan.Zero &&
                        diariaVM.SaidaTarde != null && diariaVM.SaidaTarde > TimeSpan.Zero)
                    {
                        periodoInteiro = true;
                        horasTrabalhadas = 8;
                    }
                    else if (diariaVM.EntradaManha != null && diariaVM.EntradaManha > TimeSpan.Zero &&
                        diariaVM.SaidaManha != null && diariaVM.SaidaManha > TimeSpan.Zero &&
                        diariaVM.EntradaTarde == null || diariaVM.EntradaTarde == TimeSpan.Zero &&
                        diariaVM.SaidaTarde == null || diariaVM.SaidaTarde == TimeSpan.Zero)
                    {
                        periodoInteiro = false;
                        horasTrabalhadas = 4;
                    }
                    else if (diariaVM.EntradaManha == null && diariaVM.EntradaManha == TimeSpan.Zero &&
                        diariaVM.SaidaManha == null && diariaVM.SaidaManha == TimeSpan.Zero &&
                        diariaVM.EntradaTarde != null || diariaVM.EntradaTarde > TimeSpan.Zero &&
                        diariaVM.SaidaTarde != null || diariaVM.SaidaTarde > TimeSpan.Zero)
                    {
                        periodoInteiro = false;
                        horasTrabalhadas = 4;
                    }

                    if (Id != Guid.Empty)
                    {
                        var diariaClone = await _diariaServico.ObterPorIdComColaborador(diariaVM.Id);
                        diariaVM.DataAlteracao = DateTime.Now;
                        diaria = _mapper.Map<Diaria>(diariaVM);
                        diaria.UsuarioAlteracaoId = Guid.Parse(user.Id);
                        if (periodoInteiro)
                        {
                            diaria.TipoPeriodo = TipoPeriodo.Inteiro;
                            diaria.HorasTabalhadas = horasTrabalhadas;
                        }
                        else
                        {
                            diaria.TipoPeriodo = TipoPeriodo.Meio;
                            diaria.HorasTabalhadas = horasTrabalhadas;
                        }

                        await _logAlteracaoServico.CompararAlteracoes(diariaClone, diaria, Guid.Parse(user.Id), $"Recibo[{diaria.Id}]");
                        await _diariaServico.Atualizar(diaria);

                    }
                    else
                    {
                        var identificador = string.Empty;
                        for (int i = 1; i <= diariaVM.QuantosDias; i++)
                        {
                            diaria = new Diaria();
                            diaria = _mapper.Map<Diaria>(diariaVM);
                            diaria.UsuarioCadastroId = Guid.Parse(user.Id);
                            if(i>1)
                                diaria.Data = diaria.Data.AddDays(i-1);
                            if (periodoInteiro)
                            {
                                diaria.TipoPeriodo = TipoPeriodo.Inteiro;
                                diaria.HorasTabalhadas = horasTrabalhadas;
                            }
                            else
                            {
                                diaria.TipoPeriodo = TipoPeriodo.Meio;
                                diaria.HorasTabalhadas = horasTrabalhadas;
                            }
                            diaria.Fazenda = user.Fazenda;
                            await _diariaServico.Adicionar(diaria);
                            if(i==1)
                                identificador = diaria.Id.ToString();
                            diaria.Identificador = identificador;
                            await _diariaServico.Atualizar(diaria);
                        }
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
            return View(diariaVM);
        }

        [HttpPost]
        [Route("excluir-diaria/{id:guid}")]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> Deletar(Guid id)
        {
            var diaria = await _diariaServico.ObterPorIdComColaborador(id);
            IdentityUser? user = await _userManager.GetUserAsync(User);
            if (diaria == null) return NotFound();
            if (user == null) return NotFound();

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                await _logAlteracaoServico.RegistrarLogDiretamente($"Registro: {diaria.Colaborador?.RazaoSocial} excluído.", Guid.Parse(user.Id), $"Diária[{diaria.Id}]");
                await _diariaServico.Remover(id);
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception(ex.Message);
            }

            if (!OperacaoValida()) return View(diaria);

            return RedirectToAction("Index");
        }

        private async Task<DiariaVM> PopularColaboradores(DiariaVM diaria)
        {
            Usuario? user = await _userManager.GetUserAsync(User);
            var colaboradores = await _colaboradorServico.Buscar(x => x.Situacao == Situacao.Ativo && x.Fazenda == user.Fazenda);
            var colaboradoresVM = _mapper.Map<IEnumerable<ColaboradorVM>>(colaboradores);
            colaboradoresVM = colaboradoresVM.OrderBy(x => x.RazaoSocial);
            diaria.Colaboradores = colaboradoresVM;

            return diaria;
        }


        //[Route("imprimir-diaria")]
        //[IgnoreAntiforgeryToken]
        //public async Task<IActionResult> ImprimirDiaria([FromQuery] IList<Guid> ids)
        //{
        //    return View();
        //    //var recibo = await _reciboServico.ObterPorIdComColaborador(id);
        //    //var reciboVM = _mapper.Map<ReciboVM>(recibo);
        //    //var user = await _userManager.GetUserAsync(User);
        //    //using (var memoryStream = new MemoryStream())
        //    //{
        //    //    // Cria o documento PDF
        //    //    var document = new Document(PageSize.A4, 50, 50, 25, 25);
        //    //    var writer = PdfWriter.GetInstance(document, memoryStream);
        //    //    document.Open();

        //    //    // Adiciona título ao PDF
        //    //    var table = new PdfPTable(4);
        //    //    table.WidthPercentage = 100;
        //    //    table.SpacingBefore = 20;
        //    //    table.SpacingAfter = 20;

        //    //    // Define fontes para os textos
        //    //    var titleFont = FontFactory.GetFont("Arial", 18, Font.BOLD);
        //    //    var estiloPadrao = FontFactory.GetFont("Arial", 12, Font.NORMAL);
        //    //    var estiloNumValor = FontFactory.GetFont("Arial", 14, Font.BOLD);
        //    //    var estiloNomes = FontFactory.GetFont("Arial", 14, Font.BOLD);

        //    //    // Adiciona o título à tabela, ocupando 2 colunas
        //    //    var titleCell = new PdfPCell(new Phrase("Recibo de Pagamento", titleFont))
        //    //    {
        //    //        Colspan = 4, // A célula ocupa as 2 colunas da tabela
        //    //        HorizontalAlignment = Element.ALIGN_CENTER, // Centraliza o texto
        //    //        BorderWidth = 1f, // Adiciona borda à célula
        //    //        Padding = 10f, // Adiciona espaço dentro da célula
        //    //        //SpacingAfter = 10f // Espaçamento após a célula
        //    //    };
        //    //    table.AddCell(titleCell);

        //    //    // Adiciona as células à tabela
        //    //    table.AddCell(new PdfPCell(new Phrase("Número:", estiloNumValor)) { BorderWidth = 1f });
        //    //    table.AddCell(new PdfPCell(new Phrase(reciboVM.Numero.ToString(), estiloNumValor)) { BorderWidth = 1f });

        //    //    table.AddCell(new PdfPCell(new Phrase("Valor:", estiloNumValor)) { BorderWidth = 1f });
        //    //    table.AddCell(new PdfPCell(new Phrase(reciboVM.Valor.ToString("C"), estiloNumValor)) { BorderWidth = 1f });

        //    //    table.AddCell(new PdfPCell(new Phrase("Recebi(emos) de ", estiloPadrao)) { BorderWidth = 1f, Colspan = 1 });
        //    //    table.AddCell(new PdfPCell(new Phrase("Haroldo de Sá Quartim Barbosa", estiloNomes)) { BorderWidth = 1f, Colspan = 3 });

        //    //    table.AddCell(new PdfPCell(new Phrase("Endereço ", estiloPadrao)) { BorderWidth = 1f, Colspan = 1 });
        //    //    table.AddCell(new PdfPCell(new Phrase(user.Fazenda.GetEnumDisplayName(), estiloNomes)) { BorderWidth = 1f, Colspan = 3 });

        //    //    table.AddCell(new PdfPCell(new Phrase(" ", estiloPadrao)) { BorderWidth = 1f, Colspan = 1 });
        //    //    table.AddCell(new PdfPCell(new Phrase(" ", estiloPadrao)) { BorderWidth = 1f, Colspan = 3 });

        //    //    table.AddCell(new PdfPCell(new Phrase("A importância de ", estiloPadrao)) { BorderWidth = 1f, Colspan = 1 });
        //    //    table.AddCell(new PdfPCell(new Phrase(reciboVM.ValorPorExtenso, estiloPadrao)) { BorderWidth = 1f, Colspan = 3 });

        //    //    table.AddCell(new PdfPCell(new Phrase("Referente:", estiloPadrao)) { BorderWidth = 1f, Colspan = 1 });
        //    //    table.AddCell(new PdfPCell(new Phrase(reciboVM.Referente, estiloPadrao)) { BorderWidth = 1f, Colspan = 3 });

        //    //    table.AddCell(new PdfPCell(new Phrase(" ", estiloPadrao)) { BorderWidth = 1f, Colspan = 1 });
        //    //    table.AddCell(new PdfPCell(new Phrase(" ", estiloPadrao)) { BorderWidth = 1f, Colspan = 3 });

        //    //    table.AddCell(new PdfPCell(new Phrase("Cheque Nº:", estiloPadrao)) { BorderWidth = 1f });
        //    //    table.AddCell(new PdfPCell(new Phrase(reciboVM.NumeroCheque, estiloPadrao)) { BorderWidth = 1f });

        //    //    table.AddCell(new PdfPCell(new Phrase("Agência:", estiloPadrao)) { BorderWidth = 1f });
        //    //    table.AddCell(new PdfPCell(new Phrase(reciboVM.NumeroCheque, estiloPadrao)) { BorderWidth = 1f });

        //    //    table.AddCell(new PdfPCell(new Phrase("Banco:", estiloPadrao)) { BorderWidth = 1f, Colspan = 1 });
        //    //    table.AddCell(new PdfPCell(new Phrase(reciboVM.NumeroCheque, estiloPadrao)) { BorderWidth = 1f, Colspan = 3 });

        //    //    table.AddCell(new PdfPCell(new Phrase(" ", estiloPadrao)) { BorderWidth = 1f, Colspan = 1 });
        //    //    table.AddCell(new PdfPCell(new Phrase(" ", estiloPadrao)) { BorderWidth = 1f, Colspan = 3 });

        //    //    table.AddCell(new PdfPCell(new Phrase(reciboVM.Colaborador?.RazaoSocial, estiloPadrao)) { BorderWidth = 1f, Colspan = 2, HorizontalAlignment = Element.ALIGN_CENTER });
        //    //    table.AddCell(new PdfPCell(new Phrase(reciboVM.Data.ToLongDateString(), estiloPadrao)) { BorderWidth = 1f, Colspan = 2, HorizontalAlignment = Element.ALIGN_CENTER });

        //    //    table.AddCell(new PdfPCell(new Phrase(reciboVM.Colaborador?.DocumentoFormatado, estiloPadrao)) { BorderWidth = 1f, Colspan = 2, HorizontalAlignment = Element.ALIGN_CENTER });
        //    //    table.AddCell(new PdfPCell(new Phrase("Ass.", estiloPadrao)) { BorderWidth = 1f, Colspan = 2 });

        //    //    // Adiciona a tabela ao documento
        //    //    document.Add(table);

        //    //    // Adiciona um rodapé ao PDF
        //    //    //var footer = new Paragraph("Obrigado pelo pagamento!", contentFont)
        //    //    //{
        //    //    //    Alignment = Element.ALIGN_CENTER,
        //    //    //    SpacingBefore = 20
        //    //    //};
        //    //    //document.Add(footer);

        //    //    // Fecha o documento
        //    //    document.Close();

        //    //    // Retorna o PDF para exibição inline no navegador
        //    //    var pdfBytes = memoryStream.ToArray();
        //    //    var contentDisposition = new ContentDispositionHeaderValue("inline")
        //    //    {
        //    //        FileName = "Recibo.pdf"
        //    //    };

        //    //    Response.Headers[HeaderNames.ContentDisposition] = contentDisposition.ToString();
        //    //    Response.ContentType = "application/pdf";

        //        //return new FileContentResult(pdfBytes, "application/pdf");

        //    //}
        //}
    }
}

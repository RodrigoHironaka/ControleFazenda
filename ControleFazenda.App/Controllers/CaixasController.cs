using AutoMapper;
using ControleFazenda.App.ViewModels;
using ControleFazenda.Business.Entidades;
using ControleFazenda.Business.Entidades.Enum;
using ControleFazenda.Business.Interfaces;
using ControleFazenda.Business.Interfaces.Servicos;
using ControleFazenda.Business.Servicos;
using ControleFazenda.Data.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ControleFazenda.App.Controllers
{
    [Authorize]
    public class CaixasController : BaseController
    {
        private readonly ICaixaServico _caixaServico;
        private readonly IFormaPagamentoServico _formaPagamentoServico;
        private readonly IMapper _mapper;
        private readonly UserManager<Usuario> _userManager;
        private readonly ContextoPrincipal _context;
        private readonly ILogAlteracaoServico _logAlteracaoServico;

        public CaixasController(IMapper mapper,
                                  ICaixaServico caixaServico,
                                  IFormaPagamentoServico formaPagamentoServico,
                                  UserManager<Usuario> userManager,
                                  ContextoPrincipal context,
                                  ILogAlteracaoServico logAlteracaoServico,
                                  INotificador notificador) : base(notificador)
        {
            _mapper = mapper;
            _caixaServico = caixaServico;
            _formaPagamentoServico = formaPagamentoServico;
            _userManager = userManager;
            _context = context;
            _logAlteracaoServico = logAlteracaoServico;
        }

        [Route("lista-de-caixas")]
        public async Task<IActionResult> Index(Guid Id)
        {
            Caixa? caixa = new Caixa();
            var caixas = new List<Caixa>();
            CaixaVM caixaVM = new CaixaVM();
            var caixasVM = new List<CaixaVM>();

            var usuLogado = await _userManager.GetUserAsync(User);

            if (usuLogado != null)
            {
                if(Id == Guid.Empty)
                {
                    Usuario? user = await _userManager.GetUserAsync(User);
                    caixas = await _caixaServico.ObterCaixasAberto();
                    caixasVM = _mapper.Map<List<CaixaVM>>(caixas);
                    if (user != null)
                    {
                        foreach (var item in caixasVM)
                        {
                            Usuario? usuario = await _userManager.FindByIdAsync(item.UsuarioCadastroId.ToString());
                            if (usuario?.Fazenda == user.Fazenda)
                            {
                                foreach (var itemFluxo in item.FluxosCaixa)
                                {
                                    if (itemFluxo.FormaPagamento == null)
                                        itemFluxo.FormaPagamento = _mapper.Map<FormaPagamentoVM>(await _formaPagamentoServico.ObterPorId(itemFluxo.FormaPagamentoId));
                                }
                                return View(item);
                            }
                        }
                        
                    }
                    else
                        return NotFound();
                        
                }
                else
                {
                    Usuario? user = await _userManager.GetUserAsync(User);
                    caixas = await _caixaServico.ObterTodosComFluxosDeCaixa();
                    caixasVM = _mapper.Map<List<CaixaVM>>(caixas);
                    if (user != null)
                    {
                        foreach (var item in caixasVM)
                        {
                            Usuario? usuario = await _userManager.FindByIdAsync(item.UsuarioCadastroId.ToString());
                            if (item.Id == Id && usuario?.Fazenda == user.Fazenda)
                                return View(item);
                        }
                    }
                }
            }
            return View(caixaVM);
        }

        [HttpGet]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> PesquisaCaixas()
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                var caixas = await _caixaServico.ObterTodosComFluxosDeCaixa();
                var caixasVM = _mapper.Map<List<CaixaVM>>(caixas);
                var caixasFazendaVM = new List<CaixaVM>();
                if (user != null && user.AcessoTotal == false)
                {
                    foreach (var item in caixasVM)
                    {
                        Usuario? usuario = await _userManager.FindByIdAsync(item.UsuarioCadastroId.ToString());
                        if (usuario?.Fazenda == user.Fazenda)
                            caixasFazendaVM.Add(item);
                    }
                }
                else
                {
                    caixas = await _caixaServico.ObterTodosComFluxosDeCaixa();
                    caixasVM = _mapper.Map<List<CaixaVM>>(caixas);
                    caixasFazendaVM.AddRange(caixasVM);
                }

                return PartialView("_Pesquisa", caixasFazendaVM.OrderByDescending(x => x.Numero));
            }
            catch (Exception ex)
            {
                return Json(new { success = false, errors = ex.Message });
            }
        }

        [HttpPost]
        [Route("abrir-caixa")]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> AbrirCaixa()
        {
            Usuario? user = await _userManager.GetUserAsync(User);
            Caixa caixa = new Caixa();
            if (user != null)
            {
                using var transaction = await _context.Database.BeginTransactionAsync();
                try
                {
                    caixa.Numero = await _caixaServico.ObteNumeroUltimoCaixa(user.Id.ToString()) + 1;
                    caixa.UsuarioCadastroId = Guid.Parse(user.Id);
                    await _caixaServico.Adicionar(caixa);
                    await _logAlteracaoServico.RegistrarLogDiretamente($"Caixa Aberto por: {user.UserName} - Data/Hora: {DateTime.Now}", Guid.Parse(user.Id), $"Caixa[{caixa.Id}]");
                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    return Json(new { success = false, errors = ex.Message });
                }
            }

            return Json(new { success = true, idCaixa = caixa.Id });
        }

        [HttpPost]
        [Route("fechar-caixa")]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> FecharCaixa()
        {

            if (!ModelState.IsValid)
            {
                var errors = ModelState.ToDictionary(kvp => kvp.Key, kvp => kvp.Value?.Errors.Select(e => e.ErrorMessage).ToList());
                return Json(new { success = false, errors, isModelState = true });
            }

            Usuario? user = await _userManager.GetUserAsync(User);
            var caixa = new Caixa();

            if (user != null)
            {
                using var transaction = await _context.Database.BeginTransactionAsync();
                try
                {
                    var caixas = await _caixaServico.ObterCaixasAberto();
                    
                    if (user != null)
                    {
                        foreach (var item in caixas)
                        {
                            Usuario? usuario = await _userManager.FindByIdAsync(item.UsuarioCadastroId.ToString());
                            if (usuario?.Fazenda == user.Fazenda)
                                caixa = item; break;
                        }
                    }

                    if (caixa != null)
                    {
                        caixa.Situacao = SituacaoCaixa.Fechado;
                        caixa.UsuarioAlteracaoId = Guid.Parse(user.Id);
                        await _caixaServico.Atualizar(caixa);
                        await _logAlteracaoServico.RegistrarLogDiretamente($"Caixa Fechado por: {user.UserName} - Data/Hora: {DateTime.Now}", Guid.Parse(user.Id), $"Caixa[{caixa.Id}]");
                        await transaction.CommitAsync();
                    }
                    else
                    {
                        await transaction.RollbackAsync();
                        return Json(new { success = false, errors = "Caixa não encontrado!" });
                    }

                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    return Json(new { success = false, errors = ex.Message });
                }
            }

            return Json(new { success = true, idCaixa = caixa.Id.ToString() });
        }
    }
}

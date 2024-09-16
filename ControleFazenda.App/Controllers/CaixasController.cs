using AutoMapper;
using ControleFazenda.App.ViewModels;
using ControleFazenda.Business.Entidades;
using ControleFazenda.Business.Entidades.Enum;
using ControleFazenda.Business.Interfaces;
using ControleFazenda.Business.Interfaces.Servicos;
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
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ContextoPrincipal _context;
        private readonly ILogAlteracaoServico _logAlteracaoServico;

        public CaixasController(IMapper mapper,
                                  ICaixaServico caixaServico,
                                  IFormaPagamentoServico formaPagamentoServico,
                                  UserManager<IdentityUser> userManager,
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
            CaixaVM caixaVM = new CaixaVM();

            var usuLogado = await _userManager.GetUserAsync(User);

            if (usuLogado != null)
            {
                if(Id != Guid.Empty)
                    caixa = await _caixaServico.ObterPorIdComFluxosDeCaixa(Id);
                else
                    caixa = await _caixaServico.ObterCaixaAberto(usuLogado != null ? usuLogado.Id : string.Empty);

                if (caixa != null)
                {
                    foreach (var item in caixa.FluxosCaixa)
                    {
                        if (item.FormaPagamento == null)
                            item.FormaPagamento = await _formaPagamentoServico.ObterPorId(item.FormaPagamentoId);
                    }
                    caixaVM = _mapper.Map<CaixaVM>(caixa);
                }  
            }
            else
                return NotFound();

            return View(caixaVM);
        }

        [HttpGet]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> PesquisaCaixas()
        {
            try
            {
                var usuLogado = await _userManager.GetUserAsync(User);
                var caixasVM = new List<CaixaVM>();
                if (usuLogado != null)
                    caixasVM = _mapper.Map<List<CaixaVM>>(await _caixaServico.Buscar(x => x.UsuarioCadastroId == Guid.Parse(usuLogado.Id)));
                
                return PartialView("_Pesquisa", caixasVM);
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
            IdentityUser? user = await _userManager.GetUserAsync(User);
            Caixa caixa = new Caixa();
            if (user != null)
            {
                using var transaction = await _context.Database.BeginTransactionAsync();
                try
                {
                    var caixaVM = new CaixaVM();
                    caixa = _mapper.Map<Caixa>(caixaVM);
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

            return Json(new { success = true });
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

            IdentityUser? user = await _userManager.GetUserAsync(User);

            if (user != null)
            {
                using var transaction = await _context.Database.BeginTransactionAsync();
                try
                {
                    var caixa = await _caixaServico.ObterCaixaAberto(user.Id);

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

            return Json(new { success = true });
        }
    }
}

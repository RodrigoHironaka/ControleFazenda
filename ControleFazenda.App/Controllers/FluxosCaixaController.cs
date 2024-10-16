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
    public class FluxosCaixaController : BaseController
    {
        private readonly IFluxoCaixaServico _fluxoCaixaServico;
        private readonly IFormaPagamentoServico _formaPagamentoServico;
        private readonly ICaixaServico _caixaService;
        private readonly IMapper _mapper;
        private readonly UserManager<Usuario> _userManager;
        private readonly ContextoPrincipal _context;
        private readonly ILogAlteracaoServico _logAlteracaoServico;

        public FluxosCaixaController(IMapper mapper,
                                  IFluxoCaixaServico fluxoCaixaServico,
                                  IFormaPagamentoServico formaPagamentoServico,
                                  ICaixaServico caixaService,
                                  UserManager<Usuario> userManager,
                                  ContextoPrincipal context,
                                  ILogAlteracaoServico logAlteracaoServico,
                                  INotificador notificador) : base(notificador)
        {
            _mapper = mapper;
            _fluxoCaixaServico = fluxoCaixaServico;
            _formaPagamentoServico = formaPagamentoServico;
            _caixaService = caixaService;
            _userManager = userManager;
            _context = context;
            _logAlteracaoServico = logAlteracaoServico;
        }

        [Route("lista-de-fluxos")]
        public async Task<IActionResult> Index()
        {
            Usuario? user = await _userManager.GetUserAsync(User);
            if(user != null)
            {
                var caixa = _caixaService.ObterCaixaAberto(user.Id);
                if(caixa != null)
                    return View(_mapper.Map<IEnumerable<FluxoCaixaVM>>(await _fluxoCaixaServico.ObterTodosComEntidades(Guid.Parse(caixa.Id.ToString()))));
                else
                    return View(_mapper.Map<IEnumerable<FluxoCaixaVM>>(await _fluxoCaixaServico.ObterTodosComEntidades(Guid.NewGuid())));
            }
            else
                return Json(new { success = false, errors = "Nenhum usuário encontrado!" });

        }

        [Route("editar-fluxo-caixa/{id}")]
        public async Task<IActionResult> Editar(Guid Id)
        {
            Usuario? user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                var caixa = await _caixaService.ObterCaixaAberto(user.Id);
                if (caixa != null)
                {
                    var fluxoCaixaVM = new FluxoCaixaVM();
                    if (Id != Guid.Empty)
                    {
                        var fluxoCaixa = await _fluxoCaixaServico.ObterPorIdComEntidade(Id, Guid.Parse(caixa.Id.ToString()));
                        if (fluxoCaixa == null) return NotFound();

                        fluxoCaixaVM = _mapper.Map<FluxoCaixaVM>(fluxoCaixa);
                        fluxoCaixaVM.UsuarioCadastro = await _userManager.FindByIdAsync(fluxoCaixaVM.UsuarioCadastroId.ToString());
                        fluxoCaixaVM.UsuarioAlteracao = await _userManager.FindByIdAsync(fluxoCaixaVM.UsuarioAlteracaoId.ToString());
                        if (fluxoCaixaVM.Valor < 0)
                            fluxoCaixaVM.Valor *= -1;
                        fluxoCaixaVM = await PopularFormaspagamento(fluxoCaixaVM);
                    }
                    else
                        fluxoCaixaVM = await PopularFormaspagamento(new FluxoCaixaVM());

                    return View(fluxoCaixaVM);
                }
                else
                    return Json(new { success = false, errors = "O Caixa está fechado!" });
            }
            else
                return Json(new { success = false, errors = "Nenhum usuário encontrado!" });
        }


        [Route("editar-fluxo-caixa/{id:guid}")]
        [HttpPost]
        public async Task<IActionResult> Editar(Guid Id, FluxoCaixaVM fluxoCaixaVM)
        {
            if (Id != fluxoCaixaVM.Id) return NotFound();
            if (!ModelState.IsValid)
            {
                var errors = ModelState.ToDictionary(kvp => kvp.Key, kvp => kvp.Value?.Errors.Select(e => e.ErrorMessage).ToList());
                return Json(new { success = false, errors, isModelState = true });
            }

            Usuario? user = await _userManager.GetUserAsync(User);
            FluxoCaixa fluxoCaixa;

            if (user != null)
            {
                using var transaction = await _context.Database.BeginTransactionAsync();
                try
                {
                    var caixa = await _caixaService.ObterCaixaAberto(user.Id);
                    if (caixa != null)
                    {
                        if (Id != Guid.Empty)
                        {
                            var fluxoCaixaClone = await _fluxoCaixaServico.ObterPorIdComEntidade(Id, Guid.Parse(caixa.Id.ToString()));
                            fluxoCaixaVM.DataAlteracao = DateTime.Now;
                            fluxoCaixa = _mapper.Map<FluxoCaixa>(fluxoCaixaVM);
                            fluxoCaixa.UsuarioAlteracaoId = Guid.Parse(user.Id);
                            fluxoCaixa.FormaPagamento = await _formaPagamentoServico.ObterPorId(fluxoCaixaVM.FormaPagamentoId);
                            if (fluxoCaixa.DebitoCredito == DebitoCredito.Debito)
                                fluxoCaixa.Valor = fluxoCaixa.Valor * -1;
                            await _logAlteracaoServico.CompararAlteracoes(fluxoCaixaClone, fluxoCaixa, Guid.Parse(user.Id), $"FluxoCaixa[{fluxoCaixa.Id}]");
                            await _fluxoCaixaServico.Atualizar(fluxoCaixa);
                        }
                        else
                        {
                            fluxoCaixa = _mapper.Map<FluxoCaixa>(fluxoCaixaVM);
                            fluxoCaixa.UsuarioCadastroId = Guid.Parse(user.Id);
                            fluxoCaixa.CaixaId = caixa.Id;
                            if (fluxoCaixa.DebitoCredito == DebitoCredito.Debito)
                                fluxoCaixa.Valor = fluxoCaixa.Valor * -1;
                            await _fluxoCaixaServico.Adicionar(fluxoCaixa);
                        }
                        if (!OperacaoValida())
                        {
                            await transaction.RollbackAsync();
                            List<string> errors = new List<string>();
                            errors = _notificador.ObterNotificacoes().Select(x => x.Mensagem).ToList();
                            //errors.Add(ObterNotificacoes.ExecutarValidacao(new FluxoCaixaValidation(), fluxoCaixa));
                            return Json(new { success = false, errors });
                        }
                        await transaction.CommitAsync();
                    }
                    else
                        return Json(new { success = false, errors = "O Caixa está fechado!" });

                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    return Json(new { success = false, errors = ex.Message });
                }

                return Json(new { success = true });
            }
            return View(fluxoCaixaVM);
        }

        [HttpPost]
        [Route("excluir-fluxo-caixa/{id:guid}")]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> Deletar(Guid id)
        {
            var fluxoCaixa = await _fluxoCaixaServico.ObterPorId(id);
            if (fluxoCaixa == null) return NotFound();
            Usuario? user = await _userManager.GetUserAsync(User);

            if (user == null) return NotFound();

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                await _logAlteracaoServico.RegistrarLogDiretamente($"Registro: {fluxoCaixa.Descricao} excluído.", Guid.Parse(user.Id), $"FluxoCaixa[{fluxoCaixa.Id}]");
                await _fluxoCaixaServico.Remover(id);
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
                return View(fluxoCaixa);
            }

            return RedirectToAction("Index","Caixas");
        }

        private async Task<FluxoCaixaVM> PopularFormaspagamento(FluxoCaixaVM fluxoCaixa)
        {
            var formasPagamento = await _formaPagamentoServico.Buscar(x => x.Situacao == Situacao.Ativo);
            var formasPagamentoVM = _mapper.Map<IEnumerable<FormaPagamentoVM>>(formasPagamento);
            formasPagamentoVM = formasPagamentoVM.OrderBy(x => x.Nome);
            fluxoCaixa.FormasPagamento = formasPagamentoVM;
            return fluxoCaixa;
        }
    }
}

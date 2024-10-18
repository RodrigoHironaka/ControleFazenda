using AutoMapper;
using ControleFazenda.App.ViewModels;
using ControleFazenda.Business.Entidades;
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
    public class FormasPagamentoController : BaseController
    {
        private readonly IFormaPagamentoServico _formaPagamentoServico;
        private readonly IMapper _mapper;
        private readonly UserManager<Usuario> _userManager;
        private readonly ContextoPrincipal _context;
        private readonly ILogAlteracaoServico _logAlteracaoServico;

        public FormasPagamentoController(IMapper mapper,
                                  IFormaPagamentoServico formaPagamentoServico,
                                  UserManager<Usuario> userManager,
                                  ContextoPrincipal context,
                                  ILogAlteracaoServico logAlteracaoServico,
                                  INotificador notificador) : base(notificador)
        {
            _mapper = mapper;
            _formaPagamentoServico = formaPagamentoServico;
            _userManager = userManager;
            _context = context;
            _logAlteracaoServico = logAlteracaoServico;
        }

        [Route("lista-de-formaspagamento")]
        public async Task<IActionResult> Index()
        {
            Usuario? user = await _userManager.GetUserAsync(User);
            var formas = await _formaPagamentoServico.ObterTodos();
            var formasVM = _mapper.Map<List<FormaPagamentoVM>>(formas);
            var formasFazenda = new List<FormaPagamentoVM>();
            if (user != null && user.AcessoTotal == false)
            {
                foreach (var item in formasVM)
                {
                    Usuario? usuario = await _userManager.FindByIdAsync(item.UsuarioCadastroId.ToString());
                    if (usuario?.Fazenda == user.Fazenda)
                        formasFazenda.Add(item);
                }
                return View(formasFazenda);
            }
            else
            {
                formas = await _formaPagamentoServico.ObterTodos();
                formasVM = _mapper.Map<List<FormaPagamentoVM>>(formas);
                return View(formasVM);
            }
        }

        [Route("editar-formapagamento/{id}")]
        public async Task<IActionResult> Editar(Guid Id)
        {
            var formaPagamentoVM = new FormaPagamentoVM();
            if (Id != Guid.Empty)
            {
                var formaPagamento = await _formaPagamentoServico.ObterPorId(Id);
                if (formaPagamento == null) return NotFound();

                formaPagamentoVM = _mapper.Map<FormaPagamentoVM>(formaPagamento);
                formaPagamentoVM.UsuarioCadastro = await _userManager.FindByIdAsync(formaPagamentoVM.UsuarioCadastroId.ToString());
                formaPagamentoVM.UsuarioAlteracao = await _userManager.FindByIdAsync(formaPagamentoVM.UsuarioAlteracaoId.ToString());
            }

            return View(formaPagamentoVM);
        }

        [Route("editar-formapagamento/{id:guid}")]
        [HttpPost]
        public async Task<IActionResult> Editar(Guid Id, FormaPagamentoVM formaPagamentoVM)
        {
            if (Id != formaPagamentoVM.Id) return NotFound();
            if (!ModelState.IsValid)
            {
                var errors = ModelState.ToDictionary(kvp => kvp.Key, kvp => kvp.Value?.Errors.Select(e => e.ErrorMessage).ToList());
                return Json(new { success = false, errors, isModelState = true });
            }

            Usuario? user = await _userManager.GetUserAsync(User);
            FormaPagamento formaPagamento;

            if (user != null)
            {
                using var transaction = await _context.Database.BeginTransactionAsync();
                try
                {
                    if (Id != Guid.Empty)
                    {
                        var formaPagamentoClone = await _formaPagamentoServico.ObterPorId(formaPagamentoVM.Id);
                        formaPagamentoVM.DataAlteracao = DateTime.Now;
                        formaPagamento = _mapper.Map<FormaPagamento>(formaPagamentoVM);
                        formaPagamento.UsuarioAlteracaoId = Guid.Parse(user.Id);

                        await _logAlteracaoServico.CompararAlteracoes(formaPagamentoClone, formaPagamento, Guid.Parse(user.Id), $"FormaPagamento[{formaPagamento.Id}]");
                        await _formaPagamentoServico.Atualizar(formaPagamento);

                    }
                    else
                    {
                        formaPagamento = _mapper.Map<FormaPagamento>(formaPagamentoVM);
                        formaPagamento.UsuarioCadastroId = Guid.Parse(user.Id);
                        formaPagamento.Fazenda = user.Fazenda;
                        await _formaPagamentoServico.Adicionar(formaPagamento);
                    }

                    if (!OperacaoValida())
                    {
                        await transaction.RollbackAsync();
                        List<string> errors = new List<string>();
                        errors = _notificador.ObterNotificacoes().Select(x => x.Mensagem).ToList();
                        //errors.Add(ObterNotificacoes.ExecutarValidacao(new FormaPagamentoValidation(), formaPagamento));
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
            return View(formaPagamentoVM);
        }

        [HttpPost]
        [Route("excluir-formapagamento/{id:guid}")]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> Deletar(Guid id)
        {
            var formaPagamento = await _formaPagamentoServico.ObterPorId(id);
            IdentityUser? user = await _userManager.GetUserAsync(User);
            if (formaPagamento == null) return NotFound();
            if (user == null) return NotFound();

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                await _logAlteracaoServico.RegistrarLogDiretamente($"Registro: {formaPagamento.Nome} excluído.", Guid.Parse(user.Id), $"FormaPagamento[{formaPagamento.Id}]");
                await _formaPagamentoServico.Remover(id);
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception(ex.Message);
            }

            if (!OperacaoValida()) return View(formaPagamento);

            return RedirectToAction("Index");
        }
    }
}

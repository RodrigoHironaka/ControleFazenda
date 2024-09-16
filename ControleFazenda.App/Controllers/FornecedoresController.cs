using AutoMapper;
using ControleFazenda.App.ViewModels;
using ControleFazenda.Business.Entidades;
using ControleFazenda.Business.Interfaces;
using ControleFazenda.Business.Interfaces.Servicos;
using ControleFazenda.Data.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ControleFazenda.App.Controllers
{
    [Authorize]
    public class FornecedoresController : BaseController
    {
        private readonly IFornecedorServico _fornecedorServico;
        private readonly IMapper _mapper;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ContextoPrincipal _context;
        private readonly ILogAlteracaoServico _logAlteracaoServico;

        public FornecedoresController(IMapper mapper,
                                 IFornecedorServico fornecedorServico,
                                  UserManager<IdentityUser> userManager,
                                  ContextoPrincipal context,
                                  ILogAlteracaoServico logAlteracaoServico,
                                  INotificador notificador) : base(notificador)
        {
            _mapper = mapper;
            _fornecedorServico = fornecedorServico;
            _userManager = userManager;
            _context = context;
            _logAlteracaoServico = logAlteracaoServico;
        }


        [Route("lista-de-fornecedores")]
        public async Task<IActionResult> Index()
        {
            return View(_mapper.Map<IEnumerable<FornecedorVM>>(await _fornecedorServico.ObterTodos()));
        }

        [Route("editar-fornecedor/{id}")]
        public async Task<IActionResult> Editar(Guid Id)
        {
            var fornecedorVM = new FornecedorVM();
            if (Id != Guid.Empty)
            {
                var fornecedor = await _fornecedorServico.ObterPorId(Id);
                if (fornecedor == null) return NotFound();

                fornecedorVM = _mapper.Map<FornecedorVM>(fornecedor);
                fornecedorVM.UsuarioCadastro = await _userManager.FindByIdAsync(fornecedorVM.UsuarioCadastroId.ToString());
                fornecedorVM.UsuarioAlteracao = await _userManager.FindByIdAsync(fornecedorVM.UsuarioAlteracaoId.ToString());
            }

            return View(fornecedorVM);
        }

        [Route("editar-fornecedor/{id:guid}")]
        [HttpPost]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> Editar(Guid Id, FornecedorVM fornecedorVM)
        {
            if (Id != fornecedorVM.Id) return NotFound();
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return Json(new { success = false, errors, isModelState = true });
            }

            IdentityUser? user = await _userManager.GetUserAsync(User);
            Fornecedor fornecedor;

            if (user != null)
            {
                using var transaction = await _context.Database.BeginTransactionAsync();
                try
                {
                    if (Id != Guid.Empty)
                    {
                        var fornecedorClone = await _fornecedorServico.ObterPorId(Id);
                        fornecedorVM.DataAlteracao = DateTime.Now;
                        fornecedor = _mapper.Map<Fornecedor>(fornecedorVM);
                        fornecedor.UsuarioAlteracaoId = Guid.Parse(user.Id);
                        await _logAlteracaoServico.CompararAlteracoes(fornecedorClone, fornecedor, Guid.Parse(user.Id), $"Fornecedor[{fornecedor.Id}]");
                        await _fornecedorServico.Atualizar(fornecedor);
                    }
                    else
                    {
                        fornecedor = _mapper.Map<Fornecedor>(fornecedorVM);
                        fornecedor.UsuarioCadastroId = Guid.Parse(user.Id);
                        await _fornecedorServico.Adicionar(fornecedor);
                    }

                    if (!OperacaoValida())
                    {
                        await transaction.RollbackAsync();
                        List<string> errors = new List<string>();
                        errors = _notificador.ObterNotificacoes().Select(x => x.Mensagem).ToList();
                        //errors.Add(ObterNotificacoes.ExecutarValidacao(new FornecedorValidation(), fornecedor));
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
            return View(fornecedorVM);
        }

        [HttpPost]
        [Route("excluir-fornecedor/{id:guid}")]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> Deletar(Guid id)
        {
            var fornecedor = await _fornecedorServico.ObterPorId(id);
            if (fornecedor == null) return NotFound();
            IdentityUser? user = await _userManager.GetUserAsync(User);

            if (user == null) return NotFound();

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                await _logAlteracaoServico.RegistrarLogDiretamente($"Registro: {fornecedor.RazaoSocial} excluído.", Guid.Parse(user.Id), $"Fornecedor[{fornecedor.Id}]");
                await _fornecedorServico.Remover(id);
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
    }
}


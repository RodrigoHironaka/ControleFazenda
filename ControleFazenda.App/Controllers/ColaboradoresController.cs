using AutoMapper;
using ControleFazenda.App.ViewModels;
using ControleFazenda.Business.Entidades;
using ControleFazenda.Business.Interfaces;
using ControleFazenda.Business.Interfaces.Servicos;
using ControleFazenda.Data.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ControleFazenda.App.Controllers
{
    [Authorize]
    public class ColaboradoresController : BaseController
    {
        private readonly IColaboradorServico _colaboradorServico;
        private readonly IMapper _mapper;
        private readonly UserManager<Usuario> _userManager;
        private readonly ContextoPrincipal _context;
        private readonly ILogAlteracaoServico _logAlteracaoServico;

        public ColaboradoresController(IMapper mapper,
                                  IColaboradorServico colaboradorServico,
                                  UserManager<Usuario> userManager,
                                  ContextoPrincipal context,
                                  ILogAlteracaoServico logAlteracaoServico,
                                  INotificador notificador) : base(notificador)
        {
            _mapper = mapper;
            _colaboradorServico = colaboradorServico;
            _userManager = userManager;
            _context = context;
            _logAlteracaoServico = logAlteracaoServico;
        }


        [Route("lista-de-colaboradores")]
        public async Task<IActionResult> Index()
        {
            Usuario? user = await _userManager.GetUserAsync(User);
            var colaboradores = await _colaboradorServico.ObterTodos();
            var colaboradoresVM = _mapper.Map<List<ColaboradorVM>>(colaboradores);
            var colabsFazenda = new List<ColaboradorVM>();
            if (user != null && user.AcessoTotal == false)
            {
                foreach (var item in colaboradoresVM)
                {
                    Usuario? usuario = await _userManager.FindByIdAsync(item.UsuarioCadastroId.ToString());
                    if (usuario?.Fazenda == user.Fazenda)
                        colabsFazenda.Add(item);
                }
                return View(colabsFazenda);
            }
            else
            {
                colaboradores = await _colaboradorServico.ObterTodos();
                colaboradoresVM = _mapper.Map<List<ColaboradorVM>>(colaboradores);
                return View(colaboradoresVM);
            }
        }

        [Route("editar-colaborador/{id}")]
        public async Task<IActionResult> Editar(Guid Id)
        {
            var colaboradorVM = new ColaboradorVM();
            if (Id != Guid.Empty)
            {
                var colaborador = await _colaboradorServico.ObterPorId(Id);
                if (colaborador == null) return NotFound();

                colaboradorVM = _mapper.Map<ColaboradorVM>(colaborador);
                colaboradorVM.UsuarioCadastro = await _userManager.FindByIdAsync(colaboradorVM.UsuarioCadastroId.ToString());
                colaboradorVM.UsuarioAlteracao = await _userManager.FindByIdAsync(colaboradorVM.UsuarioAlteracaoId.ToString());
            }

            return View(colaboradorVM);
        }

        [Route("editar-colaborador/{id:guid}")]
        [HttpPost]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> Editar(Guid Id, ColaboradorVM colaboradorVM)
        {
            if (Id != colaboradorVM.Id) return NotFound();
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return Json(new { success = false, errors, isModelState = true });
            }

            Usuario? user = await _userManager.GetUserAsync(User);
            Colaborador colaborador;

            if (user != null)
            {
                using var transaction = await _context.Database.BeginTransactionAsync();
                try
                {
                    if (Id != Guid.Empty)
                    {
                        var colaboradorClone = await _colaboradorServico.ObterPorId(Id);
                        colaboradorVM.DataAlteracao = DateTime.Now;
                        colaborador = _mapper.Map<Colaborador>(colaboradorVM);
                        colaborador.UsuarioAlteracaoId = Guid.Parse(user.Id);
                        await _logAlteracaoServico.CompararAlteracoes(colaboradorClone, colaborador, Guid.Parse(user.Id), $"Colaborador[{colaborador.Id}]");
                        await _colaboradorServico.Atualizar(colaborador);
                    }
                    else
                    {
                        colaborador = _mapper.Map<Colaborador>(colaboradorVM);
                        colaborador.UsuarioCadastroId = Guid.Parse(user.Id);
                        await _colaboradorServico.Adicionar(colaborador);
                    }

                    if (!OperacaoValida())
                    {
                        await transaction.RollbackAsync();
                        List<string> errors = new List<string>();
                        errors = _notificador.ObterNotificacoes().Select(x => x.Mensagem).ToList();
                        //errors.Add(ObterNotificacoes.ExecutarValidacao(new ColaboradorValidation(), colaborador));
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
            return View(colaboradorVM);
        }

        [HttpPost]
        [Route("excluir-colaborador/{id:guid}")]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> Deletar(Guid id)
        {
            var colaborador = await _colaboradorServico.ObterPorId(id);
            if (colaborador == null) return NotFound();
            Usuario? user = await _userManager.GetUserAsync(User);

            if (user == null) return NotFound();

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                await _logAlteracaoServico.RegistrarLogDiretamente($"Registro: {colaborador.RazaoSocial} excluído.", Guid.Parse(user.Id), $"Colaborador[{colaborador.Id}]");
                await _colaboradorServico.Remover(id);
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


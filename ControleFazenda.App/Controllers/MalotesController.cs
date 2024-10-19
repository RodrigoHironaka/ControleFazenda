using AutoMapper;
using ControleFazenda.App.ViewModels;
using ControleFazenda.Business.Entidades;
using ControleFazenda.Business.Interfaces;
using ControleFazenda.Business.Interfaces.Servicos;
using ControleFazenda.Data.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ControleFazenda.App.Controllers
{
    public class MalotesController : BaseController
    {
        private readonly IMaloteServico _maloteServico;
        private readonly IMapper _mapper;
        private readonly UserManager<Usuario> _userManager;
        private readonly ContextoPrincipal _context;
        private readonly ILogAlteracaoServico _logAlteracaoServico;

        public MalotesController(IMapper mapper,
                                  IMaloteServico maloteServico,
                                  UserManager<Usuario> userManager,
                                  ContextoPrincipal context,
                                  ILogAlteracaoServico logAlteracaoServico,
                                  INotificador notificador) : base(notificador)
        {
            _mapper = mapper;
            _maloteServico = maloteServico;
            _userManager = userManager;
            _context = context;
            _logAlteracaoServico = logAlteracaoServico;
        }

        [Route("lista-de-malotes")]
        public async Task<IActionResult> Index()
        {
            Usuario? user = await _userManager.GetUserAsync(User);
            var malotes = await _maloteServico.ObterTodos();
            var malotesVM = _mapper.Map<List<MaloteVM>>(malotes);
            var malotesFazenda = new List<MaloteVM>();
            if (user != null && user.AcessoTotal == false)
            {
                foreach (var item in malotesVM)
                {
                    Usuario? usuario = await _userManager.FindByIdAsync(item.UsuarioCadastroId.ToString());
                    if (usuario?.Fazenda == user.Fazenda)
                        malotesFazenda.Add(item);
                }
                return View(malotesFazenda);
            }
            else
            {
                malotes = await _maloteServico.ObterTodos();
                malotesVM = _mapper.Map<List<MaloteVM>>(malotes);
                return View(malotesVM);
            }
        }

        [Route("editar-malote/{id}")]
        public async Task<IActionResult> Editar(Guid Id)
        {
            var maloteVM = new MaloteVM();
            if (Id != Guid.Empty)
            {
                var malote = await _maloteServico.ObterPorId(Id);
                if (malote == null) return NotFound();

                maloteVM = _mapper.Map<MaloteVM>(malote);
                maloteVM.UsuarioCadastro = await _userManager.FindByIdAsync(maloteVM.UsuarioCadastroId.ToString());
                maloteVM.UsuarioAlteracao = await _userManager.FindByIdAsync(maloteVM.UsuarioAlteracaoId.ToString());
            }

            return View(maloteVM);
        }

        [Route("editar-malote/{id:guid}")]
        [HttpPost]
        public async Task<IActionResult> Editar(Guid Id, MaloteVM maloteVM)
        {
            if (Id != maloteVM.Id) return NotFound();
            if (!ModelState.IsValid)
            {
                var errors = ModelState.ToDictionary(kvp => kvp.Key, kvp => kvp.Value?.Errors.Select(e => e.ErrorMessage).ToList());
                return Json(new { success = false, errors, isModelState = true });
            }

            Usuario? user = await _userManager.GetUserAsync(User);
            Malote malote;

            if (user != null)
            {
                using var transaction = await _context.Database.BeginTransactionAsync();
                try
                {
                    if (Id != Guid.Empty)
                    {
                        var maloteClone = await _maloteServico.ObterPorId(maloteVM.Id);
                        maloteVM.DataAlteracao = DateTime.Now;
                        malote = _mapper.Map<Malote>(maloteVM);
                        malote.UsuarioAlteracaoId = Guid.Parse(user.Id);

                        await _logAlteracaoServico.CompararAlteracoes(maloteClone, malote, Guid.Parse(user.Id), $"Malote[{malote.Id}]");
                        await _maloteServico.Atualizar(malote);

                    }
                    else
                    {
                        malote = _mapper.Map<Malote>(maloteVM);
                        malote.UsuarioCadastroId = Guid.Parse(user.Id);
                        malote.Fazenda = user.Fazenda;
                        await _maloteServico.Adicionar(malote);
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
            return View(maloteVM);
        }

        [HttpPost]
        [Route("excluir-malote/{id:guid}")]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> Deletar(Guid id)
        {
            var malote = await _maloteServico.ObterPorId(id);
            IdentityUser? user = await _userManager.GetUserAsync(User);
            if (malote == null) return NotFound();
            if (user == null) return NotFound();

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                await _logAlteracaoServico.RegistrarLogDiretamente($"Registro: {malote.Numero} excluído.", Guid.Parse(user.Id), $"Malote[{malote.Id}]");
                await _maloteServico.Remover(id);
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception(ex.Message);
            }

            if (!OperacaoValida()) return View(malote);

            return RedirectToAction("Index");
        }
    }
}


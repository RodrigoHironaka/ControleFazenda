using AutoMapper;
using ControleFazenda.Business.Interfaces.Servicos;
using ControleFazenda.Business.Interfaces;
using ControleFazenda.Data.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ControleFazenda.App.ViewModels;
using ControleFazenda.Business.Servicos;
using ControleFazenda.Business.Entidades;
using ControleFazenda.Business.Entidades.Enum;
using System.Diagnostics;

namespace ControleFazenda.App.Controllers
{
    public class NFeController : BaseController
    {
        private readonly INFeServico _nfeServico;
        private readonly IFornecedorServico _fornecedorServico;
        private readonly IMapper _mapper;
        private readonly UserManager<Usuario> _userManager;
        private readonly ContextoPrincipal _context;
        private readonly ILogAlteracaoServico _logAlteracaoServico;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public NFeController(IMapper mapper,
                                  INFeServico nfeServico,
                                  IFornecedorServico fornecedorServico,
                                  UserManager<Usuario> userManager,
                                  ContextoPrincipal context,
                                  ILogAlteracaoServico logAlteracaoServico,
                                  IWebHostEnvironment webHostEnvironment,
                                  INotificador notificador) : base(notificador)
        {
            _mapper = mapper;
            _nfeServico = nfeServico;
            _fornecedorServico = fornecedorServico;
            _userManager = userManager;
            _context = context;
            _logAlteracaoServico = logAlteracaoServico;
            _webHostEnvironment = webHostEnvironment;
        }

        [Route("lista-de-nfes")]
        public async Task<IActionResult> Index()
        {
            Usuario? user = await _userManager.GetUserAsync(User);
            var nfes = await _nfeServico.ObterNFeComFornecedor();
            var nfesVM = _mapper.Map<List<NFeVM>>(nfes);
            var nfesFazenda = new List<NFeVM>();
            if (user != null)
            {
                foreach (var item in nfesVM)
                {
                    Usuario? usuario = await _userManager.FindByIdAsync(item.UsuarioCadastroId.ToString());
                    if (usuario?.Fazenda == user.Fazenda)
                        nfesFazenda.Add(item);
                }
                return View(nfesFazenda);
            }
            else
                return View(new List<NFeVM>());
        }

        [Route("editar-nfe/{id}")]
        public async Task<IActionResult> Editar(Guid Id)
        {
            var NFeVM = new NFeVM();
            if (Id != Guid.Empty)
            {
                var NFe = await _nfeServico.ObterPorIdComFornecedor(Id);
                if (NFe == null) return NotFound();

                NFeVM = _mapper.Map<NFeVM>(NFe);
                NFeVM.UsuarioCadastro = await _userManager.FindByIdAsync(NFeVM.UsuarioCadastroId.ToString());
                NFeVM.UsuarioAlteracao = await _userManager.FindByIdAsync(NFeVM.UsuarioAlteracaoId.ToString());
                NFeVM = await PopularFornecedores(NFeVM);
            }
            else
                NFeVM = await PopularFornecedores(NFeVM);

            return View(NFeVM);
        }

        [Route("editar-nfe/{id:guid}")]
        [HttpPost]
        public async Task<IActionResult> Editar(Guid Id, NFeVM nfeVM)
        {
            if (Id != nfeVM.Id) return NotFound();
            if (!ModelState.IsValid)
            {
                var errors = ModelState.ToDictionary(kvp => kvp.Key, kvp => kvp.Value?.Errors.Select(e => e.ErrorMessage).ToList());
                return Json(new { success = false, errors, isModelState = true });
            }

            Usuario? user = await _userManager.GetUserAsync(User);
            NFe nfe;

            if (user != null)
            {
                using var transaction = await _context.Database.BeginTransactionAsync();
                try
                {
                    if (Id != Guid.Empty)
                    {
                        var nfeClone = await _nfeServico.ObterPorIdComFornecedor(nfeVM.Id);
                        nfeVM.DataAlteracao = DateTime.Now;
                        var prefixo = nfeVM.Id + "_";
                        if (nfeVM.Arquivo != null)
                        {
                            nfeVM.CaminhoArquivo = prefixo + nfeVM.Arquivo.FileName;
                            string pastaNfes = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "arquivos", "nfes");

                            if (Directory.Exists(pastaNfes))
                            {
                                foreach (var arquivo in Directory.GetFiles(pastaNfes))
                                {
                                    if (arquivo.Contains(prefixo.ToString()))
                                    {
                                        System.IO.File.Delete(arquivo);
                                        break;
                                    }
                                }
                            }
                            await UploadArquivo(nfeVM.Arquivo, prefixo);
                        }
                        nfe = _mapper.Map<NFe>(nfeVM);
                        nfe.UsuarioAlteracaoId = Guid.Parse(user.Id);

                        await _logAlteracaoServico.CompararAlteracoes(nfeClone, nfe, Guid.Parse(user.Id), $"NFe[{nfe.Numero}]");
                        await _nfeServico.Atualizar(nfe);

                    }
                    else
                    {
                        nfe = _mapper.Map<NFe>(nfeVM);
                        nfe.UsuarioCadastroId = Guid.Parse(user.Id);
                        await _nfeServico.Adicionar(nfe);
                        var prefixo = nfe.Id + "_";
                        await UploadArquivo(nfeVM.Arquivo, prefixo);
                        if(nfeVM.Arquivo != null)
                            nfe.CaminhoArquivo = prefixo + nfeVM.Arquivo.FileName;
                        await _nfeServico.Atualizar(nfe);
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
            return View(nfeVM);
        }

        [HttpPost]
        [Route("excluir-nfe/{id:guid}")]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> Deletar(Guid id)
        {
            var nfe = await _nfeServico.ObterPorIdComFornecedor(id);
            IdentityUser? user = await _userManager.GetUserAsync(User);
            if (nfe == null) return NotFound();
            if (user == null) return NotFound();

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                await _logAlteracaoServico.RegistrarLogDiretamente($"Registro: {nfe.Fornecedor?.RazaoSocial} excluído.", Guid.Parse(user.Id), $"NFe[{nfe.Numero}]");
                await _nfeServico.Remover(id);
                await transaction.CommitAsync();

                string pastaNfes = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "arquivos", "nfes");

                if (Directory.Exists(pastaNfes))
                {
                    foreach (var arquivo in Directory.GetFiles(pastaNfes))
                    {
                        if (arquivo.Contains(id.ToString()))
                        {
                            System.IO.File.Delete(arquivo);
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception(ex.Message);
            }

            if (!OperacaoValida()) return View(nfe);

            return RedirectToAction("Index");
        }

        private async Task<NFeVM> PopularFornecedores(NFeVM nfe)
        {
            var fornecedores = await _fornecedorServico.Buscar(x => x.Situacao == Situacao.Ativo);
            var fornecedoresVM = _mapper.Map<IEnumerable<FornecedorVM>>(fornecedores);
            fornecedoresVM = fornecedoresVM.OrderBy(x => x.RazaoSocial);
            nfe.Fornecedores = fornecedoresVM;

            return nfe;
        }

        private async Task<bool> UploadArquivo(IFormFile? arquivo, string prefixo)
        {
            if (arquivo == null || arquivo.Length <= 0) return false;

            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/arquivos/nfes", prefixo + arquivo.FileName);

            if (System.IO.File.Exists(path))
            {
                ModelState.AddModelError(string.Empty, "Já existe um arquivo com esse nome!");
                return false;
            }

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await arquivo.CopyToAsync(stream);
            }
            return true;
        }

        [Route("abrir-arquivo/{id}")]
        [IgnoreAntiforgeryToken]
        public void AbrirArquivo(string id)
        {
            string pastaNfes = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "arquivos", "nfes");

            if (Directory.Exists(pastaNfes))
            {
                foreach (var arquivo in Directory.GetFiles(pastaNfes))
                {
                    if (arquivo.Contains(id))
                    {
                        Process.Start(new ProcessStartInfo
                        {
                            FileName = arquivo,
                            UseShellExecute = true // Necessário para abrir com o aplicativo padrão
                        });
                        break;
                    }
                }
            }
            else
            {
                throw new ArgumentException("Pasta não encontrada.");
            }
        }
    }
}

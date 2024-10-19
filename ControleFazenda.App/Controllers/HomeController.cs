using ControleFazenda.App.Models;
using ControleFazenda.App.ViewModels;
using ControleFazenda.Business.Entidades;
using ControleFazenda.Business.Interfaces.Repositorios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ControleFazenda.App.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ICaixaRepositorio _caixaRepositorio;
        private readonly UserManager<Usuario> _userManager;

        public HomeController(ICaixaRepositorio caixaRepositorio, UserManager<Usuario> userManager)
        {
            _caixaRepositorio = caixaRepositorio;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            Usuario? user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                var caixa = await _caixaRepositorio.ObterCaixaAberto(user.Id);
                if (caixa == null)
                    ViewData["Mensagem"] = "Nenhum caixa aberto!";

                var usuLogado = await _userManager.GetUserAsync(User);
                ViewBag.AcessoTotal = usuLogado?.AcessoTotal;
            }
            return View();
        }

        [Route("erro/{id:length(3,3)}")]
        public IActionResult Errors(int id)
        {
            var modelErro = new ErrorVM();

            if (id == 500)
            {
                modelErro.Mensagem = "Ocorreu um erro! Tente novamente mais tarde ou contate nosso suporte.";
                modelErro.Titulo = "Ocorreu um erro!";
                modelErro.ErroCode = id;
            }
            else if (id == 404)
            {
                modelErro.Mensagem = "A p�gina que est� procurando n�o existe! <br />Em caso de d�vidas entre em contato com nosso suporte";
                modelErro.Titulo = "Ops! P�gina n�o encontrada.";
                modelErro.ErroCode = id;
            }
            else if (id == 403)
            {
                modelErro.Mensagem = "Voc� n�o tem permiss�o para fazer isto.";
                modelErro.Titulo = "Acesso Negado";
                modelErro.ErroCode = id;
            }
            else
            {
                return StatusCode(500);
            }

            return View("Error", modelErro);
        }
    }
}

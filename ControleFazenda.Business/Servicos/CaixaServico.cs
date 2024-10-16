using ControleFazenda.Business.Entidades;
using ControleFazenda.Business.Entidades.Validacoes;
using ControleFazenda.Business.Interfaces;
using ControleFazenda.Business.Interfaces.Repositorios;
using ControleFazenda.Business.Interfaces.Servicos;
using System.Linq.Expressions;

namespace ControleFazenda.Business.Servicos
{
    public class CaixaServico : BaseServico, ICaixaServico
    {
        private readonly ICaixaRepositorio _caixaRepositorio;

        public CaixaServico(ICaixaRepositorio caixaRepositorio, INotificador notificador) : base(notificador)
        {
            _caixaRepositorio = caixaRepositorio;
        }

        public async Task<Caixa> ObterPorId(Guid id)
        {
            return await _caixaRepositorio.ObterPorId(id);
        }

        public async Task<List<Caixa>> ObterTodos()
        {
            return await _caixaRepositorio.ObterTodos();
        }

        public async Task Adicionar(Caixa entity)
        {
            if (!ExecutarValidacao(new CaixaValidacao(), entity)) return;
            await _caixaRepositorio.Adicionar(entity);
        }

        public async Task Atualizar(Caixa entity)
        {
            if (!ExecutarValidacao(new CaixaValidacao(), entity)) return;
            await _caixaRepositorio.Atualizar(entity);
        }

        public async Task Remover(Guid id)
        {
            await _caixaRepositorio.Remover(id);
        }

        public async Task<IEnumerable<Caixa>> Buscar(Expression<Func<Caixa, bool>> predicate)
        {
            return await _caixaRepositorio.Buscar(predicate);
        }

        public void Dispose()
        {
            _caixaRepositorio?.Dispose();
        }

        public async Task<Caixa> ObterCaixaAberto(string idUsuario)
        {
            return await _caixaRepositorio.ObterCaixaAberto(idUsuario);
        }

        public async Task<long> ObteNumeroUltimoCaixa(string idUsuario)
        {
            return await _caixaRepositorio.ObterNumeroUltimoCaixa(idUsuario);
        }

        public async Task<Caixa> ObterPorIdComFluxosDeCaixa(Guid Id)
        {
            return await _caixaRepositorio.ObterPorIdComFluxosDeCaixa(Id);
        }

        public async Task<List<Caixa>> ObterCaixasComFluxosDeCaixa(Expression<Func<Caixa, bool>>? predicate = null)
        {
            return await _caixaRepositorio.ObterCaixasComFluxosDeCaixa(predicate);
        }

        public async Task<List<Caixa>> ObterCaixasAberto()
        {
            return await _caixaRepositorio.ObterCaixasAberto();
        }

        public async Task<List<Caixa>> ObterTodosComFluxosDeCaixa()
        {
            return await _caixaRepositorio.ObterTodosComFluxosDeCaixa();
        }
    }
}

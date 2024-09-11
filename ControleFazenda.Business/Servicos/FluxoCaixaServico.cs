using ControleFazenda.Business.Entidades;
using ControleFazenda.Business.Entidades.Validacoes;
using ControleFazenda.Business.Interfaces;
using ControleFazenda.Business.Interfaces.Repositorios;
using ControleFazenda.Business.Interfaces.Servicos;
using System.Linq.Expressions;

namespace ControleFazenda.Business.Servicos
{
    public class FluxoCaixaServico : BaseServico, IFluxoCaixaServico
    {
        private readonly IFluxoCaixaRepositorio _fluxoCaixaRepositorio;

        public FluxoCaixaServico(IFluxoCaixaRepositorio fluxoCaixaRepositorio, INotificador notificador) : base(notificador)
        {
            _fluxoCaixaRepositorio = fluxoCaixaRepositorio;
        }

        public async Task<FluxoCaixa> ObterPorId(Guid id)
        {
            return await _fluxoCaixaRepositorio.ObterPorId(id);
        }

        public async Task<List<FluxoCaixa>> ObterTodos()
        {
            return await _fluxoCaixaRepositorio.ObterTodos();
        }

        public async Task Adicionar(FluxoCaixa entity)
        {
            if (!ExecutarValidacao(new FluxoCaixaValidacao(), entity)) return;
            await _fluxoCaixaRepositorio.Adicionar(entity);
        }

        public async Task Atualizar(FluxoCaixa entity)
        {
            if (!ExecutarValidacao(new FluxoCaixaValidacao(), entity)) return;
            await _fluxoCaixaRepositorio.Atualizar(entity);
        }

        public async Task Remover(Guid id)
        {
            await _fluxoCaixaRepositorio.Remover(id);
        }

        public async Task<IEnumerable<FluxoCaixa>> Buscar(Expression<Func<FluxoCaixa, bool>> predicate)
        {
            return await _fluxoCaixaRepositorio.Buscar(predicate);
        }

        public void Dispose()
        {
            _fluxoCaixaRepositorio?.Dispose();
        }

        public async Task<FluxoCaixa> ObterPorIdComEntidade(Guid id, Guid idCaixa)
        {
            return await _fluxoCaixaRepositorio.ObterPorIdComEntidade(id, idCaixa);
        }

        public async Task<IEnumerable<FluxoCaixa>> ObterTodosComEntidades(Guid idCaixa)
        {
            return await _fluxoCaixaRepositorio.ObterTodosComEntidades(idCaixa);
        }
    }
}

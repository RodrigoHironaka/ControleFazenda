using ControleFazenda.Business.Entidades;

namespace ControleFazenda.Business.Interfaces.Repositorios
{
    public interface IFluxoCaixaRepositorio : IRepositorio<FluxoCaixa>
    {
        Task<FluxoCaixa> ObterPorIdComEntidade(Guid id, Guid idCaixa);
        Task<IEnumerable<FluxoCaixa>> ObterTodosComEntidades(Guid idCaixa);

    }
}

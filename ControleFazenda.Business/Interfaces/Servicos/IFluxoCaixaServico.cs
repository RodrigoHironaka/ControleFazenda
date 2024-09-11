using ControleFazenda.Business.Entidades;

namespace ControleFazenda.Business.Interfaces.Servicos
{
    public interface IFluxoCaixaServico : IServico<FluxoCaixa>
    {
        Task<FluxoCaixa> ObterPorIdComEntidade(Guid id, Guid idCaixa);
        Task<IEnumerable<FluxoCaixa>> ObterTodosComEntidades(Guid idCaixa);
    }
}

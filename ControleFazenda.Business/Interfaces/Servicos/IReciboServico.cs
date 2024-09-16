using ControleFazenda.Business.Entidades;

namespace ControleFazenda.Business.Interfaces.Servicos
{
    public interface IReciboServico : IServico<Recibo>
    {
        Task<Int64> ObterNumeroUltimoRecibo();
        Task<Recibo> ObterPorIdComColaborador(Guid Id);
        Task<IEnumerable<Recibo>> ObterTodosComColaborador();
    }
}

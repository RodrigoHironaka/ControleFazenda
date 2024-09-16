using ControleFazenda.Business.Entidades;

namespace ControleFazenda.Business.Interfaces.Repositorios
{
    public interface IReciboRepositorio : IRepositorio<Recibo>
    {
        Task<Int64> ObterNumeroUltimoRecibo();
        Task<Recibo> ObterPorIdComColaborador(Guid Id);
        Task<IEnumerable<Recibo>> ObterTodosComColaborador();
    }
}

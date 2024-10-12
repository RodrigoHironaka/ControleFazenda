using ControleFazenda.Business.Entidades;

namespace ControleFazenda.Business.Interfaces.Repositorios
{
    public interface IValeRepositorio : IRepositorio<Vale>
    {
        Task<Int64> ObterNumeroUltimoVale();
        Task<Vale> ObterPorIdComColaborador(Guid Id);
        Task<IEnumerable<Vale>> ObterTodosComColaborador();
    }
}

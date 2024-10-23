using ControleFazenda.Business.Entidades;

namespace ControleFazenda.Business.Interfaces.Repositorios
{
    public interface IDiariaRepositorio : IRepositorio<Diaria>
    {
        Task<Diaria> ObterPorIdComColaborador(Guid Id);
        Task<IEnumerable<Diaria>> ObterTodosComColaborador();
    }
}

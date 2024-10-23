using ControleFazenda.Business.Entidades;

namespace ControleFazenda.Business.Interfaces.Servicos
{
    public interface IDiariaServico : IServico<Diaria>
    {
        Task<Diaria> ObterPorIdComColaborador(Guid Id);
        Task<IEnumerable<Diaria>> ObterTodosComColaborador();
    }
}

using ControleFazenda.Business.Entidades;

namespace ControleFazenda.Business.Interfaces.Repositorios
{
    public interface ICaixaRepositorio : IRepositorio<Caixa>
    {
        Task<Caixa> ObterCaixaAberto(string idUsuario);
        Task<Int64> ObterNumeroUltimoCaixa(string idUsuario);
        Task<Caixa> ObterPorIdComFluxosDeCaixa(Guid Id);
    }
}

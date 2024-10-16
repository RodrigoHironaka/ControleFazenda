using ControleFazenda.Business.Entidades;
using System.Linq.Expressions;

namespace ControleFazenda.Business.Interfaces.Repositorios
{
    public interface ICaixaRepositorio : IRepositorio<Caixa>
    {
        Task<Caixa> ObterCaixaAberto(string idUsuario);
        Task<List<Caixa>> ObterCaixasAberto();
        Task<List<Caixa>> ObterTodosComFluxosDeCaixa();
        Task<Int64> ObterNumeroUltimoCaixa(string idUsuario);
        Task<Caixa> ObterPorIdComFluxosDeCaixa(Guid Id);
        Task<List<Caixa>> ObterCaixasComFluxosDeCaixa(Expression<Func<Caixa, bool>>? predicate = null);
        
    }
}

using System.Linq.Expressions;

namespace ControleFazenda.Business.Interfaces.Servicos
{
    public interface IServico<T> : IDisposable where T : class
    {
        Task Adicionar(T entity);
        Task<T> ObterPorId(Guid id);
        Task<List<T>> ObterTodos();
        Task Atualizar(T entity);
        Task Remover(Guid id);
        Task<IEnumerable<T>> Buscar(Expression<Func<T, bool>> predicate);
    }
}

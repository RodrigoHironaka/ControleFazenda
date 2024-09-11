using ControleFazenda.Business.Entidades.Componentes;
using System.Linq.Expressions;

namespace ControleFazenda.Business.Interfaces.Repositorios
{
    public interface IRepositorio<T> : IDisposable where T : Entidade
    {
        Task Adicionar(T entity);
        Task<T> ObterPorId(Guid id);
        Task<List<T>> ObterTodos();
        Task Atualizar(T entity);
        Task Remover(Guid id);
        Task<IEnumerable<T>> Buscar(Expression<Func<T, bool>> predicate);
        Task<int> SaveChanges();
    }
}

using ControleFazenda.Business.Entidades.Componentes;
using ControleFazenda.Business.Interfaces.Repositorios;
using ControleFazenda.Data.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ControleFazenda.Data.Repository
{
    public class Repositorio<T> : IRepositorio<T> where T : Entidade, new()
    {
        protected readonly ContextoPrincipal Db;
        protected readonly DbSet<T> DbSet;

        public Repositorio(ContextoPrincipal db)
        {
            Db = db;
            DbSet = db.Set<T>();
        }

        public async Task Adicionar(T entity)
        {
            DbSet.Add(entity);
            await SaveChanges();
        }

        public async Task Atualizar(T entity)
        {
            DbSet.Update(entity);
            await SaveChanges();
        }

        public async Task<IEnumerable<T>> Buscar(Expression<Func<T, bool>> predicate)
        {
            return await DbSet.AsNoTracking().Where(predicate).ToListAsync();
        }

        public void Dispose()
        {
            Db?.Dispose();
        }

        public async Task<T> ObterPorId(Guid id)
        {
            return await DbSet.AsNoTracking().Where(x => x.Id == id).FirstAsync();
        }

        public async Task<List<T>> ObterTodos()
        {
            return await DbSet.AsNoTracking().ToListAsync();
        }

        public async Task Remover(Guid id)
        {
            DbSet.Remove(new T { Id = id });
            await SaveChanges();
        }

        public async Task<int> SaveChanges()
        {
            return await Db.SaveChangesAsync();
        }
    }
}

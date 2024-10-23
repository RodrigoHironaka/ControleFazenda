using ControleFazenda.Business.Entidades;
using ControleFazenda.Business.Interfaces.Repositorios;
using ControleFazenda.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace ControleFazenda.Data.Repository
{
    public class DiariaRepositorio : Repositorio<Diaria>, IDiariaRepositorio
    {
        public DiariaRepositorio(ContextoPrincipal db) : base(db) { }

        public async Task<Diaria> ObterPorIdComColaborador(Guid Id)
        {
            var diarias = await Db.Diarias.AsNoTracking().Include(f => f.Colaborador)
                .FirstOrDefaultAsync(p => p.Id == Id);

            return diarias;
        }

        public async Task<IEnumerable<Diaria>> ObterTodosComColaborador()
        {
            return await Db.Diarias.AsNoTracking().Include(f => f.Colaborador)
                .OrderBy(p => p.Colaborador.RazaoSocial).ToListAsync();
        }
    }
}

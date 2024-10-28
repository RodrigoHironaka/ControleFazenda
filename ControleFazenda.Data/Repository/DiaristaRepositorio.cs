using ControleFazenda.Business.Entidades;
using ControleFazenda.Business.Interfaces.Repositorios;
using ControleFazenda.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControleFazenda.Data.Repository
{
    public class DiaristaRepositorio : Repositorio<Diarista>, IDiaristaRepositorio
    {
        public DiaristaRepositorio(ContextoPrincipal db) : base(db) { }

        public async Task<Diarista> ObterPorIdComColaborador(Guid Id)
        {
            var diarista = await Db.Diaristas.AsNoTracking().Include(f => f.Diarias).Include(f => f.Colaborador)
                .FirstOrDefaultAsync(p => p.Id == Id);

            return diarista;
        }

        public async Task<IEnumerable<Diarista>> ObterTodosComColaborador()
        {
            return await Db.Diaristas.AsNoTracking().Include(f => f.Diarias).Include(c => c.Colaborador)
                .OrderBy(p => p.Colaborador.RazaoSocial).ToListAsync();
        }
    }
}
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
    public class ValeRepositorio : Repositorio<Vale>, IValeRepositorio
    {
        public ValeRepositorio(ContextoPrincipal db) : base(db) { }

        public async Task<IEnumerable<Vale>> ObterTodosComColaborador()
        {
            return await Db.Vales.AsNoTracking().Include(f => f.Colaborador)
                .OrderBy(p => p.Colaborador.RazaoSocial).ToListAsync();
        }

        public async Task<Int64> ObterNumeroUltimoVale()
        {
            var vales = await ObterTodos();
            var ultimoVale = vales.OrderBy(x => x.Numero).LastOrDefault();
            if (ultimoVale != null)
                return ultimoVale.Numero;

            return 0;
        }

        public async Task<Vale> ObterPorIdComColaborador(Guid Id)
        {
            var vale = await Db.Vales.AsNoTracking().Include(f => f.Colaborador)
                .FirstOrDefaultAsync(p => p.Id == Id);

            return vale;
        }
    }
}

using ControleFazenda.Business.Entidades.Enum;
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
    public class ReciboRepositorio : Repositorio<Recibo>, IReciboRepositorio
    {
        public ReciboRepositorio(ContextoPrincipal db) : base(db) { }

        public async Task<IEnumerable<Recibo>> ObterTodosComColaborador()
        {
            return await Db.Recibos.AsNoTracking().Include(f => f.Colaborador)
                .OrderBy(p => p.Colaborador.RazaoSocial).ToListAsync();
        }

        public async Task<Int64> ObterNumeroUltimoRecibo()
        {
            var recibos = await ObterTodos();
            var ultimoRecibo = recibos.OrderBy(x => x.Numero).LastOrDefault();
            if (ultimoRecibo != null)
                return ultimoRecibo.Numero;

            return 0;
        }

        public async Task<Recibo> ObterPorIdComColaborador(Guid Id)
        {
            var recibo = await Db.Recibos.AsNoTracking().Include(f => f.Colaborador)
                .FirstOrDefaultAsync(p => p.Id == Id);

            return recibo;
        }
    }
}

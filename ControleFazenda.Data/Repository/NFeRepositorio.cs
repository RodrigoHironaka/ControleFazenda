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
    public class NFeRepositorio : Repositorio<NFe>, INFeRepositorio
    {
        public NFeRepositorio(ContextoPrincipal db) : base(db) { }

        public async Task<NFe> ObterPorIdComFornecedor(Guid Id)
        {
            var nfe = await Db.NFes.AsNoTracking().Include(f => f.Fornecedor)
                .FirstOrDefaultAsync(p => p.Id == Id);

            return nfe;
        }

        public async Task<IEnumerable<NFe>> ObterTodosComFornecedor()
        {
            return await Db.NFes.AsNoTracking().Include(f => f.Fornecedor)
                 .OrderBy(p => p.Fornecedor.RazaoSocial).ToListAsync();
        }
    }
}

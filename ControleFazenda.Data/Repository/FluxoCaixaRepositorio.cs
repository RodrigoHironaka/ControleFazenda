using ControleFazenda.Business.Entidades;
using ControleFazenda.Business.Interfaces.Repositorios;
using ControleFazenda.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace ControleFazenda.Data.Repository
{
    public class FluxoCaixaRepositorio : Repositorio<FluxoCaixa>, IFluxoCaixaRepositorio
    {
        public FluxoCaixaRepositorio(ContextoPrincipal db) : base(db) { }

        public async Task<IEnumerable<FluxoCaixa>> ObterTodosComEntidades(Guid IdCaixa)
        {
            return await Db.FluxosCaixa.Where(x => x.CaixaId == IdCaixa).AsNoTracking().Include(f => f.FormaPagamento).Include(f => f.Caixa)
                .OrderBy(p => p.CaixaId).ToListAsync();
        }

        public async Task<FluxoCaixa> ObterPorIdComEntidade(Guid id, Guid IdCaixa)
        {
            var FluxoCaixa = await Db.FluxosCaixa.Where(x => x.CaixaId == IdCaixa).AsNoTracking().Include(f => f.FormaPagamento).Include(f => f.Caixa)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (FluxoCaixa == null)
                return new FluxoCaixa();
            return FluxoCaixa;
        }
    }
}

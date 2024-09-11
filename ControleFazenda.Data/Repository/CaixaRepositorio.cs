using ControleFazenda.Business.Entidades;
using ControleFazenda.Business.Entidades.Enum;
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
    public class CaixaRepositorio : Repositorio<Caixa>, ICaixaRepositorio
    {
        public CaixaRepositorio(ContextoPrincipal db) : base(db) { }

        public async Task<Int64> ObterNumeroUltimoCaixa(string idUsuario)
        {
            var caixas = await Buscar(x => x.UsuarioCadastroId == Guid.Parse(idUsuario));
            var ultimoCaixa = caixas.OrderBy(x => x.Numero).LastOrDefault();
            if (ultimoCaixa != null)
                return ultimoCaixa.Numero;

            return 0;
        }

        public async Task<Caixa> ObterCaixaAberto(string idUsuario)
        {
            var caixa = await Db.Caixas.AsNoTracking().Include(x => x.FluxosCaixa).Where(x => x.Situacao == SituacaoCaixa.Aberto && x.UsuarioCadastroId == Guid.Parse(idUsuario)).FirstOrDefaultAsync();
            return caixa;
        }

        public async Task<Caixa> ObterPorIdComFluxosDeCaixa(Guid Id)
        {
            var caixa = await Db.Caixas.AsNoTracking().Include(f => f.FluxosCaixa).ThenInclude(fc => fc.FormaPagamento)
                .FirstOrDefaultAsync(p => p.Id == Id);

            return caixa;
        }
    }
}

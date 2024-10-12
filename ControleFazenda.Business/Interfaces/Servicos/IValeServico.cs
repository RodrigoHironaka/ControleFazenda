using ControleFazenda.Business.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControleFazenda.Business.Interfaces.Servicos
{
    public interface IValeServico : IServico<Vale>
    {
        Task<Int64> ObterNumeroUltimoVale();
        Task<Vale> ObterPorIdComColaborador(Guid Id);
        Task<IEnumerable<Vale>> ObterTodosComColaborador();
    }
}

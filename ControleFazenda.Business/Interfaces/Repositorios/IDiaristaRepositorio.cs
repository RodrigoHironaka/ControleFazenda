using ControleFazenda.Business.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControleFazenda.Business.Interfaces.Repositorios
{
    public interface IDiaristaRepositorio : IRepositorio<Diarista>
    {
        Task<Diarista> ObterPorIdComColaborador(Guid Id);
        Task<IEnumerable<Diarista>> ObterTodosComColaborador();
    }
}

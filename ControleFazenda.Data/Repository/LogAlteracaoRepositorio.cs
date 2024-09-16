using ControleFazenda.Business.Entidades;
using ControleFazenda.Business.Interfaces.Repositorios;
using ControleFazenda.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControleFazenda.Data.Repository
{
    public class LogAlteracaoRepositorio : Repositorio<LogAlteracao>, ILogAlteracaoRepositorio
    {
        public LogAlteracaoRepositorio(ContextoPrincipal db) : base(db) { }
    }
}

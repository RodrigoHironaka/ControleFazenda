using ControleFazenda.Business.Entidades;
using ControleFazenda.Business.Interfaces.Repositorios;
using ControleFazenda.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace ControleFazenda.Data.Repository
{
    public class DiariaRepositorio : Repositorio<Diaria>, IDiariaRepositorio
    {
        public DiariaRepositorio(ContextoPrincipal db) : base(db) { }

       
    }
}

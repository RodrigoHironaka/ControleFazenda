using ControleFazenda.Business.Entidades;
using ControleFazenda.Business.Interfaces.Repositorios;
using ControleFazenda.Data.Context;

namespace ControleFazenda.Data.Repository
{
    public class ColaboradorRepositorio : Repositorio<Colaborador>, IColaboradorRepositorio
    {
        public ColaboradorRepositorio(ContextoPrincipal db) : base(db) { }
    }
}

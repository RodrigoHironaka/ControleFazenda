using ControleFazenda.Business.Entidades;
using ControleFazenda.Business.Interfaces.Repositorios;
using ControleFazenda.Data.Context;

namespace ControleFazenda.Data.Repository
{
    public class FormaPagamentoRepositorio : Repositorio<FormaPagamento>, IFormaPagamentoRepositorio
    {
        public FormaPagamentoRepositorio(ContextoPrincipal db) : base(db) { }
    }
}

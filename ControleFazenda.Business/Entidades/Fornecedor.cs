using ControleFazenda.Business.Entidades.Componentes;
using ControleFazenda.Business.Entidades.Enum;

namespace ControleFazenda.Business.Entidades
{
    public class Fornecedor : Pessoa
    {
        public Situacao Situacao { get; set; } = Situacao.Ativo;
        public ICollection<NFe> NFes { get; set; } = new List<NFe>();
    }
}

using ControleFazenda.Business.Entidades.Componentes;
using ControleFazenda.Business.Entidades.Enum;

namespace ControleFazenda.Business.Entidades
{
    public class Colaborador : Pessoa
    {
        public virtual DateTime? Admissao { get; set; }
        public virtual DateTime? Demissao { get; set; }
        public Situacao Situacao { get; set; } = Situacao.Ativo;

        public ICollection<Recibo> Recibos { get; set; } = new List<Recibo>();
        public ICollection<Vale> Vales { get; set; } = new List<Vale>();
        public ICollection<Diaria> Diarias { get; set; } = new List<Diaria>();
    }
}

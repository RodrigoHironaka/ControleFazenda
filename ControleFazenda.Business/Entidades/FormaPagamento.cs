using ControleFazenda.Business.Entidades.Componentes;
using ControleFazenda.Business.Entidades.Enum;

namespace ControleFazenda.Business.Entidades
{
    public class FormaPagamento : Entidade
    {
        public string? Nome { get; set; }
        public Int32 QtdParcelamento { get; set; }
        public Int32 PeriodoParcelamento { get; set; }
        public Situacao Situacao { get; set; } = Situacao.Ativo;

        public ICollection<FluxoCaixa> FluxosCaixa { get; set; } = new List<FluxoCaixa>();

    }
}

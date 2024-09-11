using ControleFazenda.Business.Entidades.Componentes;
using ControleFazenda.Business.Entidades.Enum;

namespace ControleFazenda.Business.Entidades
{
    public class FluxoCaixa : Entidade
    {
        public string? Descricao { get; set; }
        public Decimal Valor { get; set; }
        public DateTime Data { get; set; }
        public DebitoCredito DebitoCredito { get; set; } //Débito = Pagamento | Crédito = Recebimento

        public Guid FormaPagamentoId { get; set; }
        public FormaPagamento? FormaPagamento { get; set; }

        public Guid CaixaId { get; set; }
        public Caixa? Caixa { get; set; }
    }
}

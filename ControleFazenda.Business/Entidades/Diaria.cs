using ControleFazenda.Business.Entidades.Componentes;
using ControleFazenda.Business.Entidades.Enum;

namespace ControleFazenda.Business.Entidades
{
    public class Diaria :  Entidade
    {
        public TipoPeriodo TipoPeriodo { get; set; }
        public DateTime Data { get; set; }
        public TimeSpan? EntradaManha { get; set; }
        public TimeSpan? SaidaManha { get; set; }
        public TimeSpan? EntradaTarde { get; set; }
        public TimeSpan? SaidaTarde { get; set; }
        public decimal? Valor { get; set; }
        public string? Identificador { get; set; }
        public SituacaoPagamento SituacaoPagamento { get; set; }

        public Guid DiaristaId { get; set; }
        public Diarista? Diarista { get; set; }


    }
}

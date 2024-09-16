using ControleFazenda.Business.Entidades.Componentes;

namespace ControleFazenda.Business.Entidades
{
    public class Recibo : Entidade
    {
        public Int64 Numero { get; set; }
        public decimal Valor { get; set; }
        public DateTime Data { get; set; }
        public string? Referente { get; set; }
        public string? NumeroCheque { get; set; }
        public string? BancoCheque { get; set; }
        public string? ContaCheque { get; set; }

        public Guid ColaboradorId { get; set; }
        public Colaborador? Colaborador { get; set; }
    }
}

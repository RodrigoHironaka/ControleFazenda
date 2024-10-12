using ControleFazenda.Business.Entidades.Componentes;
using ControleFazenda.Business.Entidades.Enum;

namespace ControleFazenda.Business.Entidades
{
    public class Vale : Entidade
    {
        public Int64 Numero { get; set; }
        public decimal Valor { get; set; }
        public DateTime Data { get; set; }
        public string? AutorizadoPor { get; set; }
        public Situacao? Situacao { get; set; }
        public Guid ColaboradorId { get; set; }
        public Colaborador? Colaborador { get; set; }
    }
}

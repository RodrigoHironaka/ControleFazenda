using ControleFazenda.Business.Entidades.Componentes;

namespace ControleFazenda.Business.Entidades
{
    public class Diarista : Entidade
    {
        public Guid ColaboradorId { get; set; }
        public Colaborador? Colaborador { get; set; }
        public string? Descricao { get; set; }

        public List<Diaria>? Diarias { get; set; }
    }
}

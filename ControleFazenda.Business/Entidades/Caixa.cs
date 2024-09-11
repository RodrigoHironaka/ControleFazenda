using ControleFazenda.Business.Entidades.Componentes;
using ControleFazenda.Business.Entidades.Enum;

namespace ControleFazenda.Business.Entidades
{
    public class Caixa : Entidade
    {
        public SituacaoCaixa Situacao { get; set; }
        public Int64 Numero { get; set; }
        public IEnumerable<FluxoCaixa>? FluxosCaixa { get; set; }
    }
}

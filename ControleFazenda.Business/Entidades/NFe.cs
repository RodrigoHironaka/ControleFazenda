using ControleFazenda.Business.Entidades.Componentes;
using ControleFazenda.Business.Entidades.Enum;

namespace ControleFazenda.Business.Entidades
{
    public class NFe : Entidade
    {
        public Int64? Numero { get; set; }
        public Guid? FornecedorId { get; set; }
        public Fornecedor? Fornecedor { get; set; }
        public decimal? Valor { get; set; }
        public DateTime? Emissao { get; set; }
        public DateTime? RecebimentoNFe { get; set; }
        public TipoNFe TipoNFe { get; set; } = TipoNFe.Produto;
        public string? CaminhoArquivo { get; set; }
    }
}

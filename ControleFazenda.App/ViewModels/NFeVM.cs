using Microsoft.AspNetCore.Identity;
using NumeroExtenso;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using ControleFazenda.Business.Entidades.Enum;
using System.ComponentModel.DataAnnotations.Schema;

namespace ControleFazenda.App.ViewModels
{
    public class NFeVM
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [DisplayName("Número")]
        public Int64 Numero { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [Moeda]
        public decimal Valor { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [DisplayName("Emissão")]
        public DateTime Emissao { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [DisplayName("Recebi NFe em")]
        public DateTime RecebimentoNFe { get; set; } = DateTime.Now;

        [DisplayName("Tipo NFe")]
        public TipoNFe TipoNFe { get; set; } = TipoNFe.Produto;

        [DisplayName("Arquivo")]
        public string? CaminhoArquivo { get; set; }

        public IFormFile? Arquivo { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [DisplayName("Fornecedor")]
        public Guid FornecedorId { get; set; }
        public FornecedorVM? Fornecedor { get; set; }
        public IEnumerable<FornecedorVM>? Fornecedores { get; set; }

        [ScaffoldColumn(false)]
        public DateTime DataCadastro { get; set; }
        [ScaffoldColumn(false)]
        public DateTime DataAlteracao { get; set; }
        [ScaffoldColumn(false)]
        public Guid UsuarioCadastroId { get; set; }
        [ScaffoldColumn(false)]
        public Guid UsuarioAlteracaoId { get; set; }
        [ScaffoldColumn(false)]
        public IdentityUser? UsuarioCadastro { get; set; }
        [ScaffoldColumn(false)]
        public IdentityUser? UsuarioAlteracao { get; set; }

        public string _InfoCadastro
        {
            get
            {
                return $"Criação: {UsuarioCadastro?.UserName} - {DataCadastro}";
            }
        }
        public string _InfoAlteracao
        {
            get
            {
                return $"Alteração: {UsuarioAlteracao?.UserName} - {DataAlteracao}";
            }
        }
    }
}

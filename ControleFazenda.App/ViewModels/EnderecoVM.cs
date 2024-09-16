using ControleFazenda.Business.Entidades;
using ControleFazenda.Business.Entidades.Enum;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ControleFazenda.App.ViewModels
{
    public class EnderecoVM
    {
        [Key]
        public Guid Id { get; set; }

        [StringLength(200, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 2)]
        public string? Logradouro { get; set; }

        [DisplayName("Nº")]
        public string? Numero { get; set; }

        public string? Bairro { get; set; }

        public string? Complemento { get; set; }

        public string? Referencia { get; set; }

        public bool Principal { get; set; } = false;

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [DisplayName("Situação")] 
        public Situacao Situacao { get; set; }=Situacao.Ativo;

        public Guid FornecedorId { get; set; }
        public Fornecedor? Fornecedor { get; set; }
        public Guid ColaboradorId { get; set; }
        public Colaborador? Colaborador { get; set; }
       
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

using ControleFazenda.Business.Entidades.Enum;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace ControleFazenda.App.ViewModels
{
    public class CaixaVM
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [DisplayName("Situação")]
        public SituacaoCaixa Situacao { get; set; } = SituacaoCaixa.Aberto;

        [DisplayName("Número")]
        public Int64 Numero { get; set; }

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

        public IEnumerable<FluxoCaixaVM>? FluxosCaixa { get; set; }

        public string TotalFluxo
        {
            get
            {
                if (FluxosCaixa != null)
                    return FluxosCaixa.Sum(x => x.Valor).ToString("N2");
                else
                    return 0.ToString("N2");
            }
        }
    }
}



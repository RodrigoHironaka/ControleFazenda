using ControleFazenda.Business.Entidades.Enum;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using ControleFazenda.Business.Entidades;

namespace ControleFazenda.App.ViewModels
{
    public class DiaristaVM
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [DisplayName("Descrição")]
        public string? Descricao { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [DisplayName("Diarista")]
        public Guid ColaboradorId { get; set; }
        public ColaboradorVM? Colaborador { get; set; }
        public IEnumerable<ColaboradorVM>? Colaboradores { get; set; }

        public List<DiariaVM>? Diarias { get; set; } = new List<DiariaVM>();

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

        public string TotalDiarias
        {
            get
            {
                if(Diarias != null && Diarias.Count > 0)
                {
                    return Diarias.Sum(x => x.Valor).ToString("C2");
                }
                else
                {
                    return 0.ToString("C2");
                }
            }
        }
    }
}


﻿using ControleFazenda.Business.Entidades.Enum;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using ControleFazenda.Business.Entidades;

namespace ControleFazenda.App.ViewModels
{
    public class DiariaVM
    {
        [Key]
        public Guid Id { get; set; }

        [DisplayName("Entrada Manhã")]
        public TimeSpan? EntradaManha { get; set; } = new TimeSpan(07,00,00);
        [DisplayName("Saída Manhã")]
        public TimeSpan? SaidaManha { get; set; } = new TimeSpan(11,00,00);
        [DisplayName("Entrada Tarde")]
        public TimeSpan? EntradaTarde { get; set; } = new TimeSpan(13,00,00);
        [DisplayName("Saída Tarde")]
        public TimeSpan? SaidaTarde { get; set; } = new TimeSpan(17, 00, 00);
        
        public string? Identificador { get; set; }

        [DisplayName("Quantos Dias?")]
        public Int32 QuantosDias { get; set; } = 1;
        [Moeda]
        public Decimal Valor { get; set; }
        public DateTime? Data { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [DisplayName("Período")]
        public TipoPeriodo TipoPeriodo { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [DisplayName("Situação Pagamento")]
        public SituacaoPagamento SituacaoPagamento { get; set; }

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

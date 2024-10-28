using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControleFazenda.Business.Entidades.Validacoes
{
    public class DiaristaValidacao : AbstractValidator<Diarista>
    {
        public DiaristaValidacao()
        {
            RuleFor(x => x.ColaboradorId)
                .NotNull().WithMessage("O campo {PropertyName} é obrigatório!");
            
            RuleFor(x => x.Descricao)
                .NotEmpty()
                .NotNull().WithMessage("O campo {PropertyName} é obrigatório!");
        }
    }
}

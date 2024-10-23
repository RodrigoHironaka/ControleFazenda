using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControleFazenda.Business.Entidades.Validacoes
{
    public class DiariaValidacao : AbstractValidator<Diaria>
    {
        public DiariaValidacao()
        {
            RuleFor(x => x.Valor)
            .NotNull().WithMessage("O campo {PropertyName} precisa ser fornecido");

            RuleFor(x => x.Data)
            .NotNull().WithMessage("O campo {PropertyName} precisa ser fornecido");

            RuleFor(x => x.ColaboradorId)
            .NotNull().WithMessage("O campo {PropertyName} é obrigatório!");
        }
    }
}

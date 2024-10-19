using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControleFazenda.Business.Entidades.Validacoes
{
    public class MaloteValidacao : AbstractValidator<Malote>
    {
        public MaloteValidacao()
        {
            RuleFor(x => x.Numero)
          .NotEmpty().WithMessage("O campo {PropertyName} precisa ser fornecido");

            RuleFor(x => x.Descricao)
         .NotEmpty().WithMessage("O campo {PropertyName} precisa ser fornecido");
        }
    }
}

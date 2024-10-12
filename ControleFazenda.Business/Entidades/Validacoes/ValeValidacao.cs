using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControleFazenda.Business.Entidades.Validacoes
{
    public class ValeValidacao : AbstractValidator<Vale>
    {
        public ValeValidacao()
        {
            RuleFor(x => x.Numero)
            .NotEmpty().WithMessage("O campo {PropertyName} precisa ser fornecido");

            RuleFor(x => x.Valor)
           .NotEmpty().WithMessage("O campo {PropertyName} precisa ser fornecido");

            RuleFor(x => x.AutorizadoPor)
           .NotEmpty().WithMessage("O campo {PropertyName} precisa ser fornecido")
           .Length(2, 100).WithMessage("O campo {PropertyName} precisa ter entre {MinLength} e {MaxLenght} caracteres");

            RuleFor(x => x.Data)
                .NotNull().WithMessage("O campo {PropertyName} é obrigatório!");

            RuleFor(x => x.ColaboradorId)
                .NotNull().WithMessage("O campo {PropertyName} é obrigatório!");

            RuleFor(x => x.Situacao)
           .NotNull().WithMessage("O campo {PropertyName} é obrigatório!");
        }
    }
}

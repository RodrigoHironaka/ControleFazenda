using FluentValidation;

namespace ControleFazenda.Business.Entidades.Validacoes
{
    public class ReciboValidacao : AbstractValidator<Recibo>
    {
        public ReciboValidacao()
        {
            RuleFor(x => x.Numero)
          .NotEmpty().WithMessage("O campo {PropertyName} precisa ser fornecido");

            RuleFor(x => x.Valor)
         .NotEmpty().WithMessage("O campo {PropertyName} precisa ser fornecido");

            RuleFor(x => x.Referente)
           .NotEmpty().WithMessage("O campo {PropertyName} precisa ser fornecido")
           .Length(2, 100).WithMessage("O campo {PropertyName} precisa ter entre {MinLength} e {MaxLenght} caracteres");

            RuleFor(x => x.Data)
                .NotNull().WithMessage("O campo {PropertyName} é obrigatório!");

            RuleFor(x => x.ColaboradorId)
                .NotNull().WithMessage("O campo {PropertyName} é obrigatório!");
        }
    }
}

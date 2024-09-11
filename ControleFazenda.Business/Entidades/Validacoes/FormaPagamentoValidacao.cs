using FluentValidation;

namespace ControleFazenda.Business.Entidades.Validacoes
{
    public class FormaPagamentoValidacao : AbstractValidator<FormaPagamento>
    {
        public FormaPagamentoValidacao()
        {
            RuleFor(x => x.Nome)
           .NotEmpty().WithMessage("O campo {PropertyName} precisa ser fornecido")
           .Length(2, 100).WithMessage("O campo {PropertyName} precisa ter entre {MinLength} e {MaxLenght} caracteres");

            RuleFor(x => x.Situacao)
           .NotNull().WithMessage("O campo {PropertyName} é obrigatório!");
        }
    }
}

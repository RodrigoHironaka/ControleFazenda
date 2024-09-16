using FluentValidation;

namespace ControleFazenda.Business.Entidades.Validacoes
{
    internal class ColaboradorValidacao : AbstractValidator<Colaborador>
    {
        public ColaboradorValidacao()
        {
            RuleFor(x => x.RazaoSocial)
            .NotEmpty().WithMessage("O campo {PropertyName} precisa ser fornecido")
            .Length(2, 100).WithMessage("O campo {PropertyName} precisa ter entre {MinLength} e {MaxLenght} caracteres");

            RuleFor(x => x.Situacao)
           .NotNull().WithMessage("O campo {PropertyName} é obrigatório!");
        }
    }
}

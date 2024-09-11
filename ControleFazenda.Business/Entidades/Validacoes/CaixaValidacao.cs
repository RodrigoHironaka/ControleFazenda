using FluentValidation;

namespace ControleFazenda.Business.Entidades.Validacoes
{
    public class CaixaValidacao : AbstractValidator<Caixa>
    {
        public CaixaValidacao()
        {
            RuleFor(x => x.Situacao).NotNull().WithMessage("O campo {PropertyName} é obrigatório!");
        }
    }
}

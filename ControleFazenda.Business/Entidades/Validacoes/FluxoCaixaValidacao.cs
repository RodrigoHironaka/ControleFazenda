using FluentValidation;

namespace ControleFazenda.Business.Entidades.Validacoes
{
    public class FluxoCaixaValidacao :AbstractValidator<FluxoCaixa>
    {
        public FluxoCaixaValidacao()
        {
            RuleFor(x => x.Valor)
          .NotNull().WithMessage("O campo {PropertyName} precisa ser fornecido");

            RuleFor(x => x.Data)
           .NotNull().WithMessage("O campo {PropertyName} precisa ser fornecido");

            RuleFor(x => x.DebitoCredito)
           .NotNull().WithMessage("O campo {PropertyName} é obrigatório!");

            RuleFor(x => x.FormaPagamentoId)
           .NotNull().WithMessage("O campo {PropertyName} é obrigatório!");

            RuleFor(x => x.CaixaId)
           .NotNull().WithMessage("O campo {PropertyName} é obrigatório!");
        }
    }
}

using FluentValidation;

namespace ControleFazenda.Business.Entidades.Validacoes
{
    public class NFeValidacao : AbstractValidator<NFe>
    {
        public NFeValidacao()
        {
            RuleFor(x => x.Numero).NotNull().WithMessage("O campo {PropertyName} é obrigatório!");
            RuleFor(x => x.FornecedorId).NotNull().WithMessage("O campo {PropertyName} é obrigatório!");
            RuleFor(x => x.Valor).NotNull().WithMessage("O campo {PropertyName} é obrigatório!");
            RuleFor(x => x.Emissao).NotNull().WithMessage("O campo {PropertyName} é obrigatório!");
            RuleFor(x => x.RecebimentoNFe).NotNull().WithMessage("O campo {PropertyName} é obrigatório!");
            RuleFor(x => x.TipoNFe).NotNull().WithMessage("O campo {PropertyName} é obrigatório!");
        }
    }
}

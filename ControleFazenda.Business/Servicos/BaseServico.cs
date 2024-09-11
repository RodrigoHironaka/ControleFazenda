using ControleFazenda.Business.Entidades.Componentes;
using ControleFazenda.Business.Interfaces;
using ControleFazenda.Business.Notificacoes;
using FluentValidation;
using FluentValidation.Results;

namespace ControleFazenda.Business.Servicos
{
    public abstract class BaseServico
    {
        private readonly INotificador _notificador;

        protected BaseServico(INotificador notificador)
        {
            _notificador = notificador;
        }
        protected void Notificar(ValidationResult validationResult)
        {
            foreach (var error in validationResult.Errors)
            {
                Notificar(error.ErrorMessage);
            }
        }

        protected void Notificar(string mensagem)
        {
            //Propaga esse erro ate a camada de apresentação
            _notificador.Handle(new Notificacao(mensagem));
        }

        protected bool ExecutarValidacao<TV, TE>(TV validacao, TE entidade) where TV : AbstractValidator<TE> where TE : Entidade
        {
            var validator = validacao.Validate(entidade);
            if (validator.IsValid) return true;

            Notificar(validator);
            return false;
        }
    }
}

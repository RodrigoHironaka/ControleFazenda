﻿using ControleFazenda.Business.Interfaces;

namespace ControleFazenda.Business.Notificacoes
{

    public class Notificador : INotificador
    {
        private List<Notificacao> _notificacoes;
        public Notificador()
        {
            _notificacoes = new List<Notificacao>();
        }

        public void Handle(Notificacao notificacao)
        {
            _notificacoes.Add(notificacao);
        }

        public List<Notificacao> ObterNotificacoes() => _notificacoes;

        public bool TemNotificacao() => _notificacoes.Any();
    }
}

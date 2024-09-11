using ControleFazenda.Business.Entidades;
using ControleFazenda.Business.Entidades.Validacoes;
using ControleFazenda.Business.Interfaces;
using ControleFazenda.Business.Interfaces.Repositorios;
using ControleFazenda.Business.Interfaces.Servicos;
using System.Linq.Expressions;

namespace ControleFazenda.Business.Servicos
{
    public class FormaPagamentoServico : BaseServico, IFormaPagamentoServico
    {
        private readonly IFormaPagamentoRepositorio _formaPagamentoRepositorio;

        public FormaPagamentoServico(IFormaPagamentoRepositorio formaPagamentoRepositorio, INotificador notificador) : base(notificador)
        {
            _formaPagamentoRepositorio = formaPagamentoRepositorio;
        }

        public async Task<FormaPagamento> ObterPorId(Guid id)
        {
            return await _formaPagamentoRepositorio.ObterPorId(id);
        }

        public async Task<List<FormaPagamento>> ObterTodos()
        {
            return await _formaPagamentoRepositorio.ObterTodos();
        }

        public async Task Adicionar(FormaPagamento entity)
        {
            if (!ExecutarValidacao(new FormaPagamentoValidacao(), entity)) return;
            await _formaPagamentoRepositorio.Adicionar(entity);
        }

        public async Task Atualizar(FormaPagamento entity)
        {
            if (!ExecutarValidacao(new FormaPagamentoValidacao(), entity)) return;
            await _formaPagamentoRepositorio.Atualizar(entity);
        }

        public async Task Remover(Guid id)
        {
            await _formaPagamentoRepositorio.Remover(id);
        }

        public async Task<IEnumerable<FormaPagamento>> Buscar(Expression<Func<FormaPagamento, bool>> predicate)
        {
            return await _formaPagamentoRepositorio.Buscar(predicate);
        }

        public void Dispose()
        {
            _formaPagamentoRepositorio?.Dispose();
        }
    }
}

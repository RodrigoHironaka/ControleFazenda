using ControleFazenda.Business.Entidades;
using ControleFazenda.Business.Entidades.Validacoes;
using ControleFazenda.Business.Interfaces;
using ControleFazenda.Business.Interfaces.Repositorios;
using ControleFazenda.Business.Interfaces.Servicos;
using System.Linq.Expressions;

namespace ControleFazenda.Business.Servicos
{
    public class ReciboServico : BaseServico, IReciboServico
    {
        private readonly IReciboRepositorio _reciboRepositorio;

        public ReciboServico(IReciboRepositorio reciboRepositorio, INotificador notificador) : base(notificador)
        {
            _reciboRepositorio = reciboRepositorio;
        }

        public async Task<Recibo> ObterPorId(Guid id)
        {
            return await _reciboRepositorio.ObterPorId(id);
        }

        public async Task<List<Recibo>> ObterTodos()
        {
            return await _reciboRepositorio.ObterTodos();
        }

        public async Task Adicionar(Recibo entity)
        {
            if (!ExecutarValidacao(new ReciboValidacao(), entity)) return;
            await _reciboRepositorio.Adicionar(entity);
        }

        public async Task Atualizar(Recibo entity)
        {
            if (!ExecutarValidacao(new ReciboValidacao(), entity)) return;
            await _reciboRepositorio.Atualizar(entity);
        }

        public async Task Remover(Guid id)
        {
            await _reciboRepositorio.Remover(id);
        }

        public async Task<IEnumerable<Recibo>> Buscar(Expression<Func<Recibo, bool>> predicate)
        {
            return await _reciboRepositorio.Buscar(predicate);
        }

        public void Dispose()
        {
            _reciboRepositorio?.Dispose();
        }

        public async Task<long> ObterNumeroUltimoRecibo()
        {
            return await _reciboRepositorio.ObterNumeroUltimoRecibo();
        }

        public async Task<Recibo> ObterPorIdComColaborador(Guid Id)
        {
            return await _reciboRepositorio.ObterPorIdComColaborador(Id);
        }

        public async Task<IEnumerable<Recibo>> ObterTodosComColaborador()
        {
            return await _reciboRepositorio.ObterTodosComColaborador();
        }
    }
}

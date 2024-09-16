using ControleFazenda.Business.Entidades;
using ControleFazenda.Business.Entidades.Validacoes;
using ControleFazenda.Business.Interfaces;
using ControleFazenda.Business.Interfaces.Repositorios;
using ControleFazenda.Business.Interfaces.Servicos;
using ControleFazenda.Business.Servicos;
using System.Linq.Expressions;

namespace ControleFazenda.Business.Servicos
{
    public class ColaboradorServico : BaseServico, IColaboradorServico
    {
        private readonly IColaboradorRepositorio _colaboradorRepositorio;

        public ColaboradorServico(IColaboradorRepositorio colaboradorRepositorio,
            INotificador notificador) : base(notificador)
        {
            _colaboradorRepositorio = colaboradorRepositorio;
        }

        public async Task Adicionar(Colaborador entity)
        {
            //Validar o estado da entidade
            if (!ExecutarValidacao(new ColaboradorValidacao(), entity)) return;
            await _colaboradorRepositorio.Adicionar(entity);
        }

        public async Task Atualizar(Colaborador entity)
        {
            if (!ExecutarValidacao(new ColaboradorValidacao(), entity)) return;
            await _colaboradorRepositorio.Atualizar(entity);
        }

        public async Task<IEnumerable<Colaborador>> Buscar(Expression<Func<Colaborador, bool>> predicate)
        {
            return await _colaboradorRepositorio.Buscar(predicate);
        }

        public void Dispose()
        {
            _colaboradorRepositorio?.Dispose();
        }

        public async Task<Colaborador> ObterPorId(Guid id)
        {
            return await _colaboradorRepositorio.ObterPorId(id);
        }

        public async Task<List<Colaborador>> ObterTodos()
        {
            return await _colaboradorRepositorio.ObterTodos();
        }

        public async Task Remover(Guid id)
        {
            await _colaboradorRepositorio.Remover(id);
        }
    }
}

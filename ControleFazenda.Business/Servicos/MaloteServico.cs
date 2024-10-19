using ControleFazenda.Business.Entidades;
using ControleFazenda.Business.Entidades.Validacoes;
using ControleFazenda.Business.Interfaces;
using ControleFazenda.Business.Interfaces.Repositorios;
using ControleFazenda.Business.Interfaces.Servicos;
using System.Linq.Expressions;

namespace ControleFazenda.Business.Servicos
{
    public class MaloteServico : BaseServico, IMaloteServico
    {
        private readonly IMaloteRepositorio _maloteRepositorio;

        public MaloteServico(IMaloteRepositorio maloteRepositorio, INotificador notificador) : base(notificador)
        {
            _maloteRepositorio = maloteRepositorio;
        }

        public async Task<Malote> ObterPorId(Guid id)
        {
            return await _maloteRepositorio.ObterPorId(id);
        }

        public async Task<List<Malote>> ObterTodos()
        {
            return await _maloteRepositorio.ObterTodos();
        }

        public async Task Adicionar(Malote entity)
        {
            if (!ExecutarValidacao(new MaloteValidacao(), entity)) return;
            await _maloteRepositorio.Adicionar(entity);
        }

        public async Task Atualizar(Malote entity)
        {
            if (!ExecutarValidacao(new MaloteValidacao(), entity)) return;
            await _maloteRepositorio.Atualizar(entity);
        }

        public async Task Remover(Guid id)
        {
            await _maloteRepositorio.Remover(id);
        }

        public async Task<IEnumerable<Malote>> Buscar(Expression<Func<Malote, bool>> predicate)
        {
            return await _maloteRepositorio.Buscar(predicate);
        }

        public void Dispose()
        {
            _maloteRepositorio?.Dispose();
        }
    }
}


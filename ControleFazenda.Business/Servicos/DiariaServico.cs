using ControleFazenda.Business.Entidades;
using ControleFazenda.Business.Entidades.Validacoes;
using ControleFazenda.Business.Interfaces;
using ControleFazenda.Business.Interfaces.Repositorios;
using ControleFazenda.Business.Interfaces.Servicos;
using System.Linq.Expressions;

namespace ControleFazenda.Business.Servicos
{
    public class DiariaServico : BaseServico, IDiariaServico
    {
        private readonly IDiariaRepositorio _diariaRepositorio;

        public DiariaServico(IDiariaRepositorio diariaRepositorio, INotificador notificador) : base(notificador)
        {
            _diariaRepositorio = diariaRepositorio;
        }

        public async Task<Diaria> ObterPorId(Guid id)
        {
            return await _diariaRepositorio.ObterPorId(id);
        }

        public async Task<List<Diaria>> ObterTodos()
        {
            return await _diariaRepositorio.ObterTodos();
        }

        public async Task Adicionar(Diaria entity)
        {
            if (!ExecutarValidacao(new DiariaValidacao(), entity)) return;
            await _diariaRepositorio.Adicionar(entity);
        }

        public async Task Atualizar(Diaria entity)
        {
            if (!ExecutarValidacao(new DiariaValidacao(), entity)) return;
            await _diariaRepositorio.Atualizar(entity);
        }

        public async Task Remover(Guid id)
        {
            await _diariaRepositorio.Remover(id);
        }

        public async Task<IEnumerable<Diaria>> Buscar(Expression<Func<Diaria, bool>> predicate)
        {
            return await _diariaRepositorio.Buscar(predicate);
        }

        public void Dispose()
        {
            _diariaRepositorio?.Dispose();
        }

        public async Task<Diaria> ObterPorIdComColaborador(Guid Id)
        {
            return await _diariaRepositorio.ObterPorIdComColaborador(Id);
        }

        public async Task<IEnumerable<Diaria>> ObterTodosComColaborador()
        {
            return await _diariaRepositorio.ObterTodosComColaborador();
        }
    }
}

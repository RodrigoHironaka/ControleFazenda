using ControleFazenda.Business.Entidades;
using ControleFazenda.Business.Entidades.Validacoes;
using ControleFazenda.Business.Interfaces;
using ControleFazenda.Business.Interfaces.Repositorios;
using ControleFazenda.Business.Interfaces.Servicos;
using System.Linq.Expressions;

namespace ControleFazenda.Business.Servicos
{
    public class FornecedorServico : BaseServico, IFornecedorServico
    {
        private readonly IFornecedorRepositorio _fornecedorRepositorio;

        public FornecedorServico(IFornecedorRepositorio fornecedorRepositorio,
            INotificador notificador) : base(notificador)
        {
            _fornecedorRepositorio = fornecedorRepositorio;
        }

        public async Task Adicionar(Fornecedor entity)
        {
            //Validar o estado da entidade
            if (!ExecutarValidacao(new FornecedorValidacao(), entity)) return;
            await _fornecedorRepositorio.Adicionar(entity);
        }

        public async Task Atualizar(Fornecedor entity)
        {
            if (!ExecutarValidacao(new FornecedorValidacao(), entity)) return;
            await _fornecedorRepositorio.Atualizar(entity);
        }

        public async Task<IEnumerable<Fornecedor>> Buscar(Expression<Func<Fornecedor, bool>> predicate)
        {
            return await _fornecedorRepositorio.Buscar(predicate);
        }

        public void Dispose()
        {
            _fornecedorRepositorio?.Dispose();
        }

        public async Task<Fornecedor> ObterPorId(Guid id)
        {
            return await _fornecedorRepositorio.ObterPorId(id);
        }

        public async Task<List<Fornecedor>> ObterTodos()
        {
            return await _fornecedorRepositorio.ObterTodos();
        }

        public async Task Remover(Guid id)
        {
            await _fornecedorRepositorio.Remover(id);
        }
    }
}


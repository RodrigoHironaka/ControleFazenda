using ControleFazenda.Business.Interfaces.Repositorios;
using ControleFazenda.Business.Interfaces;
using ControleFazenda.Business.Interfaces.Servicos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ControleFazenda.Business.Entidades;
using System.Linq.Expressions;
using ControleFazenda.Business.Entidades.Validacoes;

namespace ControleFazenda.Business.Servicos
{
    public class NFeServico : BaseServico, INFeServico
    {
        private readonly INFeRepositorio _nfeRepositorio;

        public NFeServico(INFeRepositorio nfeRepositorio, INotificador notificador) : base(notificador)
        {
            _nfeRepositorio = nfeRepositorio;
        }

        public async Task Adicionar(NFe entity)
        {
            if (!ExecutarValidacao(new NFeValidacao(), entity)) return;
            await _nfeRepositorio.Adicionar(entity);
        }

        public async Task Atualizar(NFe entity)
        {
            if (!ExecutarValidacao(new NFeValidacao(), entity)) return;
            await _nfeRepositorio.Atualizar(entity);
        }

        public async Task<IEnumerable<NFe>> Buscar(Expression<Func<NFe, bool>> predicate)
        {
            return await _nfeRepositorio.Buscar(predicate);
        }

        public void Dispose()
        {
            _nfeRepositorio?.Dispose();
        }

        public async Task<List<NFe>> ObterNFeComFornecedor(Expression<Func<NFe, bool>>? predicate = null)
        {
            return await _nfeRepositorio.ObterNFeComFornecedor(predicate);
        }

        public async Task<NFe> ObterPorId(Guid id)
        {
            return await _nfeRepositorio.ObterPorId(id);
        }

        public async Task<NFe> ObterPorIdComFornecedor(Guid Id)
        {
            return await _nfeRepositorio.ObterPorIdComFornecedor(Id);
        }

        public async Task<List<NFe>> ObterTodos()
        {
            return await _nfeRepositorio.ObterTodos();
        }

        public async Task<IEnumerable<NFe>> ObterTodosComFornecedor()
        {
            return await _nfeRepositorio.ObterTodosComFornecedor();
        }

        public async Task Remover(Guid id)
        {
            await _nfeRepositorio.Remover(id);
        }
    }
}

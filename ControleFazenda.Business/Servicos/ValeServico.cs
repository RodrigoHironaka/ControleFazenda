using ControleFazenda.Business.Entidades.Validacoes;
using ControleFazenda.Business.Entidades;
using ControleFazenda.Business.Interfaces.Repositorios;
using ControleFazenda.Business.Interfaces.Servicos;
using ControleFazenda.Business.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ControleFazenda.Business.Servicos
{
    public class ValeServico : BaseServico, IValeServico
    {
        private readonly IValeRepositorio _valeRepositorio;

        public ValeServico(IValeRepositorio valeRepositorio, INotificador notificador) : base(notificador)
        {
            _valeRepositorio = valeRepositorio;
        }

        public async Task<Vale> ObterPorId(Guid id)
        {
            return await _valeRepositorio.ObterPorId(id);
        }

        public async Task<List<Vale>> ObterTodos()
        {
            return await _valeRepositorio.ObterTodos();
        }

        public async Task Adicionar(Vale entity)
        {
            if (!ExecutarValidacao(new ValeValidacao(), entity)) return;
            await _valeRepositorio.Adicionar(entity);
        }

        public async Task Atualizar(Vale entity)
        {
            if (!ExecutarValidacao(new ValeValidacao(), entity)) return;
            await _valeRepositorio.Atualizar(entity);
        }

        public async Task Remover(Guid id)
        {
            await _valeRepositorio.Remover(id);
        }

        public async Task<IEnumerable<Vale>> Buscar(Expression<Func<Vale, bool>> predicate)
        {
            return await _valeRepositorio.Buscar(predicate);
        }

        public void Dispose()
        {
            _valeRepositorio?.Dispose();
        }

        public async Task<long> ObterNumeroUltimoVale()
        {
            return await _valeRepositorio.ObterNumeroUltimoVale();
        }

        public async Task<Vale> ObterPorIdComColaborador(Guid Id)
        {
            return await _valeRepositorio.ObterPorIdComColaborador(Id);
        }

        public async Task<IEnumerable<Vale>> ObterTodosComColaborador()
        {
            return await _valeRepositorio.ObterTodosComColaborador();
        }
    }
}

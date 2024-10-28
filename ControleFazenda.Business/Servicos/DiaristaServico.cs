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
    public class DiaristaServico : BaseServico, IDiaristaServico
    {
        private readonly IDiaristaRepositorio _diaristaRepositorio;

        public DiaristaServico(IDiaristaRepositorio diaristaRepositorio, INotificador notificador) : base(notificador)
        {
            _diaristaRepositorio = diaristaRepositorio;
        }

        public async Task<Diarista> ObterPorId(Guid id)
        {
            return await _diaristaRepositorio.ObterPorId(id);
        }

        public async Task<List<Diarista>> ObterTodos()
        {
            return await _diaristaRepositorio.ObterTodos();
        }

        public async Task Adicionar(Diarista entity)
        {
            if (!ExecutarValidacao(new DiaristaValidacao(), entity)) return;
            await _diaristaRepositorio.Adicionar(entity);
        }

        public async Task Atualizar(Diarista entity)
        {
            if (!ExecutarValidacao(new DiaristaValidacao(), entity)) return;
            await _diaristaRepositorio.Atualizar(entity);
        }

        public async Task Remover(Guid id)
        {
            await _diaristaRepositorio.Remover(id);
        }

        public async Task<IEnumerable<Diarista>> Buscar(Expression<Func<Diarista, bool>> predicate)
        {
            return await _diaristaRepositorio.Buscar(predicate);
        }

        public void Dispose()
        {
            _diaristaRepositorio?.Dispose();
        }

        public async Task<Diarista> ObterPorIdComColaborador(Guid Id)
        {
            return await _diaristaRepositorio.ObterPorIdComColaborador(Id);
        }

        public async Task<IEnumerable<Diarista>> ObterTodosComColaborador()
        {
            return await _diaristaRepositorio.ObterTodosComColaborador();
        }
    }
}

using ControleFazenda.Business.Entidades;
using ControleFazenda.Business.Entidades.Validacoes;
using ControleFazenda.Business.Interfaces;
using ControleFazenda.Business.Interfaces.Repositorios;
using ControleFazenda.Business.Interfaces.Servicos;
using ObjectsComparer;
using System.Linq.Expressions;
using System.Text;

namespace ControleFazenda.Business.Servicos
{
    public class LogAlteracaoServico : BaseServico, ILogAlteracaoServico
    {
        private readonly ILogAlteracaoRepositorio _logAlteracaoRepositorio;

        public LogAlteracaoServico(ILogAlteracaoRepositorio logAlteracaoRepositorio, INotificador notificador) : base(notificador)
        {
            _logAlteracaoRepositorio = logAlteracaoRepositorio;
        }
        public async Task Adicionar(LogAlteracao entity)
        {
            if (!ExecutarValidacao(new LogAlteracaoValidacao(), entity)) return;
            await _logAlteracaoRepositorio.Adicionar(entity);
        }

        public async Task Atualizar(LogAlteracao entity)
        {
            if (!ExecutarValidacao(new LogAlteracaoValidacao(), entity)) return;
            await _logAlteracaoRepositorio.Atualizar(entity);
        }

        public async Task<IEnumerable<LogAlteracao>> Buscar(Expression<Func<LogAlteracao, bool>> predicate)
        {
            return await _logAlteracaoRepositorio.Buscar(predicate);
        }


        public void Dispose()
        {
            _logAlteracaoRepositorio?.Dispose();
        }

        public async Task<LogAlteracao> ObterPorId(Guid id)
        {
            return await _logAlteracaoRepositorio.ObterPorId(id);
        }

        public async Task<List<LogAlteracao>> ObterTodos()
        {
            return await _logAlteracaoRepositorio.ObterTodos();
        }

        public async Task Remover(Guid id)
        {
            await _logAlteracaoRepositorio.Remover(id);
        }

        public async Task CompararAlteracoes<T>(T objetoAntigo, T objetoNovo, Guid usuarioId, string chave)
        {
            var comparer = new ObjectsComparer.Comparer<T>();
            comparer.AddComparerOverride<Guid>(DoNotCompareValueComparer.Instance, member => member.Name.Contains("Id"));
            comparer.IgnoreMember("DataCadastro");
            comparer.IgnoreMember("DataAlteracao");
            var igual = comparer.Compare(objetoAntigo, objetoNovo, out IEnumerable<Difference> diferencas);
            if (!igual)
            {
                await RegistrarLogModificacao(diferencas, usuarioId, chave);
            }
        }

        public async Task CompararAlteracoesComFiltros<T>(T objetoAntigo, T objetoNovo, Guid usuarioId, string chave, ObjectsComparer.Comparer<T> comparer)
        {
            comparer.AddComparerOverride<Guid>(DoNotCompareValueComparer.Instance, member => member.Name.Contains("Id"));
            comparer.IgnoreMember("DataCadastro");
            comparer.IgnoreMember("DataAlteracao");
            var igual = comparer.Compare(objetoAntigo, objetoNovo, out IEnumerable<Difference> diferencas);
            if (!igual)
            {
                await RegistrarLogModificacao(diferencas, usuarioId, chave);
            }
        }

        public async Task RegistrarLogModificacao(IEnumerable<Difference> diferencas, Guid usuarioId, string chave)
        {
            var historico = new StringBuilder();
            foreach (var item in diferencas)
            {
                if (item.Value1 != item.Value2 &&
                    item.Value2 != DateTime.MinValue.ToString() &&
                    !item.MemberPath.EndsWith(".DataGeracao.Value") &&
                    !item.MemberPath.EndsWith(".DataAlteracao.Value"))
                {
                    if (item.MemberPath.Contains("."))
                    {
                        if (item.MemberPath.EndsWith(".Nome") || item.MemberPath.EndsWith(".Value"))
                            historico.AppendLine(String.Format("Campo: {0} | Antes: {1} | Depois: {2}", item.MemberPath, item.Value1, item.Value2));
                        else
                        {
                            if (!String.IsNullOrEmpty(item.Value1) && !String.IsNullOrEmpty(item.Value2))
                            {
                                if (item.Value1 != item.Value2)
                                    historico.AppendLine(String.Format("Campo: {0} | Antes: {1} | Depois: {2}", item.MemberPath, item.Value1, item.Value2));
                            }

                        }
                    }
                    else
                        historico.AppendLine(String.Format("Campo: {0} | Antes: {1} | Depois: {2}", item.MemberPath, item.Value1, item.Value2));
                }
            }

            if (historico.Length > 0)
            {
                var log = new LogAlteracao
                {
                    DataCadastro = DateTime.Now,
                    UsuarioCadastroId = usuarioId,
                    Historico = historico.ToString(),
                    Chave = chave,
                };
                await Adicionar(log);
            }
        }
        public async Task RegistrarLogDiretamente(string historico, Guid usuarioId, string chave)
        {
            if (historico.Length > 0)
            {
                var log = new LogAlteracao
                {
                    DataCadastro = DateTime.Now,
                    UsuarioCadastroId = usuarioId,
                    Historico = historico.ToString(),
                    Chave = chave,
                };
                await Adicionar(log);

            }
        }

    }
}

using ControleFazenda.Business.Entidades;
using ObjectsComparer;

namespace ControleFazenda.Business.Interfaces.Servicos
{
    public interface ILogAlteracaoServico : IServico<LogAlteracao>
    {
        Task CompararAlteracoes<T>(T objetoAntigo, T objetoNovo, Guid usuarioId, String chave);
        Task CompararAlteracoesComFiltros<T>(T objetoAntigo, T objetoNovo, Guid usuarioId, String chave, ObjectsComparer.Comparer<T> comparer);
        Task RegistrarLogModificacao(IEnumerable<Difference> diferencas, Guid usuarioId, String chave);
        Task RegistrarLogDiretamente(String historico, Guid usuarioId, String chave);
    }
}

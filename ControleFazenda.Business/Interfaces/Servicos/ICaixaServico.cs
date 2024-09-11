using ControleFazenda.Business.Entidades;

namespace ControleFazenda.Business.Interfaces.Servicos
{
    public interface ICaixaServico : IServico<Caixa>
    {
        Task<Caixa> ObterCaixaAberto(string idUsuario);
        Task<Int64> ObteNumeroUltimoCaixa(string idUsuario);
        Task<Caixa> ObterPorIdComFluxosDeCaixa(Guid Id);
    }
}

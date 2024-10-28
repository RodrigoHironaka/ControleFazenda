using ControleFazenda.Business.Interfaces;
using ControleFazenda.Business.Interfaces.Repositorios;
using ControleFazenda.Business.Interfaces.Servicos;
using ControleFazenda.Business.Notificacoes;
using ControleFazenda.Business.Servicos;
using ControleFazenda.Data.Context;
using ControleFazenda.Data.Repository;
using Microsoft.AspNetCore.Mvc.DataAnnotations;

namespace ControleFazenda.App.Configurations
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection ResolveDependencies(this IServiceCollection services)
        {
            services.AddScoped<ContextoPrincipal>();

           
            services.AddScoped<ILogAlteracaoRepositorio, LogAlteracaoRepositorio>();
            services.AddScoped<IFormaPagamentoRepositorio, FormaPagamentoRepositorio>();
            services.AddScoped<ICaixaRepositorio, CaixaRepositorio>();
            services.AddScoped<IFluxoCaixaRepositorio, FluxoCaixaRepositorio>();
            services.AddScoped<IColaboradorRepositorio, ColaboradorRepositorio>(); 
            services.AddScoped<IFornecedorRepositorio, FornecedorRepositorio>();
            services.AddScoped<IReciboRepositorio, ReciboRepositorio>();
            services.AddScoped<INFeRepositorio, NFeRepositorio>();
            services.AddScoped<IValeRepositorio, ValeRepositorio>();
            services.AddScoped<IMaloteRepositorio, MaloteRepositorio>();
            services.AddScoped<IDiariaRepositorio, DiariaRepositorio>();
            services.AddScoped<IDiaristaRepositorio, DiaristaRepositorio>();


            services.AddSingleton<IValidationAttributeAdapterProvider, MoedaValidationAttributeAdapterProvider>();

            services.AddScoped<INotificador, Notificador>();
            services.AddScoped<ILogAlteracaoServico, LogAlteracaoServico>();
            services.AddScoped<IFormaPagamentoServico, FormaPagamentoServico>();
            services.AddScoped<ICaixaServico, CaixaServico>();
            services.AddScoped<IFluxoCaixaServico, FluxoCaixaServico>();
            services.AddScoped<IColaboradorServico, ColaboradorServico>();
            services.AddScoped<IFornecedorServico, FornecedorServico>();
            services.AddScoped<IReciboServico, ReciboServico>();
            services.AddScoped<INFeServico, NFeServico>();
            services.AddScoped<IValeServico, ValeServico>();
            services.AddScoped<IMaloteServico, MaloteServico>();
            services.AddScoped<IDiariaServico, DiariaServico>();
            services.AddScoped<IDiaristaServico, DiaristaServico>();
            return services;
        }
    }
}

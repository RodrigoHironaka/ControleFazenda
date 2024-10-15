using ControleFazenda.App.Data;
using ControleFazenda.Business.Entidades;
using ControleFazenda.Data.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ControleFazenda.App.Configurations
{
    public static class IdentityConfig
    {
        public static IServiceCollection AddIdentityConfiguration(this IServiceCollection services, ConfigurationManager configuration)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            var connectionString = configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Conexão string 'DefaultConnection' não encontrada.");
            services.AddDbContext<ContextoPrincipal>(options =>
                options.UseSqlServer(connectionString));
            services.AddDatabaseDeveloperPageExceptionFilter();

            //services.AddIdentity<Usuario, IdentityRole>()
            //        .AddEntityFrameworkStores<ContextoPrincipal>()
            //        .AddDefaultTokenProviders();

            services.AddDefaultIdentity<Usuario>(options =>
                options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ContextoPrincipal>();

            return services;
        }
    }
}

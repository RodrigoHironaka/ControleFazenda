using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ControleFazenda.App.ViewModels;

namespace ControleFazenda.App.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<ControleFazenda.App.ViewModels.FormaPagamentoVM> FormaPagamentoVM { get; set; } = default!;
        public DbSet<ControleFazenda.App.ViewModels.ColaboradorVM> ColaboradorVM { get; set; } = default!;
        public DbSet<ControleFazenda.App.ViewModels.FornecedorVM> FornecedorVM { get; set; } = default!;
        public DbSet<ControleFazenda.App.ViewModels.CaixaVM> CaixaVM { get; set; } = default!;
        public DbSet<ControleFazenda.App.ViewModels.FluxoCaixaVM> FluxoCaixaVM { get; set; } = default!;
        public DbSet<ControleFazenda.App.ViewModels.ReciboVM> ReciboVM { get; set; } = default!;
    }
}

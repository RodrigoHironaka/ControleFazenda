using ControleFazenda.Business.Entidades;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ControleFazenda.Data.Context
{
    public class ContextoPrincipal : IdentityDbContext<Usuario>
    {
        public ContextoPrincipal(DbContextOptions options) : base(options)
        {
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            ChangeTracker.AutoDetectChangesEnabled = false;
        }
        public DbSet<Caixa> Caixas { get; set; }
        public DbSet<FluxoCaixa> FluxosCaixa { get; set; }
        public DbSet<FormaPagamento> FormasPagamento { get; set; }
        public DbSet<LogAlteracao> LogsAlteracao { get; set; }
        public DbSet<Colaborador> Colaboradores { get; set; }
        public DbSet<Fornecedor> Fornecedores { get; set; }
        public DbSet<Recibo> Recibos { get; set; }
        public DbSet<NFe> NFes { get; set; }
        public DbSet<Vale> Vales { get; set; }
        public DbSet<Malote> Malotes { get; set; }
        public DbSet<Diaria> Diarias { get; set; }
        public DbSet<Diarista> Diaristas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var property in modelBuilder.Model.GetEntityTypes()
                .SelectMany(e => e.GetProperties()
                .Where(p => p.ClrType == typeof(string))))
                property.SetColumnType("varchar(100)");

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ContextoPrincipal).Assembly);

            //Desativando efeito de exclusão em cascata
            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
                relationship.DeleteBehavior = DeleteBehavior.ClientSetNull;

            modelBuilder.Entity<Colaborador>().OwnsOne(c => c.Endereco);
            modelBuilder.Entity<Fornecedor>().OwnsOne(c => c.Endereco);
            // Ignorar a propriedade DataAlteracao e UsuarioAlteracaoId
            //modelBuilder.Entity<LogAlteracao>(entity =>
            //{
            //    entity.Ignore(e => e.DataAlteracao);
            //    entity.Ignore(e => e.UsuarioAlteracaoId);
            //});

            base.OnModelCreating(modelBuilder);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            foreach (var entry in ChangeTracker.Entries().Where(entry => entry.Entity.GetType().GetProperty("DataCadastro") != null))
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Property("DataCadastro").CurrentValue = DateTime.Now;
                }

                if (entry.State == EntityState.Modified)
                {
                    entry.Property("DataCadastro").IsModified = false;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}

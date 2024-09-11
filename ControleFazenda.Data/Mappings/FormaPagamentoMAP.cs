using ControleFazenda.Business.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ControleFazenda.Data.Mappings
{
    public class FormaPagamentoMAP : IEntityTypeConfiguration<FormaPagamento>
    {
        public void Configure(EntityTypeBuilder<FormaPagamento> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.DataCadastro).IsRequired();
            builder.Property(x => x.DataAlteracao);
            builder.Property(x => x.UsuarioCadastroId);
            builder.Property(x => x.UsuarioAlteracaoId);
            builder.Property(x => x.QtdParcelamento);
            builder.Property(x => x.PeriodoParcelamento);
            builder.Property(x => x.Nome).IsRequired().HasColumnType("varchar(200)");
            builder.Property(x => x.Situacao).HasConversion<Int32>();
            builder.HasMany(x => x.FluxosCaixa).WithOne(x => x.FormaPagamento).HasForeignKey(x => x.FormaPagamentoId);
            builder.ToTable("FormasPagamento");
        }
    }
}

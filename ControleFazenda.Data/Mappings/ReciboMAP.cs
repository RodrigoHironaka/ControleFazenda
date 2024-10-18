using ControleFazenda.Business.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ControleFazenda.Data.Mappings
{
    public class ReciboMAP : IEntityTypeConfiguration<Recibo>
    {
        public void Configure(EntityTypeBuilder<Recibo> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.DataCadastro).IsRequired();
            builder.Property(x => x.DataAlteracao);
            builder.Property(x => x.UsuarioCadastroId);
            builder.Property(x => x.UsuarioAlteracaoId);
            builder.Property(x => x.Numero);
            builder.Property(x => x.Valor).HasPrecision(10, 5);
            builder.Property(x => x.Data);
            builder.Property(x => x.Referente).HasColumnType("varchar(500)");
            builder.Property(x => x.Numero).HasColumnType("varchar(50)");
            builder.Property(x => x.BancoCheque).HasColumnType("varchar(50)");
            builder.Property(x => x.ContaCheque).HasColumnType("varchar(50)");
            builder.HasOne(x => x.Colaborador).WithMany(x => x.Recibos).HasForeignKey(x => x.ColaboradorId);
            builder.Property(x => x.Fazenda).HasConversion<Int32>();

            builder.ToTable("Recibos");
        }
    }
}

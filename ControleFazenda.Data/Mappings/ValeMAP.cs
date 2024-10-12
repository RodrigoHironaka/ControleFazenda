using ControleFazenda.Business.Entidades;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace ControleFazenda.Data.Mappings
{
    public class ValeMAP : IEntityTypeConfiguration<Vale>
    {
        public void Configure(EntityTypeBuilder<Vale> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.DataCadastro).IsRequired();
            builder.Property(x => x.DataAlteracao);
            builder.Property(x => x.UsuarioCadastroId);
            builder.Property(x => x.UsuarioAlteracaoId);
            builder.Property(x => x.Numero).IsRequired(); 
            builder.Property(x => x.Valor).HasPrecision(10, 5).IsRequired();
            builder.Property(x => x.Data).IsRequired();
            builder.Property(x => x.AutorizadoPor).HasColumnType("varchar(100)").IsRequired();
            builder.Property(x => x.Situacao).HasConversion<Int32>();
            builder.HasOne(x => x.Colaborador).WithMany(x => x.Vales).HasForeignKey(x => x.ColaboradorId);


            builder.ToTable("Vales");
        }
    }
}

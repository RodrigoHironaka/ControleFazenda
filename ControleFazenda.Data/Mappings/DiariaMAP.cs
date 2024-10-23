using ControleFazenda.Business.Entidades;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControleFazenda.Data.Mappings
{
    public class DiariaMAP : IEntityTypeConfiguration<Diaria>
    {
        public void Configure(EntityTypeBuilder<Diaria> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.DataCadastro).IsRequired();
            builder.Property(x => x.DataAlteracao);
            builder.Property(x => x.UsuarioCadastroId);
            builder.Property(x => x.UsuarioAlteracaoId);
            builder.Property(x => x.Data);
            builder.Property(x => x.EntradaManha);
            builder.Property(x => x.SaidaManha);
            builder.Property(x => x.EntradaTarde);
            builder.Property(x => x.SaidaTarde);
            builder.Property(x => x.HorasTabalhadas);
            builder.Property(x => x.Observacao).HasColumnType("varchar(8000)");
            builder.Property(x => x.Identificador).HasColumnType("varchar(100)");
            builder.Property(x => x.Valor).HasPrecision(10, 5);
            builder.Property(x => x.TipoPeriodo).HasConversion<Int32>();
            builder.Property(x => x.Fazenda).HasConversion<Int32>();
            builder.HasOne(x => x.Colaborador).WithMany(x => x.Diarias).HasForeignKey(x => x.ColaboradorId);
            builder.ToTable("Diarias");
        }
    }
}

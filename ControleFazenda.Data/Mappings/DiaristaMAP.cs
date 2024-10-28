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
    public class DiaristaMAP : IEntityTypeConfiguration<Diarista>
    {
        public void Configure(EntityTypeBuilder<Diarista> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.DataCadastro).IsRequired();
            builder.Property(x => x.DataAlteracao);
            builder.Property(x => x.UsuarioCadastroId);
            builder.Property(x => x.UsuarioAlteracaoId);
            builder.Property(x => x.Descricao).HasColumnType("varchar(8000)");
            builder.Property(x => x.Fazenda).HasConversion<Int32>();
            builder.HasMany(x => x.Diarias).WithOne(x => x.Diarista).HasForeignKey(x => x.DiaristaId);
            builder.ToTable("Diaristas");
        }
    }
}

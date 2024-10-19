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
    public class MaloteMAP : IEntityTypeConfiguration<Malote>
    {
        public void Configure(EntityTypeBuilder<Malote> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.DataCadastro).IsRequired();
            builder.Property(x => x.DataAlteracao);
            builder.Property(x => x.UsuarioCadastroId);
            builder.Property(x => x.UsuarioAlteracaoId);
            builder.Property(x => x.Numero);
            builder.Property(x => x.Enviado);
            builder.Property(x => x.QuemLevou).HasColumnType("varchar(200)");
            builder.Property(x => x.Descricao).IsRequired().HasColumnType("varchar(8000)");
            builder.Property(x => x.Fazenda).HasConversion<Int32>();
            builder.ToTable("Malote");
        }
    }
}

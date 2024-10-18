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
    public class LogAlteracaoMAP : IEntityTypeConfiguration<LogAlteracao>
    {
        public void Configure(EntityTypeBuilder<LogAlteracao> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.DataCadastro).IsRequired();
            builder.Property(x => x.UsuarioCadastroId);
            builder.Property(x => x.Chave).IsRequired().HasColumnType("varchar(200)");
            builder.Property(x => x.Historico).IsRequired().HasColumnType("varchar(8000)");
            builder.Property(x => x.Fazenda).HasConversion<Int32>();
            builder.ToTable("LogsAlteracao");
        }
    }
}

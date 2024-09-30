using ControleFazenda.Business.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControleFazenda.Data.Mappings
{
    public class NFeMAP : IEntityTypeConfiguration<NFe>
    {
        public void Configure(EntityTypeBuilder<NFe> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.DataCadastro).IsRequired();
            builder.Property(x => x.DataAlteracao);
            builder.Property(x => x.UsuarioCadastroId);
            builder.Property(x => x.UsuarioAlteracaoId);
            builder.Property(x => x.Numero);
            builder.Property(x => x.CaminhoArquivo).HasColumnType("varchar(500)");
            builder.Property(x => x.Valor).HasPrecision(10, 5);
            builder.Property(x => x.Emissao);
            builder.Property(x => x.RecebimentoNFe);
            builder.Property(x => x.TipoNFe).HasConversion<Int32>();
            builder.HasOne(x => x.Fornecedor).WithMany(x => x.NFes).HasForeignKey(x => x.FornecedorId);


            builder.ToTable("NFes");
        }
    }
}

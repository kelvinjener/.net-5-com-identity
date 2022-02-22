using Identity.NET5.Business.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.NET5.Data.Mapping
{
    public class LogMapping : IEntityTypeConfiguration<Log>
    {

        public void Configure(EntityTypeBuilder<Log> builder)
        {
            builder.ToTable("tb_log");

            builder.HasKey(e => e.Id).HasName("id");

            builder.Property(e => e.Id)
            .HasColumnName("id");

            builder.Property(e => e.Tipo)
            .IsRequired()
            .HasMaxLength(45)
            .HasColumnName("tipo");

            builder.Property(e => e.Acao)
            .IsRequired()
            .HasMaxLength(45)
            .HasColumnName("acao");

            builder.Property(e => e.Descricao)
            .HasMaxLength(255)
            .HasColumnName("descricao");

            builder.Property(e => e.DataOcorrencia)
            .IsRequired()
            .HasColumnName("data_ocorrencia");

            builder.Property(e => e.Usuario)
            .IsRequired()
            .HasMaxLength(4256)
            .HasColumnName("usuario");
        }
    }
}

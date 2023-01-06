using DevIO.Business.Models.Fornecedores;
using System.Data.Entity.ModelConfiguration;

namespace DevIO.Infra.Data.Mappings
{
    public class EnderecoConfig : EntityTypeConfiguration<Endereco>
    {
        public EnderecoConfig()
        {
            HasKey(e => e.Id);

            Property(e => e.Logradouro)
                .IsRequired()
                .HasMaxLength(200);

            Property(e => e.Numero)
                .IsRequired()
                .HasMaxLength(50);

            Property(e => e.Cep)
                .IsRequired()
                .HasMaxLength(8)
                .IsFixedLength();

            Property(e => e.Cidade)
               .IsRequired()
               .HasMaxLength(100);

            Property(e => e.Estado)
               .IsRequired()
               .HasMaxLength(100);

            ToTable("Enderecos");
        }
    }
}

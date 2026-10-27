using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TarifarioAggregate = Nodo.Tarifario.Domain.Tarifario;

namespace Nodo.Tarifario.Infrastructure.Configurations;

public class TarifarioConfiguration : IEntityTypeConfiguration<TarifarioAggregate>
{
    public void Configure(EntityTypeBuilder<TarifarioAggregate> builder)
    {
        builder.ToTable("Tarifarios");
        builder.HasKey(t => t.Id);
        builder.Ignore(t => t.DomainEvents);
        builder.HasMany(t => t.Tarifas)
            .WithOne()
            .HasForeignKey("TarifarioId")
            .OnDelete(DeleteBehavior.Cascade);
        builder.Navigation(t => t.Tarifas).UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}

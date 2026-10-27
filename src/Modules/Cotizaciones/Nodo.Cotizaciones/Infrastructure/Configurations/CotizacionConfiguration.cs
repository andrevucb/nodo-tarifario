using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nodo.Cotizaciones.Domain;

namespace Nodo.Cotizaciones.Infrastructure.Configurations;

public class CotizacionConfiguration : IEntityTypeConfiguration<Cotizacion>
{
    public void Configure(EntityTypeBuilder<Cotizacion> builder)
    {
        builder.ToTable("Cotizaciones");
        builder.HasKey(c => c.Id);
        builder.Ignore(c => c.DomainEvents);
        builder.Property(c => c.ClienteId).IsRequired();
        builder.Property(c => c.RecursoId).IsRequired();
        builder.Property(c => c.Estado).HasConversion<string>().HasMaxLength(20);

        builder.OwnsOne(c => c.PrecioOfrecido, money =>
        {
            money.Property(m => m.Amount).HasColumnName("PrecioOfrecidoMonto").HasPrecision(18,2);
            money.Property(m => m.Currency).HasColumnName("PrecioOfrecidoMoneda").HasMaxLength(3);
        });

        builder.OwnsOne(c => c.Vigencia, vigencia =>
        {
            vigencia.Property(v => v.Emision).HasColumnName("VigenciaEmision");
            vigencia.Property(v => v.Vencimiento).HasColumnName("VigenciaVencimiento");
            vigencia.WithOwner();
        });
    }
}

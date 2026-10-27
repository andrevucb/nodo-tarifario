using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nodo.Tarifario.Domain;

namespace Nodo.Tarifario.Infrastructure.Configurations;

public class TarifaConfiguration : IEntityTypeConfiguration<Tarifa>
{
    public void Configure(EntityTypeBuilder<Tarifa> builder)
    {
        builder.ToTable("Tarifas");
        builder.HasKey(t => t.Id);
        builder.Property(t => t.TipoRecurso).IsRequired().HasMaxLength(100);
        builder.Property(t => t.EsHoraPico).IsRequired();

        builder.OwnsOne(t => t.TarifaBase, money =>
        {
            money.Property(m => m.Amount).HasColumnName("TarifaBaseMonto").HasPrecision(18,2);
            money.Property(m => m.Currency).HasColumnName("TarifaBaseMoneda").HasMaxLength(3);
        });

        builder.OwnsOne(t => t.Franja, franja =>
        {
            franja.Property(f => f.Inicio).HasColumnName("FranjaInicio");
            franja.Property(f => f.Fin).HasColumnName("FranjaFin");
        });

        builder.OwnsOne(t => t.Temporada, temporada =>
        {
            temporada.Property(s => s.Inicio).HasColumnName("TemporadaInicio");
            temporada.Property(s => s.Fin).HasColumnName("TemporadaFin");
            temporada.WithOwner();
        });
    }
}

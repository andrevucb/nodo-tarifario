namespace Nodo.Web.Presentacion;

public record RecursoDemo(Guid Id, string Nombre, string Tipo);

public record ClienteDemo(Guid Id, string Nombre, string Plan);

/// <summary>
/// Nombres legibles para los identificadores de prueba. Deben coincidir con los adaptadores InMemory
/// de Cálculo de precio total hasta que Espacios y Membresías expongan sus propios datos.
/// </summary>
public static class CatalogoDemo
{
    public static readonly IReadOnlyList<RecursoDemo> Recursos =
    [
        new(Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), "Sala de reuniones (6 personas)", "SalaReuniones"),
        new(Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), "Salón de eventos (50 personas)", "SalonEventos"),
    ];

    public static readonly IReadOnlyList<ClienteDemo> Clientes =
    [
        new(Guid.Parse("11111111-1111-1111-1111-111111111111"), "Estudio Andino", "Plan de equipo · 4 h incluidas · 10 %"),
        new(Guid.Parse("22222222-2222-2222-2222-222222222222"), "Lucía Méndez", "Plan individual · 2 h incluidas · 5 %"),
        new(Guid.Parse("33333333-3333-3333-3333-333333333333"), "Visitante", "Sin plan"),
    ];

    public static string NombreRecurso(Guid id) => Recursos.FirstOrDefault(r => r.Id == id)?.Nombre ?? Corto(id);

    public static string NombreCliente(Guid id) => Clientes.FirstOrDefault(c => c.Id == id)?.Nombre ?? Corto(id);

    private static string Corto(Guid id) => id.ToString()[..8];
}

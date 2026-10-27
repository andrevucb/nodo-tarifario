using Nodo.SharedKernel;

namespace Nodo.Web.Presentacion;

public static class Formato
{
    public static string Importe(Money money) => $"{money.Amount:N2} {money.Currency}";

    /// <summary>Las fechas del dominio se guardan en UTC; se muestran en hora local.</summary>
    public static string Fecha(DateTime utc) =>
        DateTime.SpecifyKind(utc, DateTimeKind.Utc).ToLocalTime().ToString("dd/MM/yyyy HH:mm");

    public static string Horario(TimeOnly inicio, TimeOnly fin) => $"{inicio:HH\\:mm} – {fin:HH\\:mm}";

    public static string TipoRecurso(string tipo) => tipo switch
    {
        "SalaReuniones" => "Sala de reuniones",
        "SalonEventos" => "Salón de eventos",
        "EscritorioFlexible" => "Escritorio flexible",
        _ => tipo
    };
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Nodo.CalculoPrecioTotal.Domain;
using Nodo.Web.Presentacion;

namespace Nodo.Web.Pages.Calculo;

public class IndexModel : PageModel
{
    private readonly ServicioCalculoPrecio _calculo;

    public IndexModel(ServicioCalculoPrecio calculo)
    {
        _calculo = calculo;
    }

    public DesglosePrecio? Desglose { get; set; }
    public string? Error { get; set; }

    // Valores del formulario, para volver a mostrarlos junto al resultado.
    public Guid RecursoId { get; set; } = CatalogoDemo.Recursos[0].Id;
    public int Horas { get; set; } = 6;
    public DateTime FechaHoraInicio { get; set; } = DateTime.Today.AddHours(10);
    public Guid ClienteId { get; set; } = CatalogoDemo.Clientes[0].Id;

    public void OnGet() { }

    public async Task<IActionResult> OnPostAsync(Guid recursoId, int horas, DateTime fechaHoraInicio, Guid clienteId)
    {
        (RecursoId, Horas, FechaHoraInicio, ClienteId) = (recursoId, horas, fechaHoraInicio, clienteId);
        try
        {
            Desglose = await _calculo.CalcularPrecioAsync(new ServicioCalculoPrecio.SolicitudCalculo(recursoId, horas, fechaHoraInicio, clienteId, null, null));
        }
        catch (Exception ex)
        {
            Error = ex.Message;
        }
        return Page();
    }
}

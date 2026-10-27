using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Nodo.CalculoPrecioTotal.Domain;

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

    public void OnGet() { }

    public async Task<IActionResult> OnPostAsync(Guid recursoId, int horas, DateTime fechaHoraInicio, Guid? clienteId)
    {
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

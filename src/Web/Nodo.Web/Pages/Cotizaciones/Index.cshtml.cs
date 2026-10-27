using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Nodo.Cotizaciones.Application;
using Nodo.Cotizaciones.Infrastructure;

namespace Nodo.Web.Pages.Cotizaciones;

public class IndexModel : PageModel
{
    private readonly ServicioCotizacion _servicio;
    private readonly CotizacionesDbContext _ctx;

    public IndexModel(ServicioCotizacion servicio, CotizacionesDbContext ctx)
    {
        _servicio = servicio;
        _ctx = ctx;
    }

    public List<Nodo.Cotizaciones.Domain.Cotizacion> Cotizaciones { get; set; } = [];
    public string? Mensaje { get; set; }
    public string? Error { get; set; }

    public async Task OnGetAsync()
    {
        var cotizaciones = await _ctx.Cotizaciones.ToListAsync();
        Cotizaciones = cotizaciones.OrderByDescending(c => c.Vigencia.Emision).ToList();
    }

    // El mismo cliente define a quién se emite la propuesta y qué plan se usa para calcular su precio.
    public async Task<IActionResult> OnPostEmitirAsync(Guid clienteId, Guid recursoId, int horas, DateTime fechaHoraInicio)
    {
        try
        {
            await _servicio.EmitirAsync(clienteId, recursoId, horas, fechaHoraInicio, clienteId);
            Mensaje = "Cotización emitida. Tiene 4 días de vigencia y no reserva el recurso (INV-03).";
        }
        catch (Exception ex)
        {
            Error = ex.Message;
        }
        await OnGetAsync();
        return Page();
    }

    public async Task<IActionResult> OnPostAceptarAsync(Guid cotizacionId)
    {
        try
        {
            await _servicio.AceptarAsync(cotizacionId);
            Mensaje = "Cotización aceptada. Para confirmar la reserva o el evento siguen haciendo falta las condiciones de cada área.";
        }
        catch (Exception ex)
        {
            Error = ex.Message;
        }
        await OnGetAsync();
        return Page();
    }

    public async Task<IActionResult> OnPostCaducarAsync(Guid cotizacionId)
    {
        try
        {
            await _servicio.RegistrarCaducidadAsync(cotizacionId);
            Mensaje = "Caducidad registrada.";
        }
        catch (Exception ex)
        {
            Error = ex.Message;
        }
        await OnGetAsync();
        return Page();
    }
}

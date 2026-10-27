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
        Cotizaciones = await _ctx.Cotizaciones.ToListAsync();
    }

    public async Task<IActionResult> OnPostEmitirAsync(Guid clienteId, Guid recursoId, int horas, DateTime fechaHoraInicio, Guid? clienteIdParaPlan)
    {
        try
        {
            await _servicio.EmitirAsync(clienteId, recursoId, horas, fechaHoraInicio, clienteIdParaPlan);
            Mensaje = "Cotización emitida.";
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
            Mensaje = "Cotización aceptada.";
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
            Mensaje = "Cotización marcada como caducada.";
        }
        catch (Exception ex)
        {
            Error = ex.Message;
        }
        await OnGetAsync();
        return Page();
    }
}

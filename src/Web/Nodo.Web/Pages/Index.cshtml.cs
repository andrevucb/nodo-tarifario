using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Nodo.Cotizaciones.Domain;
using Nodo.Cotizaciones.Infrastructure;
using Nodo.Tarifario.Infrastructure;

namespace Nodo.Web.Pages;

public class IndexModel : PageModel
{
    private readonly TarifarioDbContext _tarifario;
    private readonly CotizacionesDbContext _cotizaciones;

    public IndexModel(TarifarioDbContext tarifario, CotizacionesDbContext cotizaciones)
    {
        _tarifario = tarifario;
        _cotizaciones = cotizaciones;
    }

    public int TotalTarifas { get; set; }
    public int CotizacionesEmitidas { get; set; }
    public int CotizacionesAceptadas { get; set; }
    public int CotizacionesCaducadas { get; set; }

    public async Task OnGetAsync()
    {
        TotalTarifas = await _tarifario.Tarifas.CountAsync();
        var estados = await _cotizaciones.Cotizaciones.Select(c => c.Estado).ToListAsync();
        CotizacionesEmitidas = estados.Count(e => e == EstadoCotizacion.Emitida);
        CotizacionesAceptadas = estados.Count(e => e == EstadoCotizacion.Aceptada);
        CotizacionesCaducadas = estados.Count(e => e == EstadoCotizacion.Caducada);
    }
}

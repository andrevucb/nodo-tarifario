using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Nodo.Tarifario.Application;
using Nodo.Tarifario.Infrastructure;

namespace Nodo.Web.Pages.Tarifario;

public class IndexModel : PageModel
{
    private readonly TarifarioDbContext _ctx;
    private readonly ServicioActualizarTarifa _servicio;

    public IndexModel(TarifarioDbContext ctx, ServicioActualizarTarifa servicio)
    {
        _ctx = ctx;
        _servicio = servicio;
    }

    public List<Nodo.Tarifario.Domain.Tarifario> Tarifarios { get; set; } = [];

    public string? Mensaje { get; set; }
    public string? Error { get; set; }

    public async Task OnGetAsync()
    {
        Tarifarios = await _ctx.Tarifarios.Include(t => t.Tarifas).ToListAsync();
    }

    public async Task<IActionResult> OnPostActualizarAsync(Guid tarifarioId, Guid tarifaId, decimal nuevoImporte, string moneda)
    {
        try
        {
            var usuarioId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "anon";
            var rol = User.FindFirstValue(ClaimTypes.Role) ?? "Usuario";
            await _servicio.ActualizarAsync(tarifarioId, tarifaId, nuevoImporte, moneda, usuarioId, rol);
            Mensaje = "Tarifa actualizada correctamente.";
        }
        catch (Exception ex)
        {
            Error = ex.Message;
        }

        await OnGetAsync();
        return Page();
    }
}

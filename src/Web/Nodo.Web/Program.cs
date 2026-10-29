using System.Globalization;
using Microsoft.AspNetCore.Authentication.Cookies;
using Nodo.CalculoPrecioTotal;
using Nodo.Cotizaciones;
using Nodo.Cotizaciones.Domain.Events;
using Nodo.SharedKernel;
using Nodo.Tarifario;
using Nodo.Tarifario.Domain.Events;
using Nodo.Web.Infrastructure;

// Cultura fija: los importes de los formularios llegan con punto decimal en cualquier sistema operativo.
CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.InvariantCulture;

var builder = WebApplication.CreateBuilder(args);

var conn = builder.Configuration.GetConnectionString("Default") ?? "Data Source=nodo.db";

builder.Services.AddSharedKernel();
builder.Services.AddTarifarioModule(conn);
builder.Services.AddCalculoPrecioTotalModule();
builder.Services.AddCotizacionesModule(conn);
builder.Services.AddScoped<DomainEventToastNotifications>();
builder.Services.AddScoped<IDomainEventHandler<CotizacionEmitida>, CotizacionEmitidaToastHandler>();
builder.Services.AddScoped<IDomainEventHandler<CotizacionAceptada>, CotizacionAceptadaToastHandler>();
builder.Services.AddScoped<IDomainEventHandler<CotizacionCaducada>, CotizacionCaducadaToastHandler>();
builder.Services.AddScoped<IDomainEventHandler<TarifaActualizada>, TarifaActualizadaToastHandler>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Login";
        options.AccessDeniedPath = "/AccessDenied";
    });

builder.Services.AddAuthorization();

builder.Services.AddRazorPages();

var app = builder.Build();

await app.Services.MigrateTarifarioModuleAsync();
await app.Services.MigrateCotizacionesModuleAsync();

if (app.Environment.IsDevelopment())
{
    await app.Services.SeedTarifarioModuleAsync();
    await app.Services.SeedCotizacionesModuleAsync();
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapRazorPages();

app.Run();
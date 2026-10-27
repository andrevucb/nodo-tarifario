using Microsoft.AspNetCore.Authentication.Cookies;
using Nodo.CalculoPrecioTotal;
using Nodo.Cotizaciones;
using Nodo.SharedKernel;
using Nodo.Tarifario;

var builder = WebApplication.CreateBuilder(args);

var conn = builder.Configuration.GetConnectionString("Default") ?? "Data Source=nodo.db";

builder.Services.AddSharedKernel();
builder.Services.AddTarifarioModule(conn);
builder.Services.AddCalculoPrecioTotalModule();
builder.Services.AddCotizacionesModule(conn);

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

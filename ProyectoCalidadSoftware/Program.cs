using DinkToPdf;
using DinkToPdf.Contracts;
using ProyectoCalidadSoftware.Extension;
using ProyectoCalidadSoftware.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
 
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var context = new CustomAssemblyLoadContext();
context.LoadUnmanagedLibrary(Path.Combine(Directory.GetCurrentDirectory(), "LibreriaPDF/libwkhtmltox.dll"));
builder.Services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));

builder.Services.AddDbContext<AppDBContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("ConexionSQL"));
});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Acceso/Iniciar";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(25);
        options.AccessDeniedPath = "/Acceso/AccesoDenegado";
        
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Acceso}/{action=Iniciar}");

app.Run();

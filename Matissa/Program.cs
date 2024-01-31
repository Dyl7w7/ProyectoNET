using Matissa.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<dbMatissaNETContext>(options =>
       options.UseSqlServer(builder.Configuration.GetConnectionString("Connection")));

// Agrega los servicios de autenticación y configura la autenticación basada en cookies
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
  .AddCookie(options =>
  {
      options.LoginPath = "/Acceso/login";
      options.LogoutPath = "/Acceso/Logout";
  });

// Define la política de autorización "Configuracion"
//builder.Services.AddAuthorization(options =>
//{
//    options.AddPolicy("Configuracion", policy => policy.RequireClaim("Permisos", "Configuracion"));
//});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Agrega el middleware de autenticación
app.UseAuthentication();
app.UseAuthorization();


//app.MapControllerRoute(
//   name: "default",
//   pattern: "{controller=Home}/{action=Index}/{id?}");
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Acceso}/{action=Login}/{id?}");
});

app.Run();
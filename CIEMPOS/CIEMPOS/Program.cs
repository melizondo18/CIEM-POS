/*Configura los servicios, la inyección de dependencias,
 * la conexión a la base de datos y el pipeline de ejecución de la
 * aplicación CIEMPOS.
 */

using CIEMPOS.Data;
using Microsoft.EntityFrameworkCore;
using CIEMPOS.Repos;
using CIEMPOS.Services;

var builder = WebApplication.CreateBuilder(args);

// Agrega los servicios MVC
builder.Services.AddControllersWithViews();

// Habilita el uso de sesiones en la aplicación
builder.Services.AddSession(options =>
{
    // Tiempo máximo de inactividad antes de que expire la sesión
    options.IdleTimeout = TimeSpan.FromMinutes(30);

    // Impide el acceso a la cookie desde JavaScript
    options.Cookie.HttpOnly = true;

    // Indica que la cookie es esencial para el funcionamiento del sistema
    options.Cookie.IsEssential = true;
});

// Configura la conexión a la base de datos
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Registra los repositorios y servicios del sistema
builder.Services.AddScoped<IRolRepo, RolRepo>();
builder.Services.AddScoped<RolService>();

builder.Services.AddScoped<IPersonaRepo, PersonaRepo>();
builder.Services.AddScoped<PersonaService>();

builder.Services.AddScoped<IUsuarioRepo, UsuarioRepo>();
builder.Services.AddScoped<UsuarioService>();

builder.Services.AddScoped<LogInService>();

// Construye la aplicación
var app = builder.Build();

// Configura el manejo de errores en producción
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");

    // Habilita HSTS para conexiones seguras
    app.UseHsts();
}

// Configura el pipeline HTTP
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Habilita el uso de sesiones
app.UseSession();

// Habilita la autorización
app.UseAuthorization();

// Configura la ruta por defecto
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
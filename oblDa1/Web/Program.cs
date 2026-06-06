using Web.Components;
using Dominio.Entidades;
using IRepositorios;
using Repositorios;
using IServicios;
using Servicios;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<SqlContext>(options =>
    options
        .UseLazyLoadingProxies()
        .UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddScoped<IEquipoRepositorio, EquipoRepositorio>();
builder.Services.AddScoped<IEstadioRepositorio, EstadioRepositorio>();
builder.Services.AddScoped<IUsuarioRepositorio, UsuarioRepositorio>();
builder.Services.AddScoped<IPartidoRepositorio, PartidoRepositorio>();
builder.Services.AddScoped<IGrupoRepositorio, GrupoRepositorio>();
builder.Services.AddScoped<IAuditoriaRepositorio, AuditoriaRepositorio>();
builder.Services.AddScoped<IFixtureRepositorio, FixtureRepositorio>();
builder.Services.AddScoped<INotificacionRepositorio, NotificacionRepositorio>();


builder.Services.AddScoped<ITorneoServicio, TorneoServicio>();
builder.Services.AddScoped<IUsuarioServicio, UsuarioServicio>();
builder.Services.AddScoped<IAuditoriaServicio, AuditoriaServicio>();
builder.Services.AddScoped<ISesionServicio, SesionServicio>();
builder.Services.AddScoped<INotificacionServicio, NotificacionServicio>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<SqlContext>();
    context.Database.Migrate();

    var sesionServicio = scope.ServiceProvider.GetRequiredService<ISesionServicio>();
    var usuarioRepositorio = scope.ServiceProvider.GetRequiredService<IUsuarioRepositorio>();


    if (usuarioRepositorio.ObtenerTodos().All(u => u.Email != "admin@worldcup.com"))
    {
        var admin = new Usuario();
        admin.Nombre = "Admin";
        admin.Apellido = "Sistema";
        admin.Email = "admin@worldcup.com";
        admin.FechaNacimiento = new DateTime(1990, 1, 1);
        admin.Contrasena = "Admin@123";
        admin.Roles.Add(Rol.Administrador);
        admin.Roles.Add(Rol.Editor);
        usuarioRepositorio.Agregar(admin);
    }
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
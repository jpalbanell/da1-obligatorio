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

builder.Services.AddSingleton<IEquipoRepositorio, EquipoRepositorio>();
builder.Services.AddScoped<IEstadioRepositorio, EstadioRepositorio>();
builder.Services.AddSingleton<IUsuarioRepositorio, UsuarioRepositorio>();
builder.Services.AddSingleton<IPartidoRepositorio, PartidoRepositorio>();
builder.Services.AddSingleton<IGrupoRepositorio, GrupoRepositorio>();
builder.Services.AddSingleton<IAuditoriaRepositorio, AuditoriaRepositorio>();
builder.Services.AddSingleton<IFixtureRepositorio, FixtureRepositorio>();


builder.Services.AddScoped<ITorneoServicio, TorneoServicio>();
builder.Services.AddScoped<IUsuarioServicio, UsuarioServicio>();
builder.Services.AddScoped<IAuditoriaServicio, AuditoriaServicio>();
builder.Services.AddScoped<ISesionServicio, SesionServicio>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<SqlContext>();
    context.Database.Migrate();

    var sesionServicio = scope.ServiceProvider.GetRequiredService<ISesionServicio>();
    var usuarioRepositorio = scope.ServiceProvider.GetRequiredService<IUsuarioRepositorio>();


    var admin = new Usuario();
    admin.Id = 1;
    admin.Nombre = "Admin";
    admin.Apellido = "Sistema";
    admin.Email = "admin@worldcup.com";
    admin.FechaNacimiento = new DateTime(1990, 1, 1);
    admin.Contrasena = "Admin@123";
    admin.Roles.Add(Rol.Administrador);
    admin.Roles.Add(Rol.Editor);
    usuarioRepositorio.Agregar(admin);
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
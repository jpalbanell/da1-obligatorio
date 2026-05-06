using Web.Components;
using Dominio.Entidades;
using Repositorios;
using Servicios;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Repositorios
builder.Services.AddSingleton<IEquipoRepositorio, EquipoRepositorio>();
builder.Services.AddSingleton<IEstadioRepositorio, EstadioRepositorio>();
builder.Services.AddSingleton<IUsuarioRepositorio, UsuarioRepositorio>();
builder.Services.AddSingleton<IPartidoRepositorio, PartidoRepositorio>();
builder.Services.AddSingleton<IGrupoRepositorio, GrupoRepositorio>();
builder.Services.AddSingleton<IAuditoriaRepositorio, AuditoriaRepositorio>();
builder.Services.AddSingleton<IFixtureRepositorio, FixtureRepositorio>();

// Servicios
builder.Services.AddScoped<IEquipoServicio, EquipoServicio>();
builder.Services.AddScoped<IEstadioServicio, EstadioServicio>();
builder.Services.AddScoped<IUsuarioServicio, UsuarioServicio>();
builder.Services.AddScoped<IPartidoServicio, PartidoServicio>();
builder.Services.AddScoped<IAuditoriaServicio, AuditoriaServicio>();
builder.Services.AddScoped<ISimulacionServicio, SimulacionServicio>();
builder.Services.AddScoped<ISesionServicio, SesionServicio>();
builder.Services.AddScoped<IAutenticacionServicio, AutenticacionServicio>();
builder.Services.AddScoped<IFixtureServicio, FixtureServicio>();
builder.Services.AddScoped<ICruceServicio, CruceServicio>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
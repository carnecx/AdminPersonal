using AdminPersonal.Repository;
using AdminPersonal.Services;
using AdminPersonal.Services.Abstract;
using Dapper;
using MySqlConnector;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();

builder.Services.AddSingleton<IDbConnectionFactory, DbConnectionFactory>();

builder.Services.AddScoped<CompaniaRepository>();
builder.Services.AddScoped<ICompaniaService, CompaniaService>();

builder.Services.AddScoped<ParametroRepository>();
builder.Services.AddScoped<IParametroService, ParametroService>();

builder.Services.AddScoped<RolRepository>();
builder.Services.AddScoped<IRolService, RolService>();

builder.Services.AddScoped<RequisitoPuestoRepository>();
builder.Services.AddScoped<IRequisitoPuestoService, RequisitoPuestoService>();

builder.Services.AddScoped<PantallaRepository>();
builder.Services.AddScoped<IPantallaService, PantallaService>();

builder.Services.AddScoped<PasswordService>();

builder.Services.AddScoped<BitacoraRepository>();
builder.Services.AddScoped<BitacoraService>();

builder.Services.AddScoped<UsuarioRepository>();
builder.Services.AddScoped<UsuarioService>();

builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();

builder.Services.AddScoped<InstitucionRepository>();
builder.Services.AddScoped<InstitucionService>();

builder.Services.AddScoped<UbicacionRepository>();
builder.Services.AddScoped<UbicacionService>();

builder.Services.AddScoped<OferenteRepository>();
builder.Services.AddScoped<IOferenteService, OferenteService>();

builder.Services.AddScoped<ConcursoRepository>();
builder.Services.AddScoped<IConcursoService, ConcursoService>();

int minutosSesion = 5;

try
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

    using var conn = new MySqlConnection(connectionString);

    minutosSesion = await conn.QueryFirstOrDefaultAsync<int>(
        "SELECT valor FROM parametro WHERE codigo = 'SESION_TIMEOUT_MIN'");

    if (minutosSesion <= 0)
        minutosSesion = 5;
}
catch
{
    minutosSesion = 5;
}

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(minutosSesion);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseSession();
app.UseAuthorization();

app.MapGet("/", context =>
{
    context.Response.Redirect("/Account/Login");
    return Task.CompletedTask;
});

app.MapRazorPages();

app.Run();
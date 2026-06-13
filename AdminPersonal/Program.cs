using AdminPersonal.Repository;
using AdminPersonal.Services;
using AdminPersonal.Services.Abstract;

// crea el constructor principal de la aplicacion web
var builder = WebApplication.CreateBuilder(args);

// agrega soporte para razor pages
builder.Services.AddRazorPages();

builder.Services.AddSingleton<IDbConnectionFactory, DbConnectionFactory>();

// registra los repositorios y servicios de compania
builder.Services.AddScoped<CompaniaRepository>();
builder.Services.AddScoped<ICompaniaService, CompaniaService>();

// registra los repositorios y servicios de parametros
builder.Services.AddScoped<ParametroRepository>();
builder.Services.AddScoped<IParametroService, ParametroService>();

// registra los repositorios y servicios de roles
builder.Services.AddScoped<RolRepository>();
builder.Services.AddScoped<IRolService, RolService>();

// registra los repositorios y servicios de requisitos de puesto
builder.Services.AddScoped<RequisitoPuestoRepository>();
builder.Services.AddScoped<IRequisitoPuestoService, RequisitoPuestoService>();

// registra los repositorios y servicios de pantallas
builder.Services.AddScoped<PantallaRepository>();
builder.Services.AddScoped<IPantallaService, PantallaService>();

// registra el servicio para encriptar y validar contrasenas
builder.Services.AddScoped<PasswordService>();

// registra los repositorios y servicios de bitacora
builder.Services.AddScoped<BitacoraRepository>();
builder.Services.AddScoped<BitacoraService>();

// registra los repositorios y servicios de usuarios
builder.Services.AddScoped<UsuarioRepository>();
builder.Services.AddScoped<UsuarioService>();

// registra usuarios tambien mediante interfaces
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();

// registra los repositorios y servicios de instituciones educativas
builder.Services.AddScoped<InstitucionRepository>();
builder.Services.AddScoped<InstitucionService>();

// registra los repositorios y servicios de ubicaciones
builder.Services.AddScoped<UbicacionRepository>();
builder.Services.AddScoped<UbicacionService>();

// registra los repositorios y servicios de oferentes
builder.Services.AddScoped<OferenteRepository>();
builder.Services.AddScoped<IOferenteService, OferenteService>();

// registra los repositorios y servicios de concursos
builder.Services.AddScoped<ConcursoRepository>();
builder.Services.AddScoped<IConcursoService, ConcursoService>();

// configura la sesion para que venza despues de 5 minutos de inactividad
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(5);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// construye la aplicacion
var app = builder.Build();

// configura el manejo de errores cuando no se esta en ambiente de desarrollo
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

// redirige peticiones http a https
app.UseHttpsRedirection();

// permite usar archivos estaticos como css, js e imagenes
app.UseStaticFiles();

// habilita el enrutamiento
app.UseRouting();

// habilita el uso de sesion
app.UseSession();

// habilita autorizacion
app.UseAuthorization();

// redirige la raiz del sitio hacia la pantalla de login
app.MapGet("/", context =>
{
    context.Response.Redirect("/Account/Login");
    return Task.CompletedTask;
});

// mapea las paginas razor del proyecto
app.MapRazorPages();

// ejecuta la aplicacion
app.Run();
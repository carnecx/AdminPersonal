using AdminPersonal.Repository;
using AdminPersonal.Services;
using AdminPersonal.Services.Abstract;

var builder = WebApplication.CreateBuilder(args);

// Registro de dependencias para Usuario
builder.Services.AddScoped<AdminPersonal.Repository.IUsuarioRepository,
                            AdminPersonal.Repository.UsuarioRepository>();
builder.Services.AddScoped<AdminPersonal.Services.IUsuarioService,
                            AdminPersonal.Services.UsuarioService>();

// Add services to the container.
builder.Services.AddRazorPages();
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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
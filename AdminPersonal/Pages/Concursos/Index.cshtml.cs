using AdminPersonal.Entities;
using AdminPersonal.Services;
using AdminPersonal.Services.Abstract;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AdminPersonal.Pages.Concursos
{
    public class IndexModel : PageModel
    {
        private readonly IConcursoService _concursoService;
        private readonly BitacoraService _bitacoraService;

        public IndexModel(IConcursoService concursoService, BitacoraService bitacoraService)
        {
            _concursoService = concursoService;
            _bitacoraService = bitacoraService;
            Concursos = new List<Concurso>();
        }

        public IEnumerable<Concurso> Concursos { get; set; }

        public async Task OnGetAsync()
        {
            Concursos = await _concursoService.ObtenerTodosAsync();
            var idUsuario = HttpContext.Session.GetInt32("IdUsuario") ?? 0;
            await _bitacoraService.RegistrarAsync(idUsuario, "El usuario consulta Concursos");
        }
    }
}
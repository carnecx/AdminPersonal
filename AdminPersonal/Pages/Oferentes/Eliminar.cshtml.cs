using AdminPersonal.Entities;
using AdminPersonal.Services.Abstract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AdminPersonal.Pages.Oferentes
{
    public class EliminarModel : PageModel
    {
        private readonly IOferenteService _service;
        public Oferente? Oferente { get; set; }
        public string Error { get; set; } = string.Empty;

        [BindProperty] public int IdOferente { get; set; }

        public EliminarModel(IOferenteService service)
        {
            _service = service;
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Oferente = await _service.ObtenerPorIdAsync(id);
            if (Oferente == null) return RedirectToPage("Index");
            IdOferente = id; 
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (await _service.TieneRelacionesAsync(IdOferente))
            {
                Error = "No se puede eliminar un registro con datos relacionados.";
                Oferente = await _service.ObtenerPorIdAsync(IdOferente);
                return Page();
            }

            await _service.EliminarAsync(IdOferente);
            TempData["Mensaje"] = "El oferente ha sido eliminado correctamente.";
            return RedirectToPage("Index");
        }
    }
}
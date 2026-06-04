using AdminPersonal.Services.Abstract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AdminPersonal.Pages.Rol
{
    public class EliminarModel : PageModel
    {
        private readonly IRolService _rolService;

        public EliminarModel(IRolService rolService)
        {
            _rolService = rolService;
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            // Verificar si el rol está asignado a algún usuario
            if (await _rolService.EstaAsignadoAUsuarioAsync(id))
            {
                TempData["Error"] = "No se puede eliminar un registro con datos relacionados.";
                return RedirectToPage("Index");
            }

            await _rolService.EliminarAsync(id);
            TempData["Mensaje"] = "Rol eliminado exitosamente.";
            return RedirectToPage("Index");
        }
    }
}

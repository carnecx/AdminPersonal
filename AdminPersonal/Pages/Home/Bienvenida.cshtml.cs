using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AdminPersonal.Pages.Home
{
    // modelo de la pagina de bienvenida
    public class BienvenidaModel : PageModel
    {
        // se ejecuta cuando el usuario abre la pagina
        public IActionResult OnGet()
        {
            // verifica si existe una sesion activa
            if (HttpContext.Session.GetInt32("IdUsuario") == null)

                // si no existe sesion redirige al login
                return RedirectToPage(
                    "/Account/Login",
                    new
                    {
                        mensaje = "Por favor inicie sesión para utilizar el sistema"
                    });

            // si existe sesion muestra la pagina de bienvenida
            return Page();
        }
    }
}
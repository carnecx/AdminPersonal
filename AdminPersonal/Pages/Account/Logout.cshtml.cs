using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AdminPersonal.Pages.Account;

public class LogoutModel : PageModel
{
    public IActionResult OnGet(string? mensaje)
    {
        HttpContext.Session.Clear();

        return RedirectToPage("/Account/Login", new
        {
            mensaje = mensaje ?? "Sesion cerrada correctamente."
        });
    }
}
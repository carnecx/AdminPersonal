using AdminPersonal.Entities;
using AdminPersonal.Services.Abstract;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AdminPersonal.Pages.Compania
{
    public class IndexModel : PageModel
    {
        private readonly ICompaniaService _companiaService;

        public IndexModel(ICompaniaService companiaService)
        {
            _companiaService = companiaService;
            Companias = new List<AdminPersonal.Entities.Compania>();
        }

        public IEnumerable<AdminPersonal.Entities.Compania> Companias { get; set; }

        public async Task OnGetAsync()
        {
            Companias = await _companiaService.ObtenerTodosAsync();
        }
    }
}
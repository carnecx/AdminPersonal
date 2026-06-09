using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using AdminPersonal.Entities;
using AdminPersonal.Repository;

namespace AdminPersonal.Pages.AccionesPersonal
{
    public class IndexModel : PageModel
    {
        public List<AccionPersonal> AccionesList { get; set; } = new List<AccionPersonal>();
        public int PageIndex { get; set; } = 1;
        public int TotalPages { get; set; }

        public void OnGet(int p = 1)
        {
            PageIndex = p < 1 ? 1 : p;
            var allAcciones = MemoryAccionPersonalRepository.GetAll();
            int pageSize = 10;
            TotalPages = (int)System.Math.Ceiling(allAcciones.Count / (double)pageSize);
            if (TotalPages == 0) TotalPages = 1;
            AccionesList = allAcciones.Skip((PageIndex - 1) * pageSize).Take(pageSize).ToList();
        }
    }
}

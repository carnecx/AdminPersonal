using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using AdminPersonal.Entities;
using AdminPersonal.Repository;

namespace AdminPersonal.Pages.Puestos
{
    public class IndexModel : PageModel
    {
        public List<Puesto> PuestosList { get; set; } = new List<Puesto>();
        public int PageIndex { get; set; } = 1;
        public int TotalPages { get; set; }

        public void OnGet(int p = 1)
        {
            PageIndex = p < 1 ? 1 : p;
            var allPuestos = MemoryPuestoRepository.GetAll();
            int pageSize = 10;
            TotalPages = (int)System.Math.Ceiling(allPuestos.Count / (double)pageSize);
            if (TotalPages == 0) TotalPages = 1;
            PuestosList = allPuestos.Skip((PageIndex - 1) * pageSize).Take(pageSize).ToList();
        }
    }
}

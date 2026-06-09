using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using AdminPersonal.Entities;
using AdminPersonal.Repository;

namespace AdminPersonal.Pages.Entrevistas
{
    public class IndexModel : PageModel
    {
        public List<Entrevista> EntrevistasList { get; set; } = new List<Entrevista>();
        public int PageIndex { get; set; } = 1;
        public int TotalPages { get; set; }

        public void OnGet(int p = 1)
        {
            PageIndex = p < 1 ? 1 : p;
            var allEntrevistas = MemoryEntrevistaRepository.GetAll();
            int pageSize = 10;
            TotalPages = (int)System.Math.Ceiling(allEntrevistas.Count / (double)pageSize);
            if (TotalPages == 0) TotalPages = 1;
            EntrevistasList = allEntrevistas.Skip((PageIndex - 1) * pageSize).Take(pageSize).ToList();
        }

        public IActionResult OnGetCompletar(int id)
        {
            MemoryEntrevistaRepository.MarcarComoRealizada(id);
            TempData["SuccessMessage"] = "Entrevista marcada como realizada";
            return RedirectToPage("Index");
        }
    }
}

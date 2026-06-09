using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using AdminPersonal.Entities;
using AdminPersonal.Repository;
using Dapper;

namespace AdminPersonal.Pages.ExperienciaLaboral
{
    public class IndexModel : PageModel
    {
        public List<AdminPersonal.Entities.ExperienciaLaboral> ExperienciasList { get; set; } = new List<AdminPersonal.Entities.ExperienciaLaboral>();
        public int PageIndex { get; set; } = 1;
        public int TotalPages { get; set; }

        public int OferenteId { get; set; }
        public string NombreOferente { get; set; } = "";

        public void OnGet(int oferenteId, int p = 1)
        {
            OferenteId = oferenteId;
            using (var connection = ConnectionProvider.GetConnection())
            {
                NombreOferente = connection.QueryFirstOrDefault<string>(
                    "SELECT nombre_completo FROM oferente WHERE id_oferente = @Id", 
                    new { Id = oferenteId }) ?? $"Oferente #{oferenteId}";
            }
            PageIndex = p < 1 ? 1 : p;
            var allExperiencias = MemoryExperienciaLaboralRepository.GetByOferenteId(oferenteId);
            int pageSize = 10;
            TotalPages = (int)System.Math.Ceiling(allExperiencias.Count / (double)pageSize);
            if (TotalPages == 0) TotalPages = 1;
            ExperienciasList = allExperiencias.Skip((PageIndex - 1) * pageSize).Take(pageSize).ToList();
        }
    }
}

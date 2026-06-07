using Microsoft.AspNetCore.Mvc.RazorPages;
using MySqlConnector;
using System.Collections.Generic;

namespace AdminPersonal.Pages.Areas
{
    public class AreaListItem
    {
        public int IdArea { get; set; }
        public string Codigo { get; set; } = "";
        public string Nombre { get; set; } = "";
        public string NombreJefatura { get; set; } = "";
    }

    public class IndexModel : PageModel
    {
        private readonly IConfiguration _config;
        public List<AreaListItem> Areas { get; set; } = new();
        public string MensajeExito { get; set; } = "";
        public string MensajeError { get; set; } = "";

        public IndexModel(IConfiguration config)
        {
            _config = config;
        }

        public void OnGet()
        {
            if (TempData.ContainsKey("MensajeExito"))
                MensajeExito = TempData["MensajeExito"]?.ToString() ?? "";
            if (TempData.ContainsKey("MensajeError"))
                MensajeError = TempData["MensajeError"]?.ToString() ?? "";

            string conn = _config.GetConnectionString("DefaultConnection")!;
            using var con = new MySqlConnection(conn);
            con.Open();

            string sql = @"
                SELECT a.id_area, a.codigo, a.nombre,
                       CONCAT(o.nombre_completo) AS nombre_jefatura
                FROM area a
                LEFT JOIN empleado e ON e.id_empleado = a.id_jefatura
                LEFT JOIN oferente o ON o.id_oferente = e.id_oferente";

            using var cmd = new MySqlCommand(sql, con);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Areas.Add(new AreaListItem
                {
                    IdArea = reader.GetInt32("id_area"),
                    Codigo = reader.GetString("codigo"),
                    Nombre = reader.GetString("nombre"),
                    NombreJefatura = reader.IsDBNull(reader.GetOrdinal("nombre_jefatura"))
                        ? "Sin jefatura" : reader.GetString("nombre_jefatura")
                });
            }

            var idUsuario = HttpContext.Session.GetInt32("IdUsuario");
            if (idUsuario != null)
            {
                con.Close();
                con.Open();
                string sqlBit = "INSERT INTO bitacora (id_usuario, descripcion) VALUES (@u, @d)";
                using var cmdBit = new MySqlCommand(sqlBit, con);
                cmdBit.Parameters.AddWithValue("@u", idUsuario);
                cmdBit.Parameters.AddWithValue("@d", "El usuario consulta Áreas");
                cmdBit.ExecuteNonQuery();
            }
        }
    }
}
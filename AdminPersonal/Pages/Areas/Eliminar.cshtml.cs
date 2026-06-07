using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySqlConnector;
using System.Text.Json;

namespace AdminPersonal.Pages.Areas
{
    public class EliminarModel : PageModel
    {
        private readonly IConfiguration _config;
        public string MensajeError { get; set; } = "";
        public int IdArea { get; set; }
        public string Codigo { get; set; } = "";
        public string Nombre { get; set; } = "";

        public EliminarModel(IConfiguration config) => _config = config;

        public void OnGet(int id)
        {
            string conn = _config.GetConnectionString("DefaultConnection")!;
            using var con = new MySqlConnection(conn);
            con.Open();
            string sql = "SELECT id_area, codigo, nombre FROM area WHERE id_area = @id";
            using var cmd = new MySqlCommand(sql, con);
            cmd.Parameters.AddWithValue("@id", id);
            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                IdArea = reader.GetInt32("id_area");
                Codigo = reader.GetString("codigo");
                Nombre = reader.GetString("nombre");
            }
        }

        public IActionResult OnPost(int IdArea)
        {
            string conn = _config.GetConnectionString("DefaultConnection")!;
            using var con = new MySqlConnection(conn);
            con.Open();

            // Guardar datos para bitácora antes de eliminar
            string datosEliminados = "";
            string sqlOld = "SELECT codigo, nombre, id_jefatura FROM area WHERE id_area = @id";
            using (var cmdOld = new MySqlCommand(sqlOld, con))
            {
                cmdOld.Parameters.AddWithValue("@id", IdArea);
                using var r = cmdOld.ExecuteReader();
                if (r.Read())
                {
                    Codigo = r.GetString("codigo");
                    Nombre = r.GetString("nombre");
                    datosEliminados = JsonSerializer.Serialize(new
                    {
                        Codigo,
                        Nombre,
                        IdJefatura = r.IsDBNull(r.GetOrdinal("id_jefatura")) ? 0 : r.GetInt32("id_jefatura")
                    });
                }
            }

            this.IdArea = IdArea;

            try
            {
                string sql = "DELETE FROM area WHERE id_area = @id";
                using var cmd = new MySqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@id", IdArea);
                cmd.ExecuteNonQuery();

                // Bitácora
                var idUsuario = HttpContext.Session.GetInt32("IdUsuario");
                if (idUsuario != null)
                {
                    string sqlBit = "INSERT INTO bitacora (id_usuario, descripcion) VALUES (@u, @d)";
                    using var cmdBit = new MySqlCommand(sqlBit, con);
                    cmdBit.Parameters.AddWithValue("@u", idUsuario);
                    cmdBit.Parameters.AddWithValue("@d", $"Eliminación de Área: {datosEliminados}");
                    cmdBit.ExecuteNonQuery();
                }

                TempData["MensajeExito"] = "Área eliminada correctamente.";
                return RedirectToPage("/Areas/Index");
            }
            catch (MySqlException ex) when (ex.Number == 1451)
            {
                MensajeError = "No se puede eliminar un registro con datos relacionados.";
                return Page();
            }
        }
    }
}
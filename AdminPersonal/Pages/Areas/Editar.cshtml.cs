using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySqlConnector;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace AdminPersonal.Pages.Areas
{
    public class EditarModel : PageModel
    {
        private readonly IConfiguration _config;
        public List<EmpleadoItem> Empleados { get; set; } = new();
        public string MensajeError { get; set; } = "";
        public int IdArea { get; set; }
        public string Codigo { get; set; } = "";
        public string Nombre { get; set; } = "";
        public int IdJefatura { get; set; }

        public EditarModel(IConfiguration config) => _config = config;

        private void CargarEmpleados(MySqlConnection con)
        {
            string sql = @"SELECT e.id_empleado, o.nombre_completo AS nombre
                           FROM empleado e
                           INNER JOIN oferente o ON o.id_oferente = e.id_oferente";
            using var cmd = new MySqlCommand(sql, con);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
                Empleados.Add(new EmpleadoItem
                {
                    IdEmpleado = reader.GetInt32("id_empleado"),
                    Nombre = reader.GetString("nombre")
                });
        }

        public void OnGet(int id)
        {
            string conn = _config.GetConnectionString("DefaultConnection")!;
            using var con = new MySqlConnection(conn);
            con.Open();

            string sql = "SELECT id_area, codigo, nombre, id_jefatura FROM area WHERE id_area = @id";
            using var cmd = new MySqlCommand(sql, con);
            cmd.Parameters.AddWithValue("@id", id);
            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                IdArea = reader.GetInt32("id_area");
                Codigo = reader.GetString("codigo");
                Nombre = reader.GetString("nombre");
                IdJefatura = reader.IsDBNull(reader.GetOrdinal("id_jefatura"))
                    ? 0 : reader.GetInt32("id_jefatura");
            }
            reader.Close();
            CargarEmpleados(con);
        }

        public IActionResult OnPost(int IdArea, string Codigo, string Nombre, int IdJefatura)
        {
            string conn = _config.GetConnectionString("DefaultConnection")!;
            using var con = new MySqlConnection(conn);
            con.Open();

            if (!Regex.IsMatch(Nombre, @"^[A-Za-z·ÈÌÛ˙¡…Õ”⁄Ò— ]+$"))
            {
                MensajeError = "El nombre solo debe contener letras y espacios.";
                this.IdArea = IdArea;
                this.Codigo = Codigo;
                this.Nombre = Nombre;
                this.IdJefatura = IdJefatura;
                CargarEmpleados(con);
                return Page();
            }

            string anterior = "";
            string sqlOld = "SELECT codigo, nombre, id_jefatura FROM area WHERE id_area = @id";
            using (var cmdOld = new MySqlCommand(sqlOld, con))
            {
                cmdOld.Parameters.AddWithValue("@id", IdArea);
                using var r = cmdOld.ExecuteReader();
                if (r.Read())
                    anterior = JsonSerializer.Serialize(new
                    {
                        Codigo = r.GetString("codigo"),
                        Nombre = r.GetString("nombre"),
                        IdJefatura = r.IsDBNull(r.GetOrdinal("id_jefatura")) ? 0 : r.GetInt32("id_jefatura")
                    });
            }

            string sql = "UPDATE area SET codigo=@c, nombre=@n, id_jefatura=@j WHERE id_area=@id";
            using var cmd = new MySqlCommand(sql, con);
            cmd.Parameters.AddWithValue("@c", Codigo);
            cmd.Parameters.AddWithValue("@n", Nombre);
            cmd.Parameters.AddWithValue("@j", IdJefatura);
            cmd.Parameters.AddWithValue("@id", IdArea);
            cmd.ExecuteNonQuery();

            var idUsuario = HttpContext.Session.GetInt32("IdUsuario");
            if (idUsuario != null)
            {
                var nuevo = JsonSerializer.Serialize(new { Codigo, Nombre, IdJefatura });
                string sqlBit = "INSERT INTO bitacora (id_usuario, descripcion) VALUES (@u, @d)";
                using var cmdBit = new MySqlCommand(sqlBit, con);
                cmdBit.Parameters.AddWithValue("@u", idUsuario);
                cmdBit.Parameters.AddWithValue("@d", $"ActualizaciÛn de ¡rea - Anterior: {anterior} | Nuevo: {nuevo}");
                cmdBit.ExecuteNonQuery();
            }

            TempData["MensajeExito"] = "¡rea actualizada correctamente.";
            return RedirectToPage("/Areas/Index");
        }
    }
}
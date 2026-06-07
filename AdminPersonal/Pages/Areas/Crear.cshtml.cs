using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySqlConnector;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace AdminPersonal.Pages.Areas
{
    public class EmpleadoItem
    {
        public int IdEmpleado { get; set; }
        public string Nombre { get; set; } = "";
    }

    public class CrearModel : PageModel
    {
        private readonly IConfiguration _config;
        public List<EmpleadoItem> Empleados { get; set; } = new();
        public string MensajeError { get; set; } = "";
        public string Codigo { get; set; } = "";
        public string Nombre { get; set; } = "";

        public CrearModel(IConfiguration config) => _config = config;

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

        public void OnGet()
        {
            string conn = _config.GetConnectionString("DefaultConnection")!;
            using var con = new MySqlConnection(conn);
            con.Open();
            CargarEmpleados(con);
        }

        public IActionResult OnPost(string Codigo, string Nombre, int IdJefatura)
        {
            string conn = _config.GetConnectionString("DefaultConnection")!;
            using var con = new MySqlConnection(conn);
            con.Open();

            if (!Regex.IsMatch(Nombre, @"^[A-Za-z·ÈÌÛ˙¡…Õ”⁄Ò— ]+$"))
            {
                MensajeError = "El nombre solo debe contener letras y espacios.";
                this.Codigo = Codigo;
                this.Nombre = Nombre;
                CargarEmpleados(con);
                return Page();
            }

            try
            {
                string sql = "INSERT INTO area (codigo, nombre, id_jefatura) VALUES (@c, @n, @j)";
                using var cmd = new MySqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@c", Codigo);
                cmd.Parameters.AddWithValue("@n", Nombre);
                cmd.Parameters.AddWithValue("@j", IdJefatura);
                cmd.ExecuteNonQuery();

                var idUsuario = HttpContext.Session.GetInt32("IdUsuario");
                if (idUsuario != null)
                {
                    var json = JsonSerializer.Serialize(new { Codigo, Nombre, IdJefatura });
                    string sqlBit = "INSERT INTO bitacora (id_usuario, descripcion) VALUES (@u, @d)";
                    using var cmdBit = new MySqlCommand(sqlBit, con);
                    cmdBit.Parameters.AddWithValue("@u", idUsuario);
                    cmdBit.Parameters.AddWithValue("@d", $"CreaciÛn de ¡rea: {json}");
                    cmdBit.ExecuteNonQuery();
                }

                TempData["MensajeExito"] = "¡rea creada correctamente.";
                return RedirectToPage("/Areas/Index");
            }
            catch (MySqlException ex) when (ex.Number == 1062)
            {
                MensajeError = "El cÛdigo del ·rea ya existe.";
                this.Codigo = Codigo;
                this.Nombre = Nombre;
                CargarEmpleados(con);
                return Page();
            }
        }
    }
}
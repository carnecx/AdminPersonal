using AdminPersonal.Entities;
using Dapper;
using Microsoft.Extensions.Configuration;
using MySqlConnector;

namespace AdminPersonal.Repository
{
    public class RequisitoPuestoRepository
    {
        private readonly string _connectionString;

        public RequisitoPuestoRepository(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("DefaultConnection")!;
        }

        private MySqlConnection AbrirConexion() => new MySqlConnection(_connectionString);

        // Obtener todos los requisitos con nombre del puesto
        public IEnumerable<RequisitoPuesto> ObtenerTodos()
        {
            using var conexion = AbrirConexion();
            return conexion.Query<RequisitoPuesto>(@"SELECT rp.id_requisito,rp.nombre_requisito,rp.id_puesto,p.nombre AS nombre_puesto
                FROM requisito_puesto rp
                INNER JOIN puesto p ON rp.id_puesto = p.id_puesto").ToList();
        }

        // Obtener un requisito por ID
        public RequisitoPuesto? ObtenerPorId(int id)
        {
            using var conexion = AbrirConexion();
            return conexion.QueryFirstOrDefault<RequisitoPuesto>("SELECT id_requisito, nombre_requisito, id_puesto FROM requisito_puesto WHERE id_requisito = @Id", new { Id = id });
        }

        // Obtener lista de puestos para dropdowns
        public IEnumerable<dynamic> ObtenerPuestos()
        {
            using var conexion = AbrirConexion();
            return conexion.Query<dynamic>("SELECT id_puesto, nombre FROM puesto").ToList();
        }

        // Verificar si un puesto existe
        public dynamic? VerificarPuesto(int id_puesto)
        {
            using var conexion = AbrirConexion();
            return conexion.QueryFirstOrDefault<dynamic>("SELECT id_puesto FROM puesto WHERE id_puesto = @id_puesto", new { id_puesto });
        }

        // Verificar si ya existe el mismo requisito en el mismo puesto (para Crear)
        public RequisitoPuesto? BuscarDuplicado(string nombre_requisito, int id_puesto)
        {
            using var conexion = AbrirConexion();
            return conexion.QueryFirstOrDefault<RequisitoPuesto>("SELECT id_requisito FROM requisito_puesto WHERE nombre_requisito = @nombre_requisito AND id_puesto = @id_puesto", new { nombre_requisito, id_puesto });
        }

        // Verificar duplicado al Editar (excluye el registro actual)
        public RequisitoPuesto? BuscarDuplicadoEditar(string nombre_requisito, int id_puesto, int id_requisito)
        {
            using var conexion = AbrirConexion();
            return conexion.QueryFirstOrDefault<RequisitoPuesto>("SELECT id_requisito FROM requisito_puesto WHERE nombre_requisito = @nombre_requisito AND id_puesto = @id_puesto AND id_requisito != @id_requisito", new { nombre_requisito, id_puesto, id_requisito });
        }

        // Crear un nuevo requisito
        public void Crear(RequisitoPuesto requisito)
        {
            using var conexion = AbrirConexion();
            conexion.Execute("INSERT INTO requisito_puesto (nombre_requisito, id_puesto) VALUES (@nombre_requisito, @id_puesto)", requisito);
        }

        // Actualizar un requisito existente
        public void Actualizar(RequisitoPuesto requisito)
        {
            using var conexion = AbrirConexion();
            conexion.Execute("UPDATE requisito_puesto SET nombre_requisito = @nombre_requisito, id_puesto = @id_puesto WHERE id_requisito = @id_requisito", requisito);
        }

        // Eliminar un requisito
        public void Eliminar(int id)
        {
            using var conexion = AbrirConexion();
            conexion.Execute("DELETE FROM requisito_puesto WHERE id_requisito = @Id", new { Id = id });
        }
    }
}
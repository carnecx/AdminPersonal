using AdminPersonal.Entities;
using Dapper;
using Microsoft.Extensions.Configuration;
using MySqlConnector;

namespace AdminPersonal.Repository
{
    public class PantallaRepository
    {
        private readonly string _connectionString;

        public PantallaRepository(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("DefaultConnection")!;
        }

        private MySqlConnection AbrirConexion() => new MySqlConnection(_connectionString);

        public IEnumerable<Pantalla> ObtenerTodos()
        {
            using var conexion = AbrirConexion();
            return conexion.Query<Pantalla>("SELECT id_pantalla, nombre_pantalla FROM pantalla").ToList();
        }

        public Pantalla? ObtenerPorId(int id)
        {
            using var conexion = AbrirConexion();
            return conexion.QueryFirstOrDefault<Pantalla>("SELECT id_pantalla, nombre_pantalla FROM pantalla WHERE id_pantalla = @Id",new { Id = id });
        }

        public Pantalla? BuscarDuplicado(string nombre_pantalla)
        {
            using var conexion = AbrirConexion();
            return conexion.QueryFirstOrDefault<Pantalla>("SELECT id_pantalla FROM pantalla WHERE nombre_pantalla = @nombre_pantalla",new { nombre_pantalla });
        }

        public Pantalla? BuscarDuplicadoEditar(string nombre_pantalla, int id_pantalla)
        {
            using var conexion = AbrirConexion();
            return conexion.QueryFirstOrDefault<Pantalla>("SELECT id_pantalla FROM pantalla WHERE nombre_pantalla = @nombre_pantalla AND id_pantalla != @id_pantalla",new { nombre_pantalla, id_pantalla });
        }

        public bool EstaAsignada(int id)
        {
            using var conexion = AbrirConexion();
            return conexion.QueryFirstOrDefault<dynamic>("SELECT id_pantalla FROM rol_pantalla WHERE id_pantalla = @Id",new { Id = id }) != null;
        }

        public void Crear(Pantalla pantalla)
        {
            using var conexion = AbrirConexion();
            conexion.Execute("INSERT INTO pantalla (nombre_pantalla) VALUES (@nombre_pantalla)",pantalla);
        }

        public void Actualizar(Pantalla pantalla)
        {
            using var conexion = AbrirConexion();
            conexion.Execute("UPDATE pantalla SET nombre_pantalla = @nombre_pantalla WHERE id_pantalla = @id_pantalla",pantalla);
        }

        public void Eliminar(int id)
        {
            using var conexion = AbrirConexion();
            conexion.Execute("DELETE FROM pantalla WHERE id_pantalla = @Id", new { Id = id });
        }
    }
}

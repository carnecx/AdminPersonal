using AdminPersonal.Entities;
using Dapper;

namespace AdminPersonal.Repository
{
    public class PantallaRepository
    {
        private readonly IDbConnectionFactory _dbFactory;

        public PantallaRepository(IDbConnectionFactory dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public IEnumerable<Pantalla> ObtenerTodos()
        {
            using var conexion = _dbFactory.CrearConexion();
            return conexion.Query<Pantalla>("SELECT id_pantalla, nombre_pantalla FROM pantalla").ToList();
        }

        public Pantalla? ObtenerPorId(int id)
        {
            using var conexion = _dbFactory.CrearConexion();
            return conexion.QueryFirstOrDefault<Pantalla>("SELECT id_pantalla, nombre_pantalla FROM pantalla WHERE id_pantalla = @Id", new { Id = id });
        }

        public int Crear(Pantalla pantalla)
        {
            using var conexion = _dbFactory.CrearConexion();
            return conexion.ExecuteScalar<int>("INSERT INTO pantalla (nombre_pantalla) VALUES (@nombre_pantalla); SELECT LAST_INSERT_ID();",pantalla);
        }

        public void Actualizar(Pantalla pantalla)
        {
            using var conexion = _dbFactory.CrearConexion();
            conexion.Execute("UPDATE pantalla SET nombre_pantalla = @nombre_pantalla WHERE id_pantalla = @id_pantalla", pantalla);
        }

        public void Eliminar(int id)
        {
            using var conexion = _dbFactory.CrearConexion();
            conexion.Execute("DELETE FROM pantalla WHERE id_pantalla = @Id", new { Id = id });
        }

        public Pantalla? BuscarDuplicado(string nombre)
        {
            using var conexion = _dbFactory.CrearConexion();
            return conexion.QueryFirstOrDefault<Pantalla>("SELECT id_pantalla FROM pantalla WHERE nombre_pantalla = @Nombre", new { Nombre = nombre });
        }

        public Pantalla? BuscarDuplicadoEditar(string nombre, int idExcluir)
        {
            using var conexion = _dbFactory.CrearConexion();
            return conexion.QueryFirstOrDefault<Pantalla>("SELECT id_pantalla FROM pantalla WHERE nombre_pantalla = @Nombre AND id_pantalla != @IdExcluir", new { Nombre = nombre, IdExcluir = idExcluir });
        }

        public bool EstaAsignada(int id)
        {
            using var conexion = _dbFactory.CrearConexion();
            return conexion.QueryFirstOrDefault<dynamic>("SELECT id_pantalla FROM rol_pantalla WHERE id_pantalla = @Id", new { Id = id }) != null;
        }

        public IEnumerable<RolPantalla> ObtenerRolesConAsignacion(int id_pantalla)
        {
            using var conexion = _dbFactory.CrearConexion();
            return conexion.Query<RolPantalla>(@"
                SELECT r.id_rol, r.nombre_rol,
                CASE WHEN rp.id_pantalla IS NOT NULL THEN 1 ELSE 0 END AS Seleccionado
                FROM rol r
                LEFT JOIN rol_pantalla rp ON r.id_rol = rp.id_rol AND rp.id_pantalla = @id_pantalla",
                new { id_pantalla }).ToList();
        }

        public void EliminarAsignaciones(int id_pantalla)
        {
            using var conexion = _dbFactory.CrearConexion();
            conexion.Execute("DELETE FROM rol_pantalla WHERE id_pantalla = @id_pantalla", new { id_pantalla });
        }

        public void AsignarRoles(int id_pantalla, List<int> rolesSeleccionados)
        {
            using var conexion = _dbFactory.CrearConexion();
            foreach (var id_rol in rolesSeleccionados)
                conexion.Execute("INSERT INTO rol_pantalla (id_rol, id_pantalla) VALUES (@id_rol, @id_pantalla)", new { id_rol, id_pantalla });
        }

        public IEnumerable<string> ObtenerNombresPorRol(int id_rol)
        {
            using var conexion = _dbFactory.CrearConexion();
            return conexion.Query<string>(@"SELECT p.nombre_pantallaFROM pantalla p
                INNER JOIN rol_pantalla rp ON p.id_pantalla = rp.id_pantalla
                WHERE rp.id_rol = @id_rol",
                new { id_rol }).ToList();
        }
    }
}

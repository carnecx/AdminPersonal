using AdminPersonal.Entities;
using Dapper;

namespace AdminPersonal.Repository
{
    public class RequisitoPuestoRepository
    {
        private readonly IDbConnectionFactory _dbFactory;

        public RequisitoPuestoRepository(IDbConnectionFactory dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public IEnumerable<RequisitoPuesto> ObtenerTodos()
        {
            using var conexion = _dbFactory.CrearConexion();
            return conexion.Query<RequisitoPuesto>(@"
                SELECT rp.id_requisito, rp.nombre_requisito, rp.id_puesto, p.nombre AS nombre_puesto
                FROM requisito_puesto rp
                INNER JOIN puesto p ON rp.id_puesto = p.id_puesto").ToList();
        }

        public RequisitoPuesto? ObtenerPorId(int id)
        {
            using var conexion = _dbFactory.CrearConexion();
            return conexion.QueryFirstOrDefault<RequisitoPuesto>(
                "SELECT id_requisito, nombre_requisito, id_puesto FROM requisito_puesto WHERE id_requisito = @Id",
                new { Id = id });
        }

        public IEnumerable<dynamic> ObtenerPuestos()
        {
            using var conexion = _dbFactory.CrearConexion();
            return conexion.Query<dynamic>("SELECT id_puesto, nombre FROM puesto").ToList();
        }
        public RequisitoPuesto? BuscarDuplicado(string nombre_requisito, int id_puesto)
        {
            using var conexion = _dbFactory.CrearConexion();
            return conexion.QueryFirstOrDefault<RequisitoPuesto>(
                "SELECT id_requisito FROM requisito_puesto WHERE nombre_requisito = @nombre_requisito AND id_puesto = @id_puesto",
                new { nombre_requisito, id_puesto });
        }

        public RequisitoPuesto? BuscarDuplicadoEditar(string nombre_requisito, int id_puesto, int id_requisito)
        {
            using var conexion = _dbFactory.CrearConexion();
            return conexion.QueryFirstOrDefault<RequisitoPuesto>(
                "SELECT id_requisito FROM requisito_puesto WHERE nombre_requisito = @nombre_requisito AND id_puesto = @id_puesto AND id_requisito != @id_requisito",
                new { nombre_requisito, id_puesto, id_requisito });
        }

        
        public void Crear(RequisitoPuesto requisito)
        {
            using var conexion = _dbFactory.CrearConexion();
            conexion.Execute(
                "INSERT INTO requisito_puesto (nombre_requisito, id_puesto) VALUES (@nombre_requisito, @id_puesto)",
                requisito);
        }

        public void Actualizar(RequisitoPuesto requisito)
        {
            using var conexion = _dbFactory.CrearConexion();
            conexion.Execute(
                "UPDATE requisito_puesto SET nombre_requisito = @nombre_requisito, id_puesto = @id_puesto WHERE id_requisito = @id_requisito",
                requisito);
        }

        public void Eliminar(int id)
        {
            using var conexion = _dbFactory.CrearConexion();
            conexion.Execute("DELETE FROM requisito_puesto WHERE id_requisito = @Id", new { Id = id });
        }

        public bool RequisitoExiste(string nombre, int idPuesto, int? idExcluir = null)
        {
            using var conexion = _dbFactory.CrearConexion();
            return conexion.QueryFirstOrDefault<RequisitoPuesto>("SELECT id_requisito FROM requisito_puesto WHERE nombre_requisito = @Nombre AND id_puesto = @IdPuesto AND id_requisito != @IdExcluir",new { Nombre = nombre, IdPuesto = idPuesto, IdExcluir = idExcluir ?? 0 }) != null;
        }
    }
}

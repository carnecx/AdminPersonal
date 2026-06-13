using AdminPersonal.Entities;
using Dapper;

namespace AdminPersonal.Repository
{
    public class BitacoraRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;

    public BitacoraRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task RegistrarAsync(int idUsuario, string descripcion)
        {
            using var conexion = _connectionFactory.CrearConexion();

            await conexion.ExecuteAsync(
                "INSERT INTO bitacora (id_usuario, descripcion) VALUES (@idUsuario, @descripcion)",
                new { idUsuario, descripcion });
        }

        public async Task<int> ContarAsync(string? usuario, string? descripcion)
        {
            using var conexion = _connectionFactory.CrearConexion();

            var sql = @"
            SELECT COUNT(*)
            FROM bitacora b
            INNER JOIN usuario u ON u.id_usuario = b.id_usuario
            WHERE (@usuario IS NULL OR @usuario = '' OR u.nombre_usuario LIKE CONCAT('%', @usuario, '%'))
              AND (@descripcion IS NULL OR @descripcion = '' OR b.descripcion LIKE CONCAT('%', @descripcion, '%'))";

            return await conexion.QueryFirstAsync<int>(
                sql,
                new { usuario, descripcion });
        }

        public async Task<IEnumerable<Bitacora>> ListarAsync(
            string? usuario,
            string? descripcion,
            string orden,
            int pagina,
            int tamanoPagina)
        {
            using var conexion = _connectionFactory.CrearConexion();

            int offset = (pagina - 1) * tamanoPagina;

            var sql = @"
            SELECT b.id_bitacora,
                   b.fecha,
                   b.id_usuario,
                   u.nombre_usuario,
                   b.descripcion
            FROM bitacora b
            INNER JOIN usuario u ON u.id_usuario = b.id_usuario
            WHERE (@usuario IS NULL OR @usuario = '' OR u.nombre_usuario LIKE CONCAT('%', @usuario, '%'))
              AND (@descripcion IS NULL OR @descripcion = '' OR b.descripcion LIKE CONCAT('%', @descripcion, '%'))";

            sql += orden switch
            {
                "fecha_asc" => " ORDER BY b.fecha ASC",
                "usuario_asc" => " ORDER BY u.nombre_usuario ASC",
                "usuario_desc" => " ORDER BY u.nombre_usuario DESC",
                _ => " ORDER BY b.fecha DESC"
            };

            sql += " LIMIT @tamanoPagina OFFSET @offset";

            return await conexion.QueryAsync<Bitacora>(
                sql,
                new
                {
                    usuario,
                    descripcion,
                    tamanoPagina,
                    offset
                });
        }
    }
}

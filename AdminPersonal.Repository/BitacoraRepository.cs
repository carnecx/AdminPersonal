using AdminPersonal.Entities;
using Dapper;
using Microsoft.Extensions.Configuration;
using MySqlConnector;

namespace AdminPersonal.Repository
{
    // repositorio encargado de realizar las operaciones de bitacora en la base de datos
    public class BitacoraRepository
    {
        // almacena la cadena de conexion obtenida desde appsettings.json
        private readonly string cadenaConexion;

        // constructor que recibe la configuracion del proyecto
        public BitacoraRepository(IConfiguration config)
        {
            // obtiene la cadena de conexion configurada para mysql
            cadenaConexion = config.GetConnectionString("DefaultConnection")!;
        }

        // metodo privado que abre una conexion a mysql
        private MySqlConnection AbrirConexion()
        {
            return new MySqlConnection(cadenaConexion);
        }

        // registra una accion en la bitacora
        public async Task RegistrarAsync(int idUsuario, string descripcion)
        {
            using var conexion = AbrirConexion();

            // inserta el usuario y la descripcion de la accion realizada
            await conexion.ExecuteAsync(
                "INSERT INTO bitacora (id_usuario, descripcion) VALUES (@idUsuario, @descripcion)",
                new { idUsuario, descripcion });
        }

        // cuenta la cantidad total de registros para la paginacion
        public async Task<int> ContarAsync(string? usuario, string? descripcion)
        {
            using var conexion = AbrirConexion();

            // consulta que permite filtrar por usuario y descripcion
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

        // obtiene los registros de bitacora aplicando filtros y paginacion
        public async Task<IEnumerable<Bitacora>> ListarAsync(
            string? usuario,
            string? descripcion,
            string orden,
            int pagina,
            int tamanoPagina)
        {
            using var conexion = AbrirConexion();

            // calcula desde cual registro iniciar la consulta
            int offset = (pagina - 1) * tamanoPagina;

            // consulta principal de bitacora
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

            // determina el orden seleccionado por el usuario
            sql += orden switch
            {
                "fecha_asc" => " ORDER BY b.fecha ASC",
                "usuario_asc" => " ORDER BY u.nombre_usuario ASC",
                "usuario_desc" => " ORDER BY u.nombre_usuario DESC",

                // por defecto ordena por fecha descendente
                _ => " ORDER BY b.fecha DESC"
            };

            // aplica paginacion usando limit y offset
            sql += " LIMIT @tamanoPagina OFFSET @offset";

            // ejecuta la consulta y devuelve la lista de registros
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
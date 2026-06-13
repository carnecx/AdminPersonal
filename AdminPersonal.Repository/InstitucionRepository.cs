using AdminPersonal.Entities;
using Dapper;

namespace AdminPersonal.Repository
{
    // repositorio encargado de administrar las instituciones educativas
    public class InstitucionRepository
    {
        // fabrica utilizada para crear conexiones a la base de datos
        private readonly IDbConnectionFactory _connectionFactory;

    // constructor que recibe la fabrica de conexiones mediante inyeccion de dependencias
    public InstitucionRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        // obtiene la lista de instituciones educativas
        public async Task<IEnumerable<InstitucionEducativa>> ObtenerTodosAsync()
        {
            // crea una conexion utilizando la fabrica
            using var conexion = _connectionFactory.CrearConexion();

            // consulta que devuelve las instituciones ordenadas por nombre
            // se muestran un maximo de 10 registros
            return await conexion.QueryAsync<InstitucionEducativa>(
                "SELECT * FROM institucion_educativa ORDER BY nombre LIMIT 10");
        }

        // convierte las filas devueltas por el query en objetos InstitucionEducativa

        // busca una institucion por su identificador
        public async Task<InstitucionEducativa?> ObtenerPorIdAsync(int id)
        {
            // crea una conexion utilizando la fabrica
            using var conexion = _connectionFactory.CrearConexion();

            // retorna la institucion correspondiente al id recibido
            return await conexion.QueryFirstOrDefaultAsync<InstitucionEducativa>(
                "SELECT * FROM institucion_educativa WHERE id_institucion = @id",
                new { id });
        }

        // inserta una nueva institucion educativa
        public async Task InsertarAsync(InstitucionEducativa item)
        {
            // crea una conexion utilizando la fabrica
            using var conexion = _connectionFactory.CrearConexion();

            // guarda codigo y nombre en la tabla institucion_educativa
            await conexion.ExecuteAsync(
                "INSERT INTO institucion_educativa (codigo, nombre) VALUES (@codigo, @nombre)",
                item);
        }

        // actualiza una institucion existente
        public async Task ActualizarAsync(InstitucionEducativa item)
        {
            // crea una conexion utilizando la fabrica
            using var conexion = _connectionFactory.CrearConexion();

            // modifica los datos de una institucion segun su id
            await conexion.ExecuteAsync(
                "UPDATE institucion_educativa SET codigo = @codigo, nombre = @nombre WHERE id_institucion = @id_institucion",
                item);
        }

        // elimina una institucion educativa
        public async Task EliminarAsync(int id)
        {
            // crea una conexion utilizando la fabrica
            using var conexion = _connectionFactory.CrearConexion();

            // elimina el registro correspondiente al id recibido
            // si existen relaciones con otras tablas se genera una excepcion
            await conexion.ExecuteAsync(
                "DELETE FROM institucion_educativa WHERE id_institucion = @id",
                new { id });
        }
    }
}

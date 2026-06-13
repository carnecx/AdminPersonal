using AdminPersonal.Entities;
using Dapper;
using Microsoft.Extensions.Configuration;
using MySqlConnector;

namespace AdminPersonal.Repository
{
    // repositorio encargado de administrar las instituciones educativas
    public class InstitucionRepository
    {
        // almacena la cadena de conexion a mysql
        private readonly string cadenaConexion;

        // constructor que obtiene la cadena de conexion desde appsettings.json
        public InstitucionRepository(IConfiguration config)
        {
            cadenaConexion = config.GetConnectionString("DefaultConnection")!;
        }

        // abre una conexion a la base de datos
        private MySqlConnection AbrirConexion()
        {
            return new MySqlConnection(cadenaConexion);
        }

        // obtiene la lista de instituciones educativas
        public async Task<IEnumerable<InstitucionEducativa>> ObtenerTodosAsync()
        {
            using var conexion = AbrirConexion();

            // consulta que devuelve las instituciones ordenadas por nombre
            // se muestran un maximo de 10 registros
            return await conexion.QueryAsync<InstitucionEducativa>(
                "SELECT * FROM institucion_educativa ORDER BY nombre LIMIT 10");
        }
        //convierte el query las filas que devuelve en objetos

        // busca una institucion por su identificador
        public async Task<InstitucionEducativa?> ObtenerPorIdAsync(int id)
        {
            using var conexion = AbrirConexion();

            // retorna la institucion correspondiente al id recibido
           
            return await conexion.QueryFirstOrDefaultAsync<InstitucionEducativa>(
                "SELECT * FROM institucion_educativa WHERE id_institucion = @id",
                new { id });
        }

        // inserta una nueva institucion educativa
        public async Task InsertarAsync(InstitucionEducativa item)
        {
            using var conexion = AbrirConexion();

            // guarda codigo y nombre en la tabla institucion_educativa
            await conexion.ExecuteAsync(
                "INSERT INTO institucion_educativa (codigo, nombre) VALUES (@codigo, @nombre)",
                item);
        }

        // actualiza una institucion existente
        public async Task ActualizarAsync(InstitucionEducativa item)
        {
            using var conexion = AbrirConexion();

            // modifica los datos de una institucion segun su id
            await conexion.ExecuteAsync(
                "UPDATE institucion_educativa SET codigo = @codigo, nombre = @nombre WHERE id_institucion = @id_institucion",
                item);
        }

        // elimina una institucion educativa
        public async Task EliminarAsync(int id)
        {
            using var conexion = AbrirConexion();

            // elimina el registro correspondiente al id recibido
            // si existen relaciones con otras tablas se genera una excepcion
            await conexion.ExecuteAsync(
                "DELETE FROM institucion_educativa WHERE id_institucion = @id",
                new { id });
        }
    }
}
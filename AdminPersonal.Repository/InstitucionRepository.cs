using AdminPersonal.Entities;
using Dapper;
using Microsoft.Extensions.Configuration;
using MySqlConnector;

namespace AdminPersonal.Repository
{
    public class InstitucionRepository
    {
        private readonly string cadenaConexion;

        public InstitucionRepository(IConfiguration config)
        {
            cadenaConexion = config.GetConnectionString("DefaultConnection")!;
        }

        private MySqlConnection AbrirConexion()
        {
            return new MySqlConnection(cadenaConexion);
        }

        public async Task<IEnumerable<InstitucionEducativa>> ObtenerTodosAsync()
        {
            using var conexion = AbrirConexion();
            return await conexion.QueryAsync<InstitucionEducativa>(
                "SELECT * FROM institucion_educativa ORDER BY nombre LIMIT 10");
        }

        public async Task<InstitucionEducativa?> ObtenerPorIdAsync(int id)
        {
            using var conexion = AbrirConexion();
            return await conexion.QueryFirstOrDefaultAsync<InstitucionEducativa>(
                "SELECT * FROM institucion_educativa WHERE id_institucion = @id", new { id });
        }

        public async Task InsertarAsync(InstitucionEducativa item)
        {
            using var conexion = AbrirConexion();
            await conexion.ExecuteAsync(
                "INSERT INTO institucion_educativa (codigo, nombre) VALUES (@codigo, @nombre)", item);
        }

        public async Task ActualizarAsync(InstitucionEducativa item)
        {
            using var conexion = AbrirConexion();
            await conexion.ExecuteAsync(
                "UPDATE institucion_educativa SET codigo = @codigo, nombre = @nombre WHERE id_institucion = @id_institucion", item);
        }

        public async Task EliminarAsync(int id)
        {
            using var conexion = AbrirConexion();
            await conexion.ExecuteAsync(
                "DELETE FROM institucion_educativa WHERE id_institucion = @id", new { id });
        }
    }
}

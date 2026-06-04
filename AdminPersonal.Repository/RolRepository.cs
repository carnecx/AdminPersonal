using AdminPersonal.Entities;
using Dapper;
using Microsoft.Extensions.Configuration;
using MySqlConnector;

namespace AdminPersonal.Repository
{
    public class RolRepository
    {
        private readonly string cadenaConexion;

        public RolRepository(IConfiguration config)
        {
            cadenaConexion = config.GetConnectionString("DefaultConnection")!;
        }

        private MySqlConnection AbrirConexion()
        {
            return new MySqlConnection(cadenaConexion);
        }

        public async Task<IEnumerable<Rol>> ObtenerTodosAsync()
        {
            using var conexion = AbrirConexion();
            return await conexion.QueryAsync<Rol>("SELECT id_rol, nombre_rol FROM rol");
        }

        public async Task<Rol?> ObtenerPorIdAsync(int id)
        {
            using var conexion = AbrirConexion();
            return await conexion.QueryFirstOrDefaultAsync<Rol>("SELECT id_rol, nombre_rol FROM rol WHERE id_rol = @Id",new { Id = id });
        }

        public async Task InsertarAsync(Rol rol)
        {
            using var conexion = AbrirConexion();
            await conexion.ExecuteAsync("INSERT INTO rol (nombre_rol) VALUES (@nombre_rol)", rol);
        }

        public async Task ActualizarAsync(Rol rol)
        {
            using var conexion = AbrirConexion();
            await conexion.ExecuteAsync("UPDATE rol SET nombre_rol = @nombre_rol WHERE id_rol = @id_rol", rol);
        }

        public async Task EliminarAsync(int id)
        {
            using var conexion = AbrirConexion();
            await conexion.ExecuteAsync("DELETE FROM rol WHERE id_rol = @Id", new { Id = id });
        }

        public async Task<bool> NombreExisteAsync(string nombre, int? idExcluir = null)
        {
            using var conexion = AbrirConexion();
            return await conexion.QueryFirstOrDefaultAsync<Rol>("SELECT id_rol FROM rol WHERE nombre_rol = @nombre_rol AND id_rol != @IdExcluir",new { nombre_rol = nombre, IdExcluir = idExcluir ?? 0 }) != null;
        }

        public async Task<bool> EstaAsignadoAUsuarioAsync(int id)
        {
            using var conexion = AbrirConexion();
            return await conexion.QueryFirstOrDefaultAsync<dynamic>("SELECT id_rol FROM usuario_rol WHERE id_rol = @Id",new { Id = id }) != null;
        }
    }
}
using AdminPersonal.Entities;
using Dapper;
using Microsoft.Extensions.Configuration;
using MySqlConnector;

namespace AdminPersonal.Repository
{
    public class UsuarioRepository
    {
        private readonly string cadenaConexion;

        public UsuarioRepository(IConfiguration config)
        {
            cadenaConexion = config.GetConnectionString("DefaultConnection")!;
        }

        private MySqlConnection AbrirConexion()
        {
            return new MySqlConnection(cadenaConexion);
        }

        public async Task<Usuario?> BuscarPorUsuarioAsync(string nombreUsuario)
        {
            using var conexion = AbrirConexion();
            return await conexion.QueryFirstOrDefaultAsync<Usuario>(
                "SELECT * FROM usuario WHERE nombre_usuario = @nombreUsuario", new { nombreUsuario });
        }

        public async Task RegistrarFalloAsync(int idUsuario, int nuevosIntentos, string nuevoEstado)
        {
            using var conexion = AbrirConexion();
            await conexion.ExecuteAsync(
                "UPDATE usuario SET intentos_fallidos = @intentos, estado = @estado WHERE id_usuario = @id",
                new { intentos = nuevosIntentos, estado = nuevoEstado, id = idUsuario });
        }

        public async Task ReiniciarIntentosAsync(int idUsuario)
        {
            using var conexion = AbrirConexion();
            await conexion.ExecuteAsync(
                "UPDATE usuario SET intentos_fallidos = 0 WHERE id_usuario = @id", new { id = idUsuario });
        }

        public async Task<string?> ObtenerRolAsync(int idUsuario)
        {
            using var conexion = AbrirConexion();
            return await conexion.QueryFirstOrDefaultAsync<string>(
                @"SELECT r.nombre_rol
                  FROM rol r
                  INNER JOIN usuario_rol ur ON ur.id_rol = r.id_rol
                  WHERE ur.id_usuario = @idUsuario", new { idUsuario });
        }
    }
}

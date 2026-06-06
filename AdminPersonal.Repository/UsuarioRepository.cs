using AdminPersonal.Entities;
using Dapper;
using Microsoft.Extensions.Configuration;
using MySqlConnector;
using System.Security.Cryptography;
using System.Text;

namespace AdminPersonal.Repository
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly string _connectionString;

        private static readonly byte[] AesKey =
            Encoding.UTF8.GetBytes("AdminPersonalKey1AdminPersonalK1");

        public UsuarioRepository(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("DefaultConnection")!;
        }

        public static string Encrypt(string plainText)
        {
            byte[] nonce = new byte[AesGcm.NonceByteSizes.MaxSize];
            byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);
            byte[] cipherBytes = new byte[plainBytes.Length];
            byte[] tag = new byte[AesGcm.TagByteSizes.MaxSize];
            RandomNumberGenerator.Fill(nonce);
            using var aes = new AesGcm(AesKey, AesGcm.TagByteSizes.MaxSize);
            aes.Encrypt(nonce, plainBytes, cipherBytes, tag);
            byte[] combined = new byte[nonce.Length + tag.Length + cipherBytes.Length];
            Buffer.BlockCopy(nonce, 0, combined, 0, nonce.Length);
            Buffer.BlockCopy(tag, 0, combined, nonce.Length, tag.Length);
            Buffer.BlockCopy(cipherBytes, 0, combined, nonce.Length + tag.Length, cipherBytes.Length);
            return Convert.ToBase64String(combined);
        }

        public static string Decrypt(string cipherText)
        {
            byte[] combined = Convert.FromBase64String(cipherText);
            int nonceSize = AesGcm.NonceByteSizes.MaxSize;
            int tagSize = AesGcm.TagByteSizes.MaxSize;
            byte[] nonce = combined[..nonceSize];
            byte[] tag = combined[nonceSize..(nonceSize + tagSize)];
            byte[] cipher = combined[(nonceSize + tagSize)..];
            byte[] plain = new byte[cipher.Length];
            using var aes = new AesGcm(AesKey, AesGcm.TagByteSizes.MaxSize);
            aes.Decrypt(nonce, cipher, tag, plain);
            return Encoding.UTF8.GetString(plain);
        }

        // ── Métodos del Login (compañero) ──────────────────────────────────

        public async Task<Usuario?> BuscarPorUsuarioAsync(string nombreUsuario)
        {
            using var conn = new MySqlConnection(_connectionString);
            return await conn.QueryFirstOrDefaultAsync<Usuario>(
                @"SELECT id_usuario        AS IdUsuario,
                         nombre_usuario    AS NombreUsuario,
                         nombre_completo   AS NombreCompleto,
                         correo            AS Correo,
                         contrasena        AS Contrasena,
                         estado            AS Estado,
                         intentos_fallidos AS IntentosFallidos
                  FROM usuario WHERE nombre_usuario = @nombreUsuario",
                new { nombreUsuario });
        }

        public async Task RegistrarFalloAsync(Usuario usuario)
        {
            using var conn = new MySqlConnection(_connectionString);
            int intentos = usuario.IntentosFallidos + 1;
            string nuevoEstado = intentos >= 3 ? "Bloqueado" : usuario.Estado;
            await conn.ExecuteAsync(
                "UPDATE usuario SET intentos_fallidos=@intentos, estado=@estado WHERE id_usuario=@id",
                new { intentos, estado = nuevoEstado, id = usuario.IdUsuario });
        }

        public async Task ReiniciarIntentosAsync(int idUsuario)
        {
            using var conn = new MySqlConnection(_connectionString);
            await conn.ExecuteAsync(
                "UPDATE usuario SET intentos_fallidos=0 WHERE id_usuario=@id",
                new { id = idUsuario });
        }

        public async Task<string?> ObtenerRolAsync(int idUsuario)
        {
            using var conn = new MySqlConnection(_connectionString);
            return await conn.QueryFirstOrDefaultAsync<string>(
                @"SELECT r.nombre_rol FROM rol r
                  INNER JOIN usuario_rol ur ON ur.id_rol = r.id_rol
                  WHERE ur.id_usuario = @id LIMIT 1",
                new { id = idUsuario });
        }

        public async Task<int?> ObtenerIdRolAsync(int idUsuario)
        {
            using var conn = new MySqlConnection(_connectionString);
            return await conn.QueryFirstOrDefaultAsync<int?>(
                "SELECT id_rol FROM usuario_rol WHERE id_usuario = @id LIMIT 1",
                new { id = idUsuario });
        }

        // ── Métodos del SEG6 (tuyos) ───────────────────────────────────────

        public async Task<IEnumerable<Usuario>> ObtenerTodosAsync()
        {
            using var conn = new MySqlConnection(_connectionString);
            const string sql = @"
                SELECT u.id_usuario        AS IdUsuario,
                       u.nombre_usuario    AS NombreUsuario,
                       u.nombre_completo   AS NombreCompleto,
                       u.correo            AS Correo,
                       u.estado            AS Estado,
                       u.intentos_fallidos AS IntentosFallidos,
                       GROUP_CONCAT(r.nombre_rol ORDER BY r.nombre_rol SEPARATOR ', ') AS RolesNombres
                FROM usuario u
                LEFT JOIN usuario_rol ur ON u.id_usuario = ur.id_usuario
                LEFT JOIN rol r ON ur.id_rol = r.id_rol
                GROUP BY u.id_usuario";
            return await conn.QueryAsync<Usuario>(sql);
        }

        public async Task<Usuario?> ObtenerPorIdAsync(int id)
        {
            using var conn = new MySqlConnection(_connectionString);
            const string sqlUsuario = @"
                SELECT id_usuario        AS IdUsuario,
                       nombre_usuario    AS NombreUsuario,
                       nombre_completo   AS NombreCompleto,
                       correo            AS Correo,
                       estado            AS Estado,
                       intentos_fallidos AS IntentosFallidos
                FROM usuario WHERE id_usuario = @Id";
            var usuario = await conn.QueryFirstOrDefaultAsync<Usuario>(sqlUsuario, new { Id = id });
            if (usuario == null) return null;
            const string sqlRoles = "SELECT id_rol FROM usuario_rol WHERE id_usuario = @Id";
            usuario.RolesSeleccionados = (await conn.QueryAsync<int>(sqlRoles, new { Id = id })).ToList();
            return usuario;
        }

        public async Task<int> CrearAsync(Usuario u, int idUsuarioSesion)
        {
            using var conn = new MySqlConnection(_connectionString);
            await conn.OpenAsync();
            using var tx = await conn.BeginTransactionAsync();
            try
            {
                string contrasenaEncriptada = Encrypt(u.Contrasena);
                const string sqlInsert = @"
                    INSERT INTO usuario (nombre_usuario, nombre_completo, correo, contrasena, estado, intentos_fallidos)
                    VALUES (@NombreUsuario, @NombreCompleto, @Correo, @Contrasena, @Estado, 0);
                    SELECT LAST_INSERT_ID();";
                int nuevoId = await conn.ExecuteScalarAsync<int>(sqlInsert, new
                {
                    u.NombreUsuario,
                    u.NombreCompleto,
                    u.Correo,
                    Contrasena = contrasenaEncriptada,
                    u.Estado
                }, tx);
                await AsignarRolesAsync(conn, tx, nuevoId, u.RolesSeleccionados);
                await RegistrarBitacoraAsync(conn, tx, idUsuarioSesion,
                    $"Creación de usuario: NombreUsuario={u.NombreUsuario}, NombreCompleto={u.NombreCompleto}, Correo={u.Correo}, Estado={u.Estado}, Roles={string.Join(",", u.RolesSeleccionados)}");
                await tx.CommitAsync();
                return nuevoId;
            }
            catch { await tx.RollbackAsync(); throw; }
        }

        public async Task ActualizarAsync(Usuario u, int idUsuarioSesion)
        {
            using var conn = new MySqlConnection(_connectionString);
            await conn.OpenAsync();
            using var tx = await conn.BeginTransactionAsync();
            try
            {
                if (!string.IsNullOrWhiteSpace(u.Contrasena))
                {
                    string enc = Encrypt(u.Contrasena);
                    const string sqlConPass = @"
                        UPDATE usuario
                        SET nombre_usuario=@NombreUsuario, nombre_completo=@NombreCompleto,
                            correo=@Correo, contrasena=@Contrasena, estado=@Estado
                        WHERE id_usuario=@IdUsuario";
                    await conn.ExecuteAsync(sqlConPass, new
                    {
                        u.NombreUsuario,
                        u.NombreCompleto,
                        u.Correo,
                        Contrasena = enc,
                        u.Estado,
                        u.IdUsuario
                    }, tx);
                }
                else
                {
                    const string sqlSinPass = @"
                        UPDATE usuario
                        SET nombre_usuario=@NombreUsuario, nombre_completo=@NombreCompleto,
                            correo=@Correo, estado=@Estado
                        WHERE id_usuario=@IdUsuario";
                    await conn.ExecuteAsync(sqlSinPass, new
                    {
                        u.NombreUsuario,
                        u.NombreCompleto,
                        u.Correo,
                        u.Estado,
                        u.IdUsuario
                    }, tx);
                }
                await conn.ExecuteAsync("DELETE FROM usuario_rol WHERE id_usuario=@Id",
                    new { Id = u.IdUsuario }, tx);
                await AsignarRolesAsync(conn, tx, u.IdUsuario, u.RolesSeleccionados);
                await RegistrarBitacoraAsync(conn, tx, idUsuarioSesion,
                    $"Actualización de usuario ID={u.IdUsuario}: NombreUsuario={u.NombreUsuario}, NombreCompleto={u.NombreCompleto}, Correo={u.Correo}, Estado={u.Estado}, Roles={string.Join(",", u.RolesSeleccionados)}");
                await tx.CommitAsync();
            }
            catch { await tx.RollbackAsync(); throw; }
        }

        public async Task<(bool ok, string mensaje)> EliminarAsync(int id, int idUsuarioSesion)
        {
            using var conn = new MySqlConnection(_connectionString);
            await conn.OpenAsync();
            int enBitacora = await conn.ExecuteScalarAsync<int>(
                "SELECT COUNT(*) FROM bitacora WHERE id_usuario=@Id", new { Id = id });
            if (enBitacora > 0)
                return (false, "No se puede eliminar un registro con datos relacionados.");
            using var tx = await conn.BeginTransactionAsync();
            try
            {
                await conn.ExecuteAsync("DELETE FROM usuario_rol WHERE id_usuario=@Id", new { Id = id }, tx);
                await conn.ExecuteAsync("DELETE FROM usuario WHERE id_usuario=@Id", new { Id = id }, tx);
                await RegistrarBitacoraAsync(conn, tx, idUsuarioSesion, $"Eliminación de usuario ID={id}");
                await tx.CommitAsync();
                return (true, "Usuario eliminado correctamente.");
            }
            catch { await tx.RollbackAsync(); throw; }
        }

        public async Task CambiarEstadoAsync(int id, string nuevoEstado, int idUsuarioSesion)
        {
            using var conn = new MySqlConnection(_connectionString);
            await conn.OpenAsync();
            using var tx = await conn.BeginTransactionAsync();
            await conn.ExecuteAsync(
                "UPDATE usuario SET estado=@Estado WHERE id_usuario=@Id",
                new { Estado = nuevoEstado, Id = id }, tx);
            await RegistrarBitacoraAsync(conn, tx, idUsuarioSesion,
                $"Cambio de estado usuario ID={id}: nuevo estado={nuevoEstado}");
            await tx.CommitAsync();
        }

        public async Task<IEnumerable<Rol>> ObtenerRolesAsync()
        {
            using var conn = new MySqlConnection(_connectionString);
            return await conn.QueryAsync<Rol>(
                "SELECT id_rol, nombre_rol FROM rol ORDER BY nombre_rol");
        }

        private static async Task AsignarRolesAsync(MySqlConnection conn,
            MySqlTransaction tx, int idUsuario, List<int> roles)
        {
            foreach (var idRol in roles)
                await conn.ExecuteAsync(
                    "INSERT INTO usuario_rol (id_usuario, id_rol) VALUES (@IdUsuario, @IdRol)",
                    new { IdUsuario = idUsuario, IdRol = idRol }, tx);
        }

        private static async Task RegistrarBitacoraAsync(MySqlConnection conn,
            MySqlTransaction tx, int idUsuario, string descripcion)
        {
            await conn.ExecuteAsync(
                "INSERT INTO bitacora (id_usuario, descripcion) VALUES (@IdUsuario, @Descripcion)",
                new { IdUsuario = idUsuario, Descripcion = descripcion }, tx);
        }
    }
}
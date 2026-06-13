using AdminPersonal.Entities;
using Dapper;
using Microsoft.Extensions.Configuration;
using MySqlConnector;
using System.Security.Cryptography;
using System.Text;

namespace AdminPersonal.Repository
{
    // repositorio encargado de administrar los usuarios del sistema
    public class UsuarioRepository : IUsuarioRepository
    {
        // cadena de conexion utilizada para conectarse a mysql
        private readonly string _connectionString;

        // clave utilizada por el algoritmo aes para encriptar contraseñas
        private static readonly byte[] AesKey =
            Encoding.UTF8.GetBytes("AdminPersonalKey1AdminPersonalK1");

        // constructor que obtiene la cadena de conexion desde appsettings.json
        public UsuarioRepository(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("DefaultConnection")!;
        }

        // encripta una contraseña antes de almacenarla en la base de datos
        public static string Encrypt(string plainText)
        {
            // genera un nonce aleatorio para aumentar la seguridad
            byte[] nonce = new byte[AesGcm.NonceByteSizes.MaxSize];

            // convierte la contraseña a bytes
            byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);

            // arreglo donde se almacenara la contraseña cifrada
            byte[] cipherBytes = new byte[plainBytes.Length];

            // etiqueta de autenticacion utilizada por aes gcm
            byte[] tag = new byte[AesGcm.TagByteSizes.MaxSize];

            // genera valores aleatorios para el nonce
            RandomNumberGenerator.Fill(nonce);

            // crea una instancia de aes gcm usando la clave definida
            using var aes = new AesGcm(AesKey, AesGcm.TagByteSizes.MaxSize);

            // realiza el proceso de cifrado
            aes.Encrypt(nonce, plainBytes, cipherBytes, tag);

            // une nonce, tag y texto cifrado en un solo arreglo
            byte[] combined = new byte[nonce.Length + tag.Length + cipherBytes.Length];

            Buffer.BlockCopy(nonce, 0, combined, 0, nonce.Length);
            Buffer.BlockCopy(tag, 0, combined, nonce.Length, tag.Length);
            Buffer.BlockCopy(cipherBytes, 0, combined, nonce.Length + tag.Length, cipherBytes.Length);

            // retorna el resultado en formato base64 para almacenarlo en mysql
            return Convert.ToBase64String(combined);
        }

        // desencripta una contraseña almacenada en la base de datos
        public static string Decrypt(string cipherText)
        {
            // convierte el texto base64 nuevamente a bytes
            byte[] combined = Convert.FromBase64String(cipherText);

            // obtiene los tamaños utilizados por aes gcm
            int nonceSize = AesGcm.NonceByteSizes.MaxSize;
            int tagSize = AesGcm.TagByteSizes.MaxSize;

            // extrae el nonce utilizado durante el cifrado
            byte[] nonce = combined[..nonceSize];

            // extrae la etiqueta de autenticacion
            byte[] tag = combined[nonceSize..(nonceSize + tagSize)];

            // extrae el texto cifrado
            byte[] cipher = combined[(nonceSize + tagSize)..];

            // arreglo donde se almacenara el texto original
            byte[] plain = new byte[cipher.Length];

            // crea una instancia de aes gcm utilizando la misma clave
            using var aes = new AesGcm(AesKey, AesGcm.TagByteSizes.MaxSize);

            // realiza el proceso de descifrado
            aes.Decrypt(nonce, cipher, tag, plain);

            // convierte los bytes nuevamente a texto
            return Encoding.UTF8.GetString(plain);
        }

        //  metodos del Login 

        // busca un usuario por nombre de usuario para el proceso de autenticacion
        public async Task<Usuario?> BuscarPorUsuarioAsync(string nombreUsuario)
        {
            // abre la conexion a mysql
            using var conn = new MySqlConnection(_connectionString);

            // consulta la informacion del usuario segun el nombre ingresado
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

        // registra un intento fallido de inicio de sesion
        public async Task RegistrarFalloAsync(Usuario usuario)
        {
            using var conn = new MySqlConnection(_connectionString);

            int intentos = usuario.IntentosFallidos + 1;

            int maxIntentos = await conn.QueryFirstOrDefaultAsync<int>(
                "SELECT valor FROM parametro WHERE codigo = 'INTENTOS_LOGIN_MAX'");

            if (maxIntentos <= 0)
                maxIntentos = 3;

            string nuevoEstado = intentos >= maxIntentos
                ? "Bloqueado"
                : usuario.Estado;

            await conn.ExecuteAsync(
                "UPDATE usuario SET intentos_fallidos=@intentos, estado=@estado WHERE id_usuario=@id",
                new
                {
                    intentos,
                    estado = nuevoEstado,
                    id = usuario.IdUsuario
                });
        }

        // reinicia el contador de intentos despues de un login exitoso
        public async Task ReiniciarIntentosAsync(int idUsuario)
        {
            // abre la conexion a mysql
            using var conn = new MySqlConnection(_connectionString);

            // establece los intentos fallidos nuevamente en cero
            await conn.ExecuteAsync(
                "UPDATE usuario SET intentos_fallidos=0 WHERE id_usuario=@id",
                new { id = idUsuario });
        }

        // obtiene el nombre del rol asociado al usuario
        public async Task<string?> ObtenerRolAsync(int idUsuario)
        {
            // abre la conexion a mysql
            using var conn = new MySqlConnection(_connectionString);

            // consulta el rol asignado al usuario mediante la tabla usuario_rol
            return await conn.QueryFirstOrDefaultAsync<string>(
                @"SELECT r.nombre_rol FROM rol r
          INNER JOIN usuario_rol ur ON ur.id_rol = r.id_rol
          WHERE ur.id_usuario = @id LIMIT 1",
                new { id = idUsuario });
        }

        // obtiene el identificador numerico del rol del usuario
        public async Task<int?> ObtenerIdRolAsync(int idUsuario)
        {
            // abre la conexion a mysql
            using var conn = new MySqlConnection(_connectionString);

            // consulta el id del rol asociado al usuario
            return await conn.QueryFirstOrDefaultAsync<int?>(
                "SELECT id_rol FROM usuario_rol WHERE id_usuario = @id LIMIT 1",
                new { id = idUsuario });
        }
        // ── Métodos del SEG6 

        // obtiene todos los usuarios registrados junto con sus roles
        public async Task<IEnumerable<Usuario>> ObtenerTodosAsync()
        {
            // abre la conexion a mysql
            using var conn = new MySqlConnection(_connectionString);

            // consulta los usuarios y une las tablas usuario_rol y rol
            // group_concat permite mostrar todos los roles del usuario en una sola columna
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

            // ejecuta la consulta y devuelve la lista de usuarios
            return await conn.QueryAsync<Usuario>(sql);
        }

        // obtiene un usuario especifico por su id
        public async Task<Usuario?> ObtenerPorIdAsync(int id)
        {
            // abre la conexion a mysql
            using var conn = new MySqlConnection(_connectionString);

            // consulta los datos principales del usuario
            const string sqlUsuario = @"
        SELECT id_usuario        AS IdUsuario,
               nombre_usuario    AS NombreUsuario,
               nombre_completo   AS NombreCompleto,
               correo            AS Correo,
               estado            AS Estado,
               intentos_fallidos AS IntentosFallidos
        FROM usuario WHERE id_usuario = @Id";

            // busca el usuario en la base de datos
            var usuario = await conn.QueryFirstOrDefaultAsync<Usuario>(
                sqlUsuario,
                new { Id = id });

            // si no existe, retorna null
            if (usuario == null)
                return null;

            // consulta los roles asociados al usuario
            const string sqlRoles = "SELECT id_rol FROM usuario_rol WHERE id_usuario = @Id";

            // guarda los roles encontrados en la propiedad roles seleccionados
            usuario.RolesSeleccionados = (await conn.QueryAsync<int>(
                sqlRoles,
                new { Id = id })).ToList();

            // retorna el usuario con sus roles
            return usuario;
        }

        // crea un nuevo usuario con su contraseña encriptada y roles asignados
        public async Task<int> CrearAsync(Usuario u, int idUsuarioSesion)
        {
            // abre la conexion a mysql
            using var conn = new MySqlConnection(_connectionString);

            // abre la conexion manualmente porque se usara una transaccion
            await conn.OpenAsync();

            // inicia una transaccion para que todo se guarde junto
            using var tx = await conn.BeginTransactionAsync();

            try
            {
                // encripta la contraseña antes de guardarla
                string contrasenaEncriptada = Encrypt(u.Contrasena);

                // inserta el usuario y devuelve el id generado
                const string sqlInsert = @"
            INSERT INTO usuario (nombre_usuario, nombre_completo, correo, contrasena, estado, intentos_fallidos)
            VALUES (@NombreUsuario, @NombreCompleto, @Correo, @Contrasena, @Estado, 0);
            SELECT LAST_INSERT_ID();";

                // ejecuta el insert y obtiene el nuevo id del usuario
                int nuevoId = await conn.ExecuteScalarAsync<int>(
                    sqlInsert,
                    new
                    {
                        u.NombreUsuario,
                        u.NombreCompleto,
                        u.Correo,
                        Contrasena = contrasenaEncriptada,
                        u.Estado
                    },
                    tx);

                // asigna los roles seleccionados al nuevo usuario
                await AsignarRolesAsync(
                    conn,
                    tx,
                    nuevoId,
                    u.RolesSeleccionados);

                // registra en bitacora la creacion del usuario
                // no se registra la contraseña por seguridad
                await RegistrarBitacoraAsync(
                    conn,
                    tx,
                    idUsuarioSesion,
                    $"Creación de usuario: NombreUsuario={u.NombreUsuario}, NombreCompleto={u.NombreCompleto}, Correo={u.Correo}, Estado={u.Estado}, Roles={string.Join(",", u.RolesSeleccionados)}");

                // confirma los cambios realizados en la transaccion
                await tx.CommitAsync();

                // retorna el id del usuario creado
                return nuevoId;
            }
            catch
            {
                // si ocurre un error, deshace todos los cambios
                await tx.RollbackAsync();

                // vuelve a lanzar el error para que la capa superior lo maneje
                throw;
            }
        }
        // actualiza los datos de un usuario existente
        public async Task ActualizarAsync(Usuario u, int idUsuarioSesion)
        {
            // abre la conexion a mysql
            using var conn = new MySqlConnection(_connectionString);

            // abre la conexion porque se va a trabajar con transaccion
            await conn.OpenAsync();

            // inicia una transaccion para asegurar que todos los cambios se guarden juntos
            using var tx = await conn.BeginTransactionAsync();

            try
            {
                // si el usuario escribio una nueva contrasena, se actualiza encriptada
                if (!string.IsNullOrWhiteSpace(u.Contrasena))
                {
                    // encripta la nueva contrasena
                    string enc = Encrypt(u.Contrasena);

                    // consulta para actualizar incluyendo contrasena
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
                    // si no se escribio contrasena, se actualizan solo los demas datos
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

                // elimina los roles actuales del usuario
                await conn.ExecuteAsync(
                    "DELETE FROM usuario_rol WHERE id_usuario=@Id",
                    new { Id = u.IdUsuario },
                    tx);

                // vuelve a asignar los roles seleccionados
                await AsignarRolesAsync(
                    conn,
                    tx,
                    u.IdUsuario,
                    u.RolesSeleccionados);

                // registra en bitacora la actualizacion realizada
                // no se registra la contrasena por seguridad
                await RegistrarBitacoraAsync(
                    conn,
                    tx,
                    idUsuarioSesion,
                    $"Actualización de usuario ID={u.IdUsuario}: NombreUsuario={u.NombreUsuario}, NombreCompleto={u.NombreCompleto}, Correo={u.Correo}, Estado={u.Estado}, Roles={string.Join(",", u.RolesSeleccionados)}");

                // confirma todos los cambios
                await tx.CommitAsync();
            }
            catch
            {
                // si ocurre un error, se revierten los cambios
                await tx.RollbackAsync();
                throw;
            }
        }

        // elimina un usuario si no tiene datos relacionados
        public async Task<(bool ok, string mensaje)> EliminarAsync(int id, int idUsuarioSesion)
        {
            // abre la conexion a mysql
            using var conn = new MySqlConnection(_connectionString);

            await conn.OpenAsync();

            // verifica si el usuario tiene registros en bitacora
            int enBitacora = await conn.ExecuteScalarAsync<int>(
                "SELECT COUNT(*) FROM bitacora WHERE id_usuario=@Id",
                new { Id = id });

            // si tiene registros relacionados, no permite eliminar
            if (enBitacora > 0)
                return (false, "No se puede eliminar un registro con datos relacionados.");

            // inicia una transaccion para eliminar roles y usuario juntos
            using var tx = await conn.BeginTransactionAsync();

            try
            {
                // elimina primero la relacion con roles
                await conn.ExecuteAsync(
                    "DELETE FROM usuario_rol WHERE id_usuario=@Id",
                    new { Id = id },
                    tx);

                // elimina el usuario
                await conn.ExecuteAsync(
                    "DELETE FROM usuario WHERE id_usuario=@Id",
                    new { Id = id },
                    tx);

                // registra la eliminacion en bitacora
                await RegistrarBitacoraAsync(
                    conn,
                    tx,
                    idUsuarioSesion,
                    $"Eliminación de usuario ID={id}");

                // confirma la eliminacion
                await tx.CommitAsync();

                return (true, "Usuario eliminado correctamente.");
            }
            catch
            {
                // si ocurre error, revierte todo
                await tx.RollbackAsync();
                throw;
            }
        }

        // cambia el estado del usuario
        public async Task CambiarEstadoAsync(int id, string nuevoEstado, int idUsuarioSesion)
        {
            // abre la conexion a mysql
            using var conn = new MySqlConnection(_connectionString);

            await conn.OpenAsync();

            // inicia una transaccion
            using var tx = await conn.BeginTransactionAsync();

            // actualiza el estado del usuario
            await conn.ExecuteAsync(
                "UPDATE usuario SET estado=@Estado WHERE id_usuario=@Id",
                new { Estado = nuevoEstado, Id = id },
                tx);

            // registra el cambio en bitacora
            await RegistrarBitacoraAsync(
                conn,
                tx,
                idUsuarioSesion,
                $"Cambio de estado usuario ID={id}: nuevo estado={nuevoEstado}");

            // confirma los cambios
            await tx.CommitAsync();
        }

        // obtiene todos los roles disponibles
        public async Task<IEnumerable<Rol>> ObtenerRolesAsync()
        {
            // abre la conexion a mysql
            using var conn = new MySqlConnection(_connectionString);

            // consulta los roles ordenados por nombre
            return await conn.QueryAsync<Rol>(
                "SELECT id_rol, nombre_rol FROM rol ORDER BY nombre_rol");
        }

        // asigna los roles seleccionados a un usuario
        private static async Task AsignarRolesAsync(
            MySqlConnection conn,
            MySqlTransaction tx,
            int idUsuario,
            List<int> roles)
        {
            // recorre cada rol seleccionado
            foreach (var idRol in roles)
                await conn.ExecuteAsync(
                    "INSERT INTO usuario_rol (id_usuario, id_rol) VALUES (@IdUsuario, @IdRol)",
                    new { IdUsuario = idUsuario, IdRol = idRol },
                    tx);
        }

        // registra acciones importantes en la bitacora
        private static async Task RegistrarBitacoraAsync(
            MySqlConnection conn,
            MySqlTransaction tx,
            int idUsuario,
            string descripcion)
        {
            // inserta el registro de bitacora
            await conn.ExecuteAsync(
                "INSERT INTO bitacora (id_usuario, descripcion) VALUES (@IdUsuario, @Descripcion)",
                new { IdUsuario = idUsuario, Descripcion = descripcion },
                tx);
        }
    }
}
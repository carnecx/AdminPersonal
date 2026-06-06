using AdminPersonal.Entities;
using Dapper;
using Microsoft.Extensions.Configuration;
using MySqlConnector;

namespace AdminPersonal.Repository
{
    public class OferenteRepository
    {
        private readonly string cadenaConexion;

        public OferenteRepository(IConfiguration config)
        {
            cadenaConexion = config.GetConnectionString("DefaultConnection")!;
        }

        private MySqlConnection AbrirConexion() => new MySqlConnection(cadenaConexion);

        public async Task<IEnumerable<Oferente>> ObtenerTodosAsync()
        {
            using var conexion = AbrirConexion();
            return await conexion.QueryAsync<Oferente>(
                @"SELECT id_oferente, identificacion, tipo_identificacion AS TipoIdentificacion,
                         nombre_completo AS NombreCompleto, fecha_nacimiento AS FechaNacimiento
                  FROM oferente ORDER BY nombre_completo");
        }

        public async Task<Oferente?> ObtenerPorIdAsync(int id)
        {
            using var conexion = AbrirConexion();
            return await conexion.QueryFirstOrDefaultAsync<Oferente>(
                @"SELECT id_oferente, identificacion, tipo_identificacion AS TipoIdentificacion,
                         nombre_completo AS NombreCompleto, fecha_nacimiento AS FechaNacimiento
                  FROM oferente WHERE id_oferente = @Id", new { Id = id });
        }

        public async Task<IEnumerable<string>> ObtenerCorreosAsync(int idOferente)
        {
            using var conexion = AbrirConexion();
            return await conexion.QueryAsync<string>(
                "SELECT correo FROM oferente_correo WHERE id_oferente = @id", new { id = idOferente });
        }

        public async Task<IEnumerable<string>> ObtenerTelefonosAsync(int idOferente)
        {
            using var conexion = AbrirConexion();
            return await conexion.QueryAsync<string>(
                "SELECT telefono FROM oferente_telefono WHERE id_oferente = @id", new { id = idOferente });
        }

        public async Task<IEnumerable<int>> ObtenerConcursosIdsAsync(int idOferente)
        {
            using var conexion = AbrirConexion();
            return await conexion.QueryAsync<int>(
                "SELECT id_concurso FROM oferente_concurso WHERE id_oferente = @id", new { id = idOferente });
        }

        public async Task<int> InsertarAsync(Oferente oferente)
        {
            using var conexion = AbrirConexion();
            await conexion.OpenAsync();
            using var tx = await conexion.BeginTransactionAsync();

            var id = await conexion.QueryFirstAsync<int>(
                @"INSERT INTO oferente (identificacion, tipo_identificacion, nombre_completo, fecha_nacimiento)
                  VALUES (@Identificacion, @TipoIdentificacion, @NombreCompleto, @FechaNacimiento);
                  SELECT LAST_INSERT_ID();", oferente, tx);

            foreach (var correo in oferente.Correos.Where(c => !string.IsNullOrWhiteSpace(c)))
                await conexion.ExecuteAsync(
                    "INSERT INTO oferente_correo (id_oferente, correo) VALUES (@id, @correo)",
                    new { id, correo }, tx);

            foreach (var tel in oferente.Telefonos.Where(t => !string.IsNullOrWhiteSpace(t)))
                await conexion.ExecuteAsync(
                    "INSERT INTO oferente_telefono (id_oferente, telefono) VALUES (@id, @tel)",
                    new { id, tel }, tx);

            foreach (var idConcurso in oferente.ConcursosIds)
                await conexion.ExecuteAsync(
                    "INSERT INTO oferente_concurso (id_oferente, id_concurso) VALUES (@id, @idConcurso)",
                    new { id, idConcurso }, tx);

            await tx.CommitAsync();
            return id;
        }

        public async Task ActualizarAsync(Oferente oferente)
        {
            using var conexion = AbrirConexion();
            await conexion.OpenAsync();
            using var tx = await conexion.BeginTransactionAsync();

            await conexion.ExecuteAsync(
                @"UPDATE oferente SET identificacion=@Identificacion,
                  tipo_identificacion=@TipoIdentificacion,
                  nombre_completo=@NombreCompleto,
                  fecha_nacimiento=@FechaNacimiento
                  WHERE id_oferente=@id_oferente", oferente, tx);

            await conexion.ExecuteAsync("DELETE FROM oferente_correo WHERE id_oferente=@id",
                new { id = oferente.id_oferente }, tx);
            foreach (var correo in oferente.Correos.Where(c => !string.IsNullOrWhiteSpace(c)))
                await conexion.ExecuteAsync(
                    "INSERT INTO oferente_correo (id_oferente, correo) VALUES (@id, @correo)",
                    new { id = oferente.id_oferente, correo }, tx);

            await conexion.ExecuteAsync("DELETE FROM oferente_telefono WHERE id_oferente=@id",
                new { id = oferente.id_oferente }, tx);
            foreach (var tel in oferente.Telefonos.Where(t => !string.IsNullOrWhiteSpace(t)))
                await conexion.ExecuteAsync(
                    "INSERT INTO oferente_telefono (id_oferente, telefono) VALUES (@id, @tel)",
                    new { id = oferente.id_oferente, tel }, tx);

            await conexion.ExecuteAsync("DELETE FROM oferente_concurso WHERE id_oferente=@id",
                new { id = oferente.id_oferente }, tx);
            foreach (var idConcurso in oferente.ConcursosIds)
                await conexion.ExecuteAsync(
                    "INSERT INTO oferente_concurso (id_oferente, id_concurso) VALUES (@id, @idConcurso)",
                    new { id = oferente.id_oferente, idConcurso }, tx);

            await tx.CommitAsync();
        }

        public async Task EliminarAsync(int id)
        {
            using var conexion = AbrirConexion();
            await conexion.OpenAsync();
            using var tx = await conexion.BeginTransactionAsync();
            await conexion.ExecuteAsync("DELETE FROM oferente_correo WHERE id_oferente=@id", new { id }, tx);
            await conexion.ExecuteAsync("DELETE FROM oferente_telefono WHERE id_oferente=@id", new { id }, tx);
            await conexion.ExecuteAsync("DELETE FROM oferente_concurso WHERE id_oferente=@id", new { id }, tx);
            await conexion.ExecuteAsync("DELETE FROM oferente WHERE id_oferente=@id", new { id }, tx);
            await tx.CommitAsync();
        }

        public async Task<bool> TieneRelacionesAsync(int id)
        {
            using var conexion = AbrirConexion();
            var count = await conexion.QueryFirstAsync<int>(
                @"SELECT (SELECT COUNT(*) FROM entrevista WHERE id_oferente=@id) +
                         (SELECT COUNT(*) FROM empleado   WHERE id_oferente=@id) +
                         (SELECT COUNT(*) FROM preparacion_academica WHERE id_oferente=@id)", new { id });
            return count > 0;
        }

        public async Task<bool> IdentificacionExisteAsync(string identificacion, int? idExcluir = null)
        {
            using var conexion = AbrirConexion();
            return await conexion.QueryFirstOrDefaultAsync<int?>(
                "SELECT id_oferente FROM oferente WHERE identificacion=@identificacion AND id_oferente!=@idExcluir",
                new { identificacion, idExcluir = idExcluir ?? 0 }) != null;
        }
    }
}
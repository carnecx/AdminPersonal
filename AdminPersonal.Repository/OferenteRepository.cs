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

        private MySqlConnection AbrirConexion()
        {
            return new MySqlConnection(cadenaConexion);
        }

        public async Task<IEnumerable<Oferente>> ObtenerTodosAsync()
        {
            using var conexion = AbrirConexion();
            return await conexion.QueryAsync<Oferente>(@"
                SELECT id_oferente AS IdOferente, identificacion AS Identificacion,
                       tipo_identificacion AS TipoIdentificacion,
                       nombre_completo AS NombreCompleto, fecha_nacimiento AS FechaNacimiento
                FROM oferente ORDER BY nombre_completo");
        }

        public async Task<Oferente?> ObtenerPorIdAsync(int id)
        {
            using var conexion = AbrirConexion();
            var oferente = await conexion.QueryFirstOrDefaultAsync<Oferente>(@"
                SELECT id_oferente AS IdOferente, identificacion AS Identificacion,
                       tipo_identificacion AS TipoIdentificacion,
                       nombre_completo AS NombreCompleto, fecha_nacimiento AS FechaNacimiento
                FROM oferente WHERE id_oferente = @Id", new { Id = id });

            if (oferente == null) return null;

            oferente.Correos = (await conexion.QueryAsync<string>(
                "SELECT correo FROM oferente_correo WHERE id_oferente = @Id", new { Id = id })).ToList();

            oferente.Telefonos = (await conexion.QueryAsync<string>(
                "SELECT telefono FROM oferente_telefono WHERE id_oferente = @Id", new { Id = id })).ToList();

            oferente.ConcursosIds = (await conexion.QueryAsync<int>(
                "SELECT id_concurso FROM oferente_concurso WHERE id_oferente = @Id", new { Id = id })).ToList();

            return oferente;
        }

        public async Task<bool> IdentificacionExisteAsync(string identificacion, int? idExcluir = null)
        {
            using var conexion = AbrirConexion();
            return await conexion.QueryFirstOrDefaultAsync<int?>(@"
                SELECT id_oferente FROM oferente
                WHERE identificacion = @Identificacion AND id_oferente != @IdExcluir",
                new { Identificacion = identificacion, IdExcluir = idExcluir ?? 0 }) != null;
        }

        public async Task InsertarAsync(Oferente oferente, List<string> correos, List<string> telefonos, List<int> concursosIds)
        {
            using var conexion = AbrirConexion();
            await conexion.OpenAsync();
            using var tran = await conexion.BeginTransactionAsync();

            var newId = await conexion.QuerySingleAsync<int>(@"
                INSERT INTO oferente (identificacion, tipo_identificacion, nombre_completo, fecha_nacimiento)
                VALUES (@Identificacion, @TipoIdentificacion, @NombreCompleto, @FechaNacimiento);
                SELECT LAST_INSERT_ID();", oferente, tran);

            foreach (var correo in correos)
                await conexion.ExecuteAsync(
                    "INSERT INTO oferente_correo (id_oferente, correo) VALUES (@Id, @Correo)",
                    new { Id = newId, Correo = correo }, tran);

            foreach (var tel in telefonos)
                await conexion.ExecuteAsync(
                    "INSERT INTO oferente_telefono (id_oferente, telefono) VALUES (@Id, @Telefono)",
                    new { Id = newId, Telefono = tel }, tran);

            foreach (var cId in concursosIds)
                await conexion.ExecuteAsync(
                    "INSERT INTO oferente_concurso (id_oferente, id_concurso) VALUES (@Id, @ConcursoId)",
                    new { Id = newId, ConcursoId = cId }, tran);

            await tran.CommitAsync();
        }

        public async Task ActualizarAsync(Oferente oferente, List<string> correos, List<string> telefonos, List<int> concursosIds)
        {
            using var conexion = AbrirConexion();
            await conexion.OpenAsync();
            using var tran = await conexion.BeginTransactionAsync();

            await conexion.ExecuteAsync(@"
                UPDATE oferente SET
                    identificacion = @Identificacion,
                    tipo_identificacion = @TipoIdentificacion,
                    nombre_completo = @NombreCompleto,
                    fecha_nacimiento = @FechaNacimiento
                WHERE id_oferente = @IdOferente", oferente, tran);

            await conexion.ExecuteAsync("DELETE FROM oferente_correo WHERE id_oferente = @Id", new { Id = oferente.IdOferente }, tran);
            await conexion.ExecuteAsync("DELETE FROM oferente_telefono WHERE id_oferente = @Id", new { Id = oferente.IdOferente }, tran);
            await conexion.ExecuteAsync("DELETE FROM oferente_concurso WHERE id_oferente = @Id", new { Id = oferente.IdOferente }, tran);

            foreach (var correo in correos)
                await conexion.ExecuteAsync(
                    "INSERT INTO oferente_correo (id_oferente, correo) VALUES (@Id, @Correo)",
                    new { Id = oferente.IdOferente, Correo = correo }, tran);

            foreach (var tel in telefonos)
                await conexion.ExecuteAsync(
                    "INSERT INTO oferente_telefono (id_oferente, telefono) VALUES (@Id, @Telefono)",
                    new { Id = oferente.IdOferente, Telefono = tel }, tran);

            foreach (var cId in concursosIds)
                await conexion.ExecuteAsync(
                    "INSERT INTO oferente_concurso (id_oferente, id_concurso) VALUES (@Id, @ConcursoId)",
                    new { Id = oferente.IdOferente, ConcursoId = cId }, tran);

            await tran.CommitAsync();
        }

        public async Task<bool> TieneRelacionesAsync(int id)
        {
            using var conexion = AbrirConexion();
            var total = await conexion.QuerySingleAsync<int>(@"
                SELECT
                  (SELECT COUNT(*) FROM empleado WHERE id_oferente = @Id) +
                  (SELECT COUNT(*) FROM entrevista WHERE id_oferente = @Id) +
                  (SELECT COUNT(*) FROM preparacion_academica WHERE id_oferente = @Id) +
                  (SELECT COUNT(*) FROM experiencia_laboral WHERE id_oferente = @Id) AS total",
                new { Id = id });
            return total > 0;
        }

        public async Task EliminarAsync(int id)
        {
            using var conexion = AbrirConexion();
            await conexion.OpenAsync();
            using var tran = await conexion.BeginTransactionAsync();
            await conexion.ExecuteAsync("DELETE FROM oferente_correo WHERE id_oferente = @Id", new { Id = id }, tran);
            await conexion.ExecuteAsync("DELETE FROM oferente_telefono WHERE id_oferente = @Id", new { Id = id }, tran);
            await conexion.ExecuteAsync("DELETE FROM oferente_concurso WHERE id_oferente = @Id", new { Id = id }, tran);
            await conexion.ExecuteAsync("DELETE FROM oferente WHERE id_oferente = @Id", new { Id = id }, tran);
            await tran.CommitAsync();
        }

        public async Task<IEnumerable<Concurso>> ObtenerConcursosAsync()
        {
            using var conexion = AbrirConexion();
            return await conexion.QueryAsync<Concurso>(@"
                SELECT id_concurso AS IdConcurso, codigo AS Codigo, nombre AS Nombre
                FROM concurso ORDER BY nombre");
        }
    }
}
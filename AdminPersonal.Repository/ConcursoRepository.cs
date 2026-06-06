using AdminPersonal.Entities;
using Dapper;
using Microsoft.Extensions.Configuration;
using MySqlConnector;

namespace AdminPersonal.Repository
{
    public class ConcursoRepository
    {
        private readonly string cadenaConexion;

        public ConcursoRepository(IConfiguration config)
        {
            cadenaConexion = config.GetConnectionString("DefaultConnection")!;
        }

        private MySqlConnection AbrirConexion() => new MySqlConnection(cadenaConexion);

        public async Task<IEnumerable<Concurso>> ObtenerTodosAsync()
        {
            using var conexion = AbrirConexion();
            return await conexion.QueryAsync<Concurso>(
                @"SELECT id_concurso, codigo, nombre,
                         fecha_inicio AS FechaInicio, fecha_fin AS FechaFin, estado
                  FROM concurso ORDER BY nombre");
        }

        public async Task<IEnumerable<Concurso>> ObtenerVigentesAsync()
        {
            using var conexion = AbrirConexion();
            return await conexion.QueryAsync<Concurso>(
                @"SELECT id_concurso, codigo, nombre,
                         fecha_inicio AS FechaInicio, fecha_fin AS FechaFin, estado
                  FROM concurso WHERE estado='Vigente' ORDER BY nombre");
        }

        public async Task<Concurso?> ObtenerPorIdAsync(int id)
        {
            using var conexion = AbrirConexion();
            return await conexion.QueryFirstOrDefaultAsync<Concurso>(
                @"SELECT id_concurso, codigo, nombre,
                         fecha_inicio AS FechaInicio, fecha_fin AS FechaFin, estado
                  FROM concurso WHERE id_concurso=@Id", new { Id = id });
        }

        public async Task InsertarAsync(Concurso concurso)
        {
            using var conexion = AbrirConexion();
            await conexion.ExecuteAsync(
                @"INSERT INTO concurso (codigo, nombre, fecha_inicio, fecha_fin, estado)
                  VALUES (@Codigo, @Nombre, @FechaInicio, @FechaFin, @Estado)", concurso);
        }

        public async Task ActualizarAsync(Concurso concurso)
        {
            using var conexion = AbrirConexion();
            await conexion.ExecuteAsync(
                @"UPDATE concurso SET codigo=@Codigo, nombre=@Nombre,
                  fecha_inicio=@FechaInicio, fecha_fin=@FechaFin, estado=@Estado
                  WHERE id_concurso=@id_concurso", concurso);
        }

        public async Task EliminarAsync(int id)
        {
            using var conexion = AbrirConexion();
            await conexion.ExecuteAsync("DELETE FROM concurso WHERE id_concurso=@Id", new { Id = id });
        }

        public async Task<bool> TieneRelacionesAsync(int id)
        {
            using var conexion = AbrirConexion();
            var count = await conexion.QueryFirstAsync<int>(
                "SELECT COUNT(*) FROM oferente_concurso WHERE id_concurso=@id", new { id });
            return count > 0;
        }

        public async Task<bool> CodigoExisteAsync(string codigo, int? idExcluir = null)
        {
            using var conexion = AbrirConexion();
            return await conexion.QueryFirstOrDefaultAsync<int?>(
                "SELECT id_concurso FROM concurso WHERE codigo=@codigo AND id_concurso!=@idExcluir",
                new { codigo, idExcluir = idExcluir ?? 0 }) != null;
        }
    }
}
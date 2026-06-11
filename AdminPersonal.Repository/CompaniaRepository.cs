using AdminPersonal.Entities;
using Dapper;
using System.Data;

namespace AdminPersonal.Repository
{
    public class CompaniaRepository
    {
        private readonly IDbConnectionFactory _dbFactory;

        public CompaniaRepository(IDbConnectionFactory dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public async Task<IEnumerable<Compania>> ObtenerTodosAsync()
        {
            using var conexion = _dbFactory.CrearConexion();
            return await conexion.QueryAsync<Compania>("SELECT id_compania, codigo, nombre FROM compania");
        }

        public async Task<Compania?> ObtenerPorIdAsync(int id)
        {
            using var conexion = _dbFactory.CrearConexion();
            return await conexion.QueryFirstOrDefaultAsync<Compania>("SELECT id_compania, codigo, nombre FROM compania WHERE id_compania = @Id", new { Id = id });
        }

        public async Task InsertarAsync(Compania compania)
        {
            using var conexion = _dbFactory.CrearConexion();
            await conexion.ExecuteAsync("INSERT INTO compania (codigo, nombre) VALUES (@Codigo, @Nombre)", compania);
        }

        public async Task ActualizarAsync(Compania compania)
        {
            using var conexion = _dbFactory.CrearConexion();
            await conexion.ExecuteAsync("UPDATE compania SET codigo = @Codigo, nombre = @Nombre WHERE id_compania = @id_compania", compania);
        }

        public async Task EliminarAsync(int id)
        {
            using var conexion = _dbFactory.CrearConexion();
            await conexion.ExecuteAsync("DELETE FROM compania WHERE id_compania = @Id", new { Id = id });
        }

        public async Task<bool> CodigoExisteAsync(string codigo, int? idExcluir = null)
        {
            using var conexion = _dbFactory.CrearConexion();
            return await conexion.QueryFirstOrDefaultAsync<Compania>("SELECT id_compania FROM compania WHERE codigo = @Codigo AND id_compania != @IdExcluir", new { Codigo = codigo, IdExcluir = idExcluir ?? 0 }) != null;
        }
    }
}
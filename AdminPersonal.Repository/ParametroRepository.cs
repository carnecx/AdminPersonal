using AdminPersonal.Entities;
using Dapper;

namespace AdminPersonal.Repository
{
    public class ParametroRepository
    {
        private readonly IDbConnectionFactory _dbFactory;

        public ParametroRepository(IDbConnectionFactory dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public async Task<IEnumerable<Parametro>> ObtenerTodosAsync()
        {
            using var conexion = _dbFactory.CrearConexion();return await conexion.QueryAsync<Parametro>("SELECT id_parametro, codigo, valor FROM parametro");
        }

        public async Task<Parametro?> ObtenerPorIdAsync(int id)
        {
            using var conexion = _dbFactory.CrearConexion();
            return await conexion.QueryFirstOrDefaultAsync<Parametro>("SELECT id_parametro, codigo, valor FROM parametro WHERE id_parametro = @Id",new { Id = id });
        }

        public async Task InsertarAsync(Parametro parametro)
        {
            using var conexion = _dbFactory.CrearConexion();
            await conexion.ExecuteAsync("INSERT INTO parametro (codigo, valor) VALUES (@Codigo, @Valor)",parametro);
        }

        public async Task ActualizarAsync(Parametro parametro)
        {
            using var conexion = _dbFactory.CrearConexion();
            await conexion.ExecuteAsync("UPDATE parametro SET codigo = @Codigo, valor = @Valor WHERE id_parametro = @id_parametro",parametro);
        }

        public async Task EliminarAsync(int id)
        {
            using var conexion = _dbFactory.CrearConexion();
            await conexion.ExecuteAsync("DELETE FROM parametro WHERE id_parametro = @Id",new { Id = id });
        }

        public async Task<bool> CodigoExisteAsync(string codigo, int? idExcluir = null)
        {
            using var conexion = _dbFactory.CrearConexion();
            return await conexion.QueryFirstOrDefaultAsync<Parametro>("SELECT id_parametro FROM parametro WHERE codigo = @Codigo AND id_parametro != @IdExcluir",new { Codigo = codigo, IdExcluir = idExcluir ?? 0 }) != null;
        }
    }
}
using AdminPersonal.Entities;
using AdminPersonal.Repository;
using AdminPersonal.Services.Abstract;

namespace AdminPersonal.Services
{
    public class ParametroService : IParametroService
    {
        private readonly ParametroRepository _repositorio;

        public ParametroService(ParametroRepository repositorio)
        {
            _repositorio = repositorio;
        }

        public async Task<IEnumerable<Parametro>> ObtenerTodosAsync()
        {
            return await _repositorio.ObtenerTodosAsync();
        }

        public async Task<Parametro?> ObtenerPorIdAsync(int id)
        {
            return await _repositorio.ObtenerPorIdAsync(id);
        }

        public async Task InsertarAsync(Parametro parametro)
        {
            await _repositorio.InsertarAsync(parametro);
        }

        public async Task ActualizarAsync(Parametro parametro)
        {
            await _repositorio.ActualizarAsync(parametro);
        }

        public async Task EliminarAsync(int id)
        {
            await _repositorio.EliminarAsync(id);
        }

        public async Task<bool> CodigoExisteAsync(string codigo, int? idExcluir = null)
        {
            return await _repositorio.CodigoExisteAsync(codigo, idExcluir);
        }
    }
}
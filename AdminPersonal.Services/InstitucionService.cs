using AdminPersonal.Entities;
using AdminPersonal.Repository;

namespace AdminPersonal.Services
{
    public class InstitucionService
    {
        private readonly InstitucionRepository _repositorio;

        public InstitucionService(InstitucionRepository repositorio)
        {
            _repositorio = repositorio;
        }

        public async Task<IEnumerable<InstitucionEducativa>> ObtenerTodosAsync()
        {
            return await _repositorio.ObtenerTodosAsync();
        }

        public async Task<InstitucionEducativa?> ObtenerPorIdAsync(int id)
        {
            return await _repositorio.ObtenerPorIdAsync(id);
        }

        public async Task InsertarAsync(InstitucionEducativa item)
        {
            await _repositorio.InsertarAsync(item);
        }

        public async Task ActualizarAsync(InstitucionEducativa item)
        {
            await _repositorio.ActualizarAsync(item);
        }

        public async Task EliminarAsync(int id)
        {
            await _repositorio.EliminarAsync(id);
        }
    }
}

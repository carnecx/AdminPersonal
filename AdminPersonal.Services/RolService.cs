using AdminPersonal.Entities;
using AdminPersonal.Repository;
using AdminPersonal.Services.Abstract;

namespace AdminPersonal.Services
{
    public class RolService : IRolService
    {
        private readonly RolRepository _repositorio;

        public RolService(RolRepository repositorio)
        {
            _repositorio = repositorio;
        }

        public async Task<IEnumerable<Rol>> ObtenerTodosAsync()
        {
            return await _repositorio.ObtenerTodosAsync();
        }

        public async Task<Rol?> ObtenerPorIdAsync(int id)
        {
            return await _repositorio.ObtenerPorIdAsync(id);
        }

        public async Task InsertarAsync(Rol rol)
        {
            await _repositorio.InsertarAsync(rol);
        }

        public async Task ActualizarAsync(Rol rol)
        {
            await _repositorio.ActualizarAsync(rol);
        }

        public async Task EliminarAsync(int id)
        {
            await _repositorio.EliminarAsync(id);
        }

        public async Task<bool> NombreExisteAsync(string nombre, int? idExcluir = null)
        {
            return await _repositorio.NombreExisteAsync(nombre, idExcluir);
        }

        public async Task<bool> EstaAsignadoAUsuarioAsync(int id)
        {
            return await _repositorio.EstaAsignadoAUsuarioAsync(id);
        }
    }
}
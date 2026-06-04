using AdminPersonal.Entities;
using AdminPersonal.Repository;

namespace AdminPersonal.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _repo;

        public UsuarioService(IUsuarioRepository repo)
        {
            _repo = repo;
        }

        public Task<IEnumerable<Usuario>> ObtenerTodosAsync() => _repo.ObtenerTodosAsync();
        public Task<Usuario?> ObtenerPorIdAsync(int id) => _repo.ObtenerPorIdAsync(id);
        public Task<int> CrearAsync(Usuario u, int idUsuarioSesion) => _repo.CrearAsync(u, idUsuarioSesion);
        public Task ActualizarAsync(Usuario u, int idUsuarioSesion) => _repo.ActualizarAsync(u, idUsuarioSesion);
        public Task<(bool ok, string mensaje)> EliminarAsync(int id, int idUsuarioSesion) => _repo.EliminarAsync(id, idUsuarioSesion);
        public Task CambiarEstadoAsync(int id, string nuevoEstado, int idUsuarioSesion) => _repo.CambiarEstadoAsync(id, nuevoEstado, idUsuarioSesion);
        public Task<IEnumerable<Rol>> ObtenerRolesAsync() => _repo.ObtenerRolesAsync();
    }
}
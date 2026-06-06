using AdminPersonal.Entities;
using AdminPersonal.Repository;
using AdminPersonal.Services.Abstract;

namespace AdminPersonal.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _repo;
        private readonly PasswordService _passwordService;

        public UsuarioService(IUsuarioRepository repo, PasswordService passwordService)
        {
            _repo = repo;
            _passwordService = passwordService;
        }

        // ── Métodos del Login (compañero) ──────────────────────────────────
        public async Task<Usuario?> BuscarPorUsuarioAsync(string nombreUsuario)
            => await _repo.BuscarPorUsuarioAsync(nombreUsuario);

        public bool ValidarPassword(string contrasenaDigitada, string contrasenaBD)
            => _passwordService.Verify(contrasenaDigitada, contrasenaBD);

        public async Task RegistrarFalloAsync(Usuario usuario)
            => await _repo.RegistrarFalloAsync(usuario);

        public async Task ReiniciarIntentosAsync(int idUsuario)
            => await _repo.ReiniciarIntentosAsync(idUsuario);

        public async Task<string?> ObtenerRolAsync(int idUsuario)
            => await _repo.ObtenerRolAsync(idUsuario);

        public async Task<int?> ObtenerIdRolAsync(int idUsuario)
            => await _repo.ObtenerIdRolAsync(idUsuario);

        // ── Métodos del SEG6 (tuyos) ───────────────────────────────────────
        public Task<IEnumerable<Usuario>> ObtenerTodosAsync() => _repo.ObtenerTodosAsync();
        public Task<Usuario?> ObtenerPorIdAsync(int id) => _repo.ObtenerPorIdAsync(id);
        public Task<int> CrearAsync(Usuario u, int idUsuarioSesion) => _repo.CrearAsync(u, idUsuarioSesion);
        public Task ActualizarAsync(Usuario u, int idUsuarioSesion) => _repo.ActualizarAsync(u, idUsuarioSesion);
        public Task<(bool ok, string mensaje)> EliminarAsync(int id, int idUsuarioSesion) => _repo.EliminarAsync(id, idUsuarioSesion);
        public Task CambiarEstadoAsync(int id, string nuevoEstado, int idUsuarioSesion) => _repo.CambiarEstadoAsync(id, nuevoEstado, idUsuarioSesion);
        public Task<IEnumerable<Rol>> ObtenerRolesAsync() => _repo.ObtenerRolesAsync();
    }
}
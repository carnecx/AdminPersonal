using AdminPersonal.Entities;
using AdminPersonal.Repository;
using AdminPersonal.Services.Abstract;
using System.Text.RegularExpressions;

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

        public async Task<Usuario?> BuscarPorUsuarioAsync(string nombreUsuario)
        {
            if (string.IsNullOrWhiteSpace(nombreUsuario))
                return null;

            return await _repo.BuscarPorUsuarioAsync(nombreUsuario);
        }

        public bool ValidarPassword(string contrasenaDigitada, string contrasenaBD)
        {
            if (string.IsNullOrWhiteSpace(contrasenaDigitada))
                return false;

            if (string.IsNullOrWhiteSpace(contrasenaBD))
                return false;

            return _passwordService.Verify(contrasenaDigitada, contrasenaBD);
        }

        public async Task RegistrarFalloAsync(Usuario usuario)
        {
            if (usuario == null)
                throw new Exception("El usuario es requerido.");

            await _repo.RegistrarFalloAsync(usuario);
        }

        public async Task ReiniciarIntentosAsync(int idUsuario)
        {
            if (idUsuario <= 0)
                throw new Exception("El id del usuario no es valido.");

            await _repo.ReiniciarIntentosAsync(idUsuario);
        }

        public async Task<string?> ObtenerRolAsync(int idUsuario)
        {
            if (idUsuario <= 0)
                return null;

            return await _repo.ObtenerRolAsync(idUsuario);
        }

        public async Task<int?> ObtenerIdRolAsync(int idUsuario)
        {
            if (idUsuario <= 0)
                return null;

            return await _repo.ObtenerIdRolAsync(idUsuario);
        }

        public Task<IEnumerable<Usuario>> ObtenerTodosAsync()
        {
            return _repo.ObtenerTodosAsync();
        }

        public async Task<Usuario?> ObtenerPorIdAsync(int id)
        {
            if (id <= 0)
                throw new Exception("El id del usuario no es valido.");

            return await _repo.ObtenerPorIdAsync(id);
        }

        public async Task<int> CrearAsync(Usuario u, int idUsuarioSesion)
        {
            ValidarUsuario(u, esCreacion: true);

            return await _repo.CrearAsync(u, idUsuarioSesion);
        }

        public async Task ActualizarAsync(Usuario u, int idUsuarioSesion)
        {
            if (u.IdUsuario <= 0)
                throw new Exception("El id del usuario no es valido.");

            ValidarUsuario(u, esCreacion: false);

            await _repo.ActualizarAsync(u, idUsuarioSesion);
        }

        public async Task<(bool ok, string mensaje)> EliminarAsync(int id, int idUsuarioSesion)
        {
            if (id <= 0)
                return (false, "El id del usuario no es valido.");

            return await _repo.EliminarAsync(id, idUsuarioSesion);
        }

        public async Task CambiarEstadoAsync(int id, string nuevoEstado, int idUsuarioSesion)
        {
            if (id <= 0)
                throw new Exception("El id del usuario no es valido.");

            if (nuevoEstado != "Activo" &&
                nuevoEstado != "Inactivo" &&
                nuevoEstado != "Bloqueado")
                throw new Exception("El estado indicado no es valido.");

            await _repo.CambiarEstadoAsync(id, nuevoEstado, idUsuarioSesion);
        }

        public Task<IEnumerable<Rol>> ObtenerRolesAsync()
        {
            return _repo.ObtenerRolesAsync();
        }

        private void ValidarUsuario(Usuario u, bool esCreacion)
        {
            if (u == null)
                throw new Exception("Los datos del usuario son requeridos.");

            if (string.IsNullOrWhiteSpace(u.NombreUsuario))
                throw new Exception("El nombre de usuario es requerido.");

            if (string.IsNullOrWhiteSpace(u.NombreCompleto))
                throw new Exception("El nombre completo es requerido.");

            if (string.IsNullOrWhiteSpace(u.Correo))
                throw new Exception("El correo es requerido.");

            if (!Regex.IsMatch(u.Correo, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                throw new Exception("El correo no tiene un formato valido.");

            if (esCreacion && string.IsNullOrWhiteSpace(u.Contrasena))
                throw new Exception("La contrasena es requerida.");

            if (u.RolesSeleccionados == null || !u.RolesSeleccionados.Any())
                throw new Exception("Debe seleccionar al menos un rol.");

            if (u.Estado != "Activo" &&
                u.Estado != "Inactivo" &&
                u.Estado != "Bloqueado")
                throw new Exception("El estado del usuario no es valido.");
        }
    }
}
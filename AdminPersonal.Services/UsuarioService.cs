using AdminPersonal.Entities;
using AdminPersonal.Repository;

namespace AdminPersonal.Services
{
    public class UsuarioService
    {
        private readonly UsuarioRepository _repositorio;
        private readonly PasswordService _passwordService;

        public UsuarioService(UsuarioRepository repositorio, PasswordService passwordService)
        {
            _repositorio = repositorio;
            _passwordService = passwordService;
        }

        public async Task<Usuario?> BuscarPorUsuarioAsync(string nombreUsuario)
        {
            return await _repositorio.BuscarPorUsuarioAsync(nombreUsuario);
        }

        public bool ValidarPassword(string contrasenaDigitada, string contrasenaBD)
        {
            return _passwordService.Verify(contrasenaDigitada, contrasenaBD);
        }

        public async Task RegistrarFalloAsync(Usuario usuario)
        {
            int nuevosIntentos = usuario.intentos_fallidos + 1;
            string nuevoEstado = nuevosIntentos >= 3 ? "Bloqueado" : usuario.estado;
            await _repositorio.RegistrarFalloAsync(usuario.id_usuario, nuevosIntentos, nuevoEstado);
        }

        public async Task ReiniciarIntentosAsync(int idUsuario)
        {
            await _repositorio.ReiniciarIntentosAsync(idUsuario);
        }

        public async Task<string?> ObtenerRolAsync(int idUsuario)
        {
            return await _repositorio.ObtenerRolAsync(idUsuario);
        }

        // NUEVO
        public async Task<int?> ObtenerIdRolAsync(int idUsuario)
        {
            return await _repositorio.ObtenerIdRolAsync(idUsuario);
        }
    }
}

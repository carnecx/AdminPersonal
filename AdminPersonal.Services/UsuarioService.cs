using AdminPersonal.Entities;
using AdminPersonal.Repository;
using AdminPersonal.Services.Abstract;

namespace AdminPersonal.Services
{
    // servicio encargado de manejar la logica de negocio relacionada con usuarios
    public class UsuarioService : IUsuarioService
    {
        // referencia al repositorio de usuarios
        private readonly IUsuarioRepository _repo;

        // servicio encargado de la validacion y manejo de contraseñas
        private readonly PasswordService _passwordService;

        // constructor que recibe las dependencias mediante inyeccion
        public UsuarioService(IUsuarioRepository repo, PasswordService passwordService)
        {
            _repo = repo;
            _passwordService = passwordService;
        }

        // metodos del Login

        // busca un usuario por nombre de usuario
        public async Task<Usuario?> BuscarPorUsuarioAsync(string nombreUsuario)
            => await _repo.BuscarPorUsuarioAsync(nombreUsuario);

        // valida que la contraseña digitada coincida con la almacenada
        public bool ValidarPassword(string contrasenaDigitada, string contrasenaBD)
            => _passwordService.Verify(contrasenaDigitada, contrasenaBD);

        // registra un intento fallido de inicio de sesion
        public async Task RegistrarFalloAsync(Usuario usuario)
            => await _repo.RegistrarFalloAsync(usuario);

        // reinicia el contador de intentos fallidos despues de un login exitoso
        public async Task ReiniciarIntentosAsync(int idUsuario)
            => await _repo.ReiniciarIntentosAsync(idUsuario);

        // obtiene el nombre del rol asociado al usuario
        public async Task<string?> ObtenerRolAsync(int idUsuario)
            => await _repo.ObtenerRolAsync(idUsuario);

        // obtiene el identificador del rol asociado al usuario
        public async Task<int?> ObtenerIdRolAsync(int idUsuario)
            => await _repo.ObtenerIdRolAsync(idUsuario);

        // metodos de mantenimiento de usuarios 

        // obtiene todos los usuarios registrados
        public Task<IEnumerable<Usuario>> ObtenerTodosAsync()
            => _repo.ObtenerTodosAsync();

        // obtiene un usuario especifico por id
        public Task<Usuario?> ObtenerPorIdAsync(int id)
            => _repo.ObtenerPorIdAsync(id);

        // crea un nuevo usuario
        public Task<int> CrearAsync(Usuario u, int idUsuarioSesion)
            => _repo.CrearAsync(u, idUsuarioSesion);

        // actualiza la informacion de un usuario
        public Task ActualizarAsync(Usuario u, int idUsuarioSesion)
            => _repo.ActualizarAsync(u, idUsuarioSesion);

        // elimina un usuario si no posee datos relacionados
        public Task<(bool ok, string mensaje)> EliminarAsync(int id, int idUsuarioSesion)
            => _repo.EliminarAsync(id, idUsuarioSesion);

        // cambia el estado del usuario
        // activo, inactivo o bloqueado
        public Task CambiarEstadoAsync(int id, string nuevoEstado, int idUsuarioSesion)
            => _repo.CambiarEstadoAsync(id, nuevoEstado, idUsuarioSesion);

        // obtiene la lista de roles disponibles
        public Task<IEnumerable<Rol>> ObtenerRolesAsync()
            => _repo.ObtenerRolesAsync();
    }
}
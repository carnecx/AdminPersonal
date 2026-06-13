using AdminPersonal.Entities;

namespace AdminPersonal.Services
{
    // interfaz que define las operaciones disponibles para la gestion de usuarios
    public interface IUsuarioService
    {
        // busca un usuario por su nombre de usuario
        // se utiliza durante el proceso de login
        Task<Usuario?> BuscarPorUsuarioAsync(string nombreUsuario);

        // valida si la contraseña digitada coincide con la almacenada
        bool ValidarPassword(string contrasenaDigitada, string contrasenaBD);

        // registra un intento fallido de inicio de sesion
        Task RegistrarFalloAsync(Usuario usuario);

        // reinicia el contador de intentos fallidos
        // se ejecuta cuando el login es exitoso
        Task ReiniciarIntentosAsync(int idUsuario);

        // obtiene el nombre del rol asignado al usuario
        Task<string?> ObtenerRolAsync(int idUsuario);

        // obtiene el id del rol asignado al usuario
        Task<int?> ObtenerIdRolAsync(int idUsuario);

        // obtiene todos los usuarios registrados
        Task<IEnumerable<Usuario>> ObtenerTodosAsync();

        // obtiene un usuario especifico por su id
        Task<Usuario?> ObtenerPorIdAsync(int id);

        // crea un nuevo usuario en la base de datos
        Task<int> CrearAsync(
            Usuario u,
            int idUsuarioSesion);

        // actualiza la informacion de un usuario existente
        Task ActualizarAsync(
            Usuario u,
            int idUsuarioSesion);

        // elimina un usuario
        // retorna si la operacion fue exitosa y un mensaje descriptivo
        Task<(bool ok, string mensaje)> EliminarAsync(
            int id,
            int idUsuarioSesion);

        // cambia el estado del usuario
        // por ejemplo activo, inactivo o bloqueado
        Task CambiarEstadoAsync(
            int id,
            string nuevoEstado,
            int idUsuarioSesion);

        // obtiene todos los roles disponibles
        Task<IEnumerable<Rol>> ObtenerRolesAsync();
    }
}
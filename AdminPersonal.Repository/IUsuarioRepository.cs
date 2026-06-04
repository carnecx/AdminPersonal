using AdminPersonal.Entities;

namespace AdminPersonal.Repository
{
    public interface IUsuarioRepository
    {
        Task<IEnumerable<Usuario>> ObtenerTodosAsync();
        Task<Usuario?> ObtenerPorIdAsync(int id);
        Task<int> CrearAsync(Usuario u, int idUsuarioSesion);
        Task ActualizarAsync(Usuario u, int idUsuarioSesion);
        Task<(bool ok, string mensaje)> EliminarAsync(int id, int idUsuarioSesion);
        Task CambiarEstadoAsync(int id, string nuevoEstado, int idUsuarioSesion);
        Task<IEnumerable<Rol>> ObtenerRolesAsync();
    }
}
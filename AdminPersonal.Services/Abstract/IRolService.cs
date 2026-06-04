using AdminPersonal.Entities;

namespace AdminPersonal.Services.Abstract
{
    public interface IRolService
    {
        Task<IEnumerable<Rol>> ObtenerTodosAsync();
        Task<Rol?> ObtenerPorIdAsync(int id);
        Task InsertarAsync(Rol rol);
        Task ActualizarAsync(Rol rol);
        Task EliminarAsync(int id);
        Task<bool> NombreExisteAsync(string nombre, int? idExcluir = null);
        Task<bool> EstaAsignadoAUsuarioAsync(int id);
    }
}
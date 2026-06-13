using AdminPersonal.Entities;

namespace AdminPersonal.Services.Abstract
{
    public interface IRolService
    {
        Task<IEnumerable<Rol>> ObtenerTodosAsync();
        Task<Rol?> ObtenerPorIdAsync(int id);
        Task<string?> ValidarYCrearAsync(Rol rol);
        Task<string?> ValidarYActualizarAsync(Rol rol);
        Task<string?> EliminarAsync(int id);
    }
}
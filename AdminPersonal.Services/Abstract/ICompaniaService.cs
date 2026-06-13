using AdminPersonal.Entities;

namespace AdminPersonal.Services.Abstract
{
    public interface ICompaniaService
    {
        Task<IEnumerable<Compania>> ObtenerTodosAsync();
        Task<Compania?> ObtenerPorIdAsync(int id);
        Task<string?> ValidarYCrearAsync(Compania compania);
        Task<string?> ValidarYActualizarAsync(Compania compania);
        Task EliminarAsync(int id);
    }
}
using AdminPersonal.Entities;

namespace AdminPersonal.Services.Abstract
{
    public interface IParametroService
    {
        Task<IEnumerable<Parametro>> ObtenerTodosAsync();
        Task<Parametro?> ObtenerPorIdAsync(int id);
        Task<string?> ValidarYCrearAsync(Parametro parametro);
        Task<string?> ValidarYActualizarAsync(Parametro parametro);
        Task EliminarAsync(int id);
    }
}
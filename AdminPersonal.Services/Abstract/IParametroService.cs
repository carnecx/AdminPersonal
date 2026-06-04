using AdminPersonal.Entities;

namespace AdminPersonal.Services.Abstract
{
    public interface IParametroService
    {
        Task<IEnumerable<Parametro>> ObtenerTodosAsync();
        Task<Parametro?> ObtenerPorIdAsync(int id);
        Task InsertarAsync(Parametro parametro);
        Task ActualizarAsync(Parametro parametro);
        Task EliminarAsync(int id);
        Task<bool> CodigoExisteAsync(string codigo, int? idExcluir = null);
    }
}
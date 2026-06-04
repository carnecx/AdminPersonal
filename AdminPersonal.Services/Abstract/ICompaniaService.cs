using AdminPersonal.Entities;

namespace AdminPersonal.Services.Abstract
{
    public interface ICompaniaService
    {
        Task<IEnumerable<Compania>> ObtenerTodosAsync();
        Task<Compania?> ObtenerPorIdAsync(int id);
        Task InsertarAsync(Compania compania);
        Task ActualizarAsync(Compania compania);
        Task EliminarAsync(int id);
        Task<bool> CodigoExisteAsync(string codigo, int? idExcluir = null);
    }
}

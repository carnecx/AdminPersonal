using AdminPersonal.Entities;

namespace AdminPersonal.Services.Abstract
{
    public interface IOferenteService
    {
        Task<IEnumerable<Oferente>> ObtenerTodosAsync();
        Task<Oferente?> ObtenerPorIdAsync(int id);
        Task<IEnumerable<string>> ObtenerCorreosAsync(int idOferente);
        Task<IEnumerable<string>> ObtenerTelefonosAsync(int idOferente);
        Task<IEnumerable<int>> ObtenerConcursosIdsAsync(int idOferente);
        Task<int> InsertarAsync(Oferente oferente);
        Task ActualizarAsync(Oferente oferente);
        Task EliminarAsync(int id);
        Task<bool> TieneRelacionesAsync(int id);
        Task<bool> IdentificacionExisteAsync(string identificacion, int? idExcluir = null);
    }
}
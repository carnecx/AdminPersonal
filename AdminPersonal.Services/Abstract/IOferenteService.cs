using AdminPersonal.Entities;

namespace AdminPersonal.Services.Abstract
{
    public interface IOferenteService
    {
        Task<IEnumerable<Oferente>> ObtenerTodosAsync();
        Task<Oferente?> ObtenerPorIdAsync(int id);
        Task InsertarAsync(Oferente oferente, List<string> correos, List<string> telefonos, List<int> concursosIds);
        Task ActualizarAsync(Oferente oferente, List<string> correos, List<string> telefonos, List<int> concursosIds);
        Task EliminarAsync(int id);
        Task<bool> IdentificacionExisteAsync(string identificacion, int? idExcluir = null);
        Task<bool> TieneRelacionesAsync(int id);
        Task<IEnumerable<Concurso>> ObtenerConcursosAsync();
    }
}
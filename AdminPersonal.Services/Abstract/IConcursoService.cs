using AdminPersonal.Entities;

namespace AdminPersonal.Services.Abstract
{
    public interface IConcursoService
    {
        Task<IEnumerable<Concurso>> ObtenerTodosAsync();
        Task<IEnumerable<Concurso>> ObtenerVigentesAsync();
        Task<Concurso?> ObtenerPorIdAsync(int id);
        Task InsertarAsync(Concurso concurso);
        Task ActualizarAsync(Concurso concurso);
        Task EliminarAsync(int id);
        Task<bool> TieneRelacionesAsync(int id);
        Task<bool> CodigoExisteAsync(string codigo, int? idExcluir = null);
    }
}
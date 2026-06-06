using AdminPersonal.Entities;
using AdminPersonal.Repository;
using AdminPersonal.Services.Abstract;

namespace AdminPersonal.Services
{
    public class ConcursoService : IConcursoService
    {
        private readonly ConcursoRepository _repositorio;
        public ConcursoService(ConcursoRepository repositorio) => _repositorio = repositorio;

        public async Task<IEnumerable<Concurso>> ObtenerTodosAsync() => await _repositorio.ObtenerTodosAsync();
        public async Task<IEnumerable<Concurso>> ObtenerVigentesAsync() => await _repositorio.ObtenerVigentesAsync();
        public async Task<Concurso?> ObtenerPorIdAsync(int id) => await _repositorio.ObtenerPorIdAsync(id);
        public async Task InsertarAsync(Concurso c) => await _repositorio.InsertarAsync(c);
        public async Task ActualizarAsync(Concurso c) => await _repositorio.ActualizarAsync(c);
        public async Task EliminarAsync(int id) => await _repositorio.EliminarAsync(id);
        public async Task<bool> TieneRelacionesAsync(int id) => await _repositorio.TieneRelacionesAsync(id);
        public async Task<bool> CodigoExisteAsync(string codigo, int? idExcluir = null)
            => await _repositorio.CodigoExisteAsync(codigo, idExcluir);
    }
}
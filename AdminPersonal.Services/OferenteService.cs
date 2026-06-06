using AdminPersonal.Entities;
using AdminPersonal.Repository;
using AdminPersonal.Services.Abstract;

namespace AdminPersonal.Services
{
    public class OferenteService : IOferenteService
    {
        private readonly OferenteRepository _repositorio;
        public OferenteService(OferenteRepository repositorio) => _repositorio = repositorio;

        public async Task<IEnumerable<Oferente>> ObtenerTodosAsync() => await _repositorio.ObtenerTodosAsync();
        public async Task<Oferente?> ObtenerPorIdAsync(int id) => await _repositorio.ObtenerPorIdAsync(id);
        public async Task<IEnumerable<string>> ObtenerCorreosAsync(int id) => await _repositorio.ObtenerCorreosAsync(id);
        public async Task<IEnumerable<string>> ObtenerTelefonosAsync(int id) => await _repositorio.ObtenerTelefonosAsync(id);
        public async Task<IEnumerable<int>> ObtenerConcursosIdsAsync(int id) => await _repositorio.ObtenerConcursosIdsAsync(id);
        public async Task<int> InsertarAsync(Oferente oferente) => await _repositorio.InsertarAsync(oferente);
        public async Task ActualizarAsync(Oferente oferente) => await _repositorio.ActualizarAsync(oferente);
        public async Task EliminarAsync(int id) => await _repositorio.EliminarAsync(id);
        public async Task<bool> TieneRelacionesAsync(int id) => await _repositorio.TieneRelacionesAsync(id);
        public async Task<bool> IdentificacionExisteAsync(string identificacion, int? idExcluir = null)
            => await _repositorio.IdentificacionExisteAsync(identificacion, idExcluir);
    }
}
using AdminPersonal.Entities;
using AdminPersonal.Repository;
using AdminPersonal.Services.Abstract;

namespace AdminPersonal.Services
{
    public class OferenteService : IOferenteService
    {
        private readonly OferenteRepository _repositorio;

        public OferenteService(OferenteRepository repositorio)
        {
            _repositorio = repositorio;
        }

        public async Task<IEnumerable<Oferente>> ObtenerTodosAsync()
            => await _repositorio.ObtenerTodosAsync();

        public async Task<Oferente?> ObtenerPorIdAsync(int id)
            => await _repositorio.ObtenerPorIdAsync(id);

        public async Task InsertarAsync(Oferente oferente, List<string> correos, List<string> telefonos, List<int> concursosIds)
            => await _repositorio.InsertarAsync(oferente, correos, telefonos, concursosIds);

        public async Task ActualizarAsync(Oferente oferente, List<string> correos, List<string> telefonos, List<int> concursosIds)
            => await _repositorio.ActualizarAsync(oferente, correos, telefonos, concursosIds);

        public async Task EliminarAsync(int id)
            => await _repositorio.EliminarAsync(id);

        public async Task<bool> IdentificacionExisteAsync(string identificacion, int? idExcluir = null)
            => await _repositorio.IdentificacionExisteAsync(identificacion, idExcluir);

        public async Task<bool> TieneRelacionesAsync(int id)
            => await _repositorio.TieneRelacionesAsync(id);

        public async Task<IEnumerable<Concurso>> ObtenerConcursosAsync()
            => await _repositorio.ObtenerConcursosAsync();
    }
}
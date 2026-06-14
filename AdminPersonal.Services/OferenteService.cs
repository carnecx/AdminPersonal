using AdminPersonal.Entities;
using AdminPersonal.Repository;
using AdminPersonal.Services.Abstract;
using System.Text.RegularExpressions;

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
        public async Task EliminarAsync(int id) => await _repositorio.EliminarAsync(id);
        public async Task<bool> TieneRelacionesAsync(int id) => await _repositorio.TieneRelacionesAsync(id);
        public async Task<bool> IdentificacionExisteAsync(string identificacion, int? idExcluir = null)
            => await _repositorio.IdentificacionExisteAsync(identificacion, idExcluir);

        public async Task<string?> ValidarYCrearAsync(Oferente oferente)
        {
            if (string.IsNullOrWhiteSpace(oferente.Identificacion))
                return "La identificación es obligatoria.";
            if (string.IsNullOrWhiteSpace(oferente.NombreCompleto))
                return "El nombre completo es obligatorio.";
            if (oferente.FechaNacimiento == default)
                return "La fecha de nacimiento es obligatoria.";
            if (oferente.Correos == null || !oferente.Correos.Any(c => !string.IsNullOrWhiteSpace(c)))
                return "Debe indicar al menos un correo electrónico.";
            foreach (var correo in oferente.Correos.Where(c => !string.IsNullOrWhiteSpace(c)))
                if (!Regex.IsMatch(correo, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                    return $"El correo '{correo}' no tiene un formato válido.";
            if (oferente.Telefonos == null || !oferente.Telefonos.Any(t => !string.IsNullOrWhiteSpace(t)))
                return "Debe indicar al menos un teléfono de contacto.";
            if (oferente.ConcursosIds == null || !oferente.ConcursosIds.Any())
                return "Debe seleccionar al menos un concurso.";
            if (await _repositorio.IdentificacionExisteAsync(oferente.Identificacion))
                return "Ya existe un oferente con esa identificación.";

            await _repositorio.InsertarAsync(oferente);
            return null;
        }

        public async Task<string?> ValidarYActualizarAsync(Oferente oferente)
        {
            if (string.IsNullOrWhiteSpace(oferente.Identificacion))
                return "La identificación es obligatoria.";
            if (string.IsNullOrWhiteSpace(oferente.NombreCompleto))
                return "El nombre completo es obligatorio.";
            if (oferente.FechaNacimiento == default)
                return "La fecha de nacimiento es obligatoria.";
            if (oferente.Correos == null || !oferente.Correos.Any(c => !string.IsNullOrWhiteSpace(c)))
                return "Debe indicar al menos un correo electrónico.";
            foreach (var correo in oferente.Correos.Where(c => !string.IsNullOrWhiteSpace(c)))
                if (!Regex.IsMatch(correo, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                    return $"El correo '{correo}' no tiene un formato válido.";
            if (oferente.Telefonos == null || !oferente.Telefonos.Any(t => !string.IsNullOrWhiteSpace(t)))
                return "Debe indicar al menos un teléfono de contacto.";
            if (oferente.ConcursosIds == null || !oferente.ConcursosIds.Any())
                return "Debe seleccionar al menos un concurso.";
            if (await _repositorio.IdentificacionExisteAsync(oferente.Identificacion, oferente.id_oferente))
                return "Ya existe un oferente con esa identificación.";

            await _repositorio.ActualizarAsync(oferente);
            return null;
        }

        public async Task<int> InsertarAsync(Oferente oferente) => await _repositorio.InsertarAsync(oferente);
        public async Task ActualizarAsync(Oferente oferente) => await _repositorio.ActualizarAsync(oferente);
    }
}
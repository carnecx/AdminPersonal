using AdminPersonal.Entities;
using AdminPersonal.Repository;
using AdminPersonal.Services.Abstract;

namespace AdminPersonal.Services
{
    public class ParametroService : IParametroService
    {
        private readonly ParametroRepository _repositorio;

        public ParametroService(ParametroRepository repositorio)
        {
            _repositorio = repositorio;
        }

        public async Task<IEnumerable<Parametro>> ObtenerTodosAsync()
            => await _repositorio.ObtenerTodosAsync();

        public async Task<Parametro?> ObtenerPorIdAsync(int id)
            => await _repositorio.ObtenerPorIdAsync(id);

        public async Task<string?> ValidarYCrearAsync(Parametro parametro)
        {
            if (string.IsNullOrWhiteSpace(parametro.Codigo))
                return "El código es obligatorio.";

            if (string.IsNullOrWhiteSpace(parametro.Valor))
                return "El valor es obligatorio.";

            if (parametro.Valor.Length > 500)
                return "El valor no puede superar los 500 caracteres.";

            if (await _repositorio.CodigoExisteAsync(parametro.Codigo))
                return "Ya existe un parámetro con ese código.";

            await _repositorio.InsertarAsync(parametro);
            return null;
        }

        public async Task<string?> ValidarYActualizarAsync(Parametro parametro)
        {
            if (string.IsNullOrWhiteSpace(parametro.Codigo))
                return "El código es obligatorio.";

            if (string.IsNullOrWhiteSpace(parametro.Valor))
                return "El valor es obligatorio.";

            if (parametro.Valor.Length > 500)
                return "El valor no puede superar los 500 caracteres.";

            if (await _repositorio.CodigoExisteAsync(parametro.Codigo, parametro.id_parametro))
                return "Ya existe un parámetro con ese código.";

            await _repositorio.ActualizarAsync(parametro);
            return null;
        }

        public async Task EliminarAsync(int id)
            => await _repositorio.EliminarAsync(id);
    }
}
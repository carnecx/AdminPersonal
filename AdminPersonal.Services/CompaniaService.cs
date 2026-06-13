using AdminPersonal.Entities;
using AdminPersonal.Repository;
using AdminPersonal.Services.Abstract;

namespace AdminPersonal.Services
{
    public class CompaniaService : ICompaniaService
    {
        private readonly CompaniaRepository _repositorio;

        public CompaniaService(CompaniaRepository repositorio)
        {
            _repositorio = repositorio;
        }

        public async Task<IEnumerable<Compania>> ObtenerTodosAsync()
            => await _repositorio.ObtenerTodosAsync();

        public async Task<Compania?> ObtenerPorIdAsync(int id)
            => await _repositorio.ObtenerPorIdAsync(id);

        public async Task<string?> ValidarYCrearAsync(Compania compania)
        {
            if (string.IsNullOrWhiteSpace(compania.Codigo))
                return "El código es obligatorio.";

            if (string.IsNullOrWhiteSpace(compania.Nombre))
                return "El nombre es obligatorio.";

            if (compania.Nombre.Length > 150)
                return "El nombre no puede superar los 150 caracteres.";

            if (await _repositorio.CodigoExisteAsync(compania.Codigo))
                return "Ya existe una compañía con ese código.";

            await _repositorio.InsertarAsync(compania);
            return null;
        }

        public async Task<string?> ValidarYActualizarAsync(Compania compania)
        {
            if (string.IsNullOrWhiteSpace(compania.Codigo))
                return "El código es obligatorio.";

            if (string.IsNullOrWhiteSpace(compania.Nombre))
                return "El nombre es obligatorio.";

            if (compania.Nombre.Length > 150)
                return "El nombre no puede superar los 150 caracteres.";

            if (await _repositorio.CodigoExisteAsync(compania.Codigo, compania.id_compania))
                return "Ya existe una compañía con ese código.";

            await _repositorio.ActualizarAsync(compania);
            return null;
        }

        public async Task EliminarAsync(int id)
            => await _repositorio.EliminarAsync(id);
    }
}
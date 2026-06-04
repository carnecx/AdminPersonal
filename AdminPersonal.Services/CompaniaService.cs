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
        {
            return await _repositorio.ObtenerTodosAsync();
        }

        public async Task<Compania?> ObtenerPorIdAsync(int id)
        {
            return await _repositorio.ObtenerPorIdAsync(id);
        }

        public async Task InsertarAsync(Compania compania)
        {
            await _repositorio.InsertarAsync(compania);
        }

        public async Task ActualizarAsync(Compania compania)
        {
            await _repositorio.ActualizarAsync(compania);
        }

        public async Task EliminarAsync(int id)
        {
            await _repositorio.EliminarAsync(id);
        }

        public async Task<bool> CodigoExisteAsync(string codigo, int? idExcluir = null)
        {
            return await _repositorio.CodigoExisteAsync(codigo, idExcluir);
        }
    }
}
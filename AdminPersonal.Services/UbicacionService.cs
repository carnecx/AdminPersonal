using AdminPersonal.Repository;

namespace AdminPersonal.Services
{
    public class UbicacionService
    {
        private readonly UbicacionRepository _repositorio;

        public UbicacionService(UbicacionRepository repositorio)
        {
            _repositorio = repositorio;
        }

        public async Task<int> CargarCsvAsync(Stream archivo)
        {
            return await _repositorio.CargarCsvAsync(archivo);
        }
    }
}

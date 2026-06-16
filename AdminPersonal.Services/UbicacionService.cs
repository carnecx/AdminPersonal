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

        public async Task<int> CargarCsvAsync(Stream archivo, string? nombreArchivo = null)
        {
            if (archivo == null)
                throw new Exception("Debe seleccionar un archivo.");

            if (archivo.Length == 0)
                throw new Exception("El archivo seleccionado esta vacio.");

            if (!string.IsNullOrWhiteSpace(nombreArchivo) &&
                !nombreArchivo.EndsWith(".csv", StringComparison.OrdinalIgnoreCase))
                throw new Exception("El archivo debe tener formato csv.");

            return await _repositorio.CargarCsvAsync(archivo);
        }
    }
}
using AdminPersonal.Repository;

namespace AdminPersonal.Services
{
    // servicio encargado de la logica relacionada con la carga de ubicaciones
    public class UbicacionService
    {
        // referencia al repositorio que accede a la base de datos
        private readonly UbicacionRepository _repositorio;

        // constructor que recibe el repositorio mediante inyeccion de dependencias
        public UbicacionService(UbicacionRepository repositorio)
        {
            _repositorio = repositorio;
        }

        // procesa un archivo csv con provincias, cantones y distritos
        public async Task<int> CargarCsvAsync(Stream archivo)
        {
            // delega el procesamiento al repositorio
            return await _repositorio.CargarCsvAsync(archivo);
        }
    }
}
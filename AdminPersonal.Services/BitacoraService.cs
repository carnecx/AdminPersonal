using AdminPersonal.Entities;
using AdminPersonal.Repository;

namespace AdminPersonal.Services
{
    // servicio encargado de manejar la logica relacionada con la bitacora
    public class BitacoraService
    {
        // referencia al repositorio que accede a la base de datos
        private readonly BitacoraRepository _repositorio;

        // constructor que recibe el repositorio mediante inyeccion de dependencias
        public BitacoraService(BitacoraRepository repositorio)
        {
            _repositorio = repositorio;
        }

        // registra una accion realizada por un usuario
        public async Task RegistrarAsync(int idUsuario, string descripcion)
        {
            // delega el registro al repositorio
            await _repositorio.RegistrarAsync(idUsuario, descripcion);
        }

        // obtiene la cantidad total de registros de bitacora
        // se utiliza para calcular la paginacion
        public async Task<int> ContarAsync(string? usuario, string? descripcion)
        {
            return await _repositorio.ContarAsync(usuario, descripcion);
        }

        // obtiene la lista de registros de bitacora aplicando filtros y paginacion
        public async Task<IEnumerable<Bitacora>> ListarAsync(
            string? usuario,
            string? descripcion,
            string orden,
            int pagina,
            int tamanoPagina)
        {
            return await _repositorio.ListarAsync(
                usuario,
                descripcion,
                orden,
                pagina,
                tamanoPagina);
        }
    }
}
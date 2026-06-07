using AdminPersonal.Entities;
using AdminPersonal.Repository;

namespace AdminPersonal.Services
{
    // servicio encargado de manejar la logica de negocio de instituciones educativas
    public class InstitucionService
    {
        // referencia al repositorio que accede a la base de datos
        private readonly InstitucionRepository _repositorio;

        // constructor que recibe el repositorio mediante inyeccion de dependencias
        public InstitucionService(InstitucionRepository repositorio)
        {
            _repositorio = repositorio;
        }

        // obtiene la lista completa de instituciones educativas
        public async Task<IEnumerable<InstitucionEducativa>> ObtenerTodosAsync()
        {
            return await _repositorio.ObtenerTodosAsync();
        }

        // busca una institucion educativa por su identificador
        public async Task<InstitucionEducativa?> ObtenerPorIdAsync(int id)
        {
            return await _repositorio.ObtenerPorIdAsync(id);
        }

        // registra una nueva institucion educativa
        public async Task InsertarAsync(InstitucionEducativa item)
        {
            await _repositorio.InsertarAsync(item);
        }

        // actualiza los datos de una institucion existente
        public async Task ActualizarAsync(InstitucionEducativa item)
        {
            await _repositorio.ActualizarAsync(item);
        }

        // elimina una institucion educativa por su identificador
        public async Task EliminarAsync(int id)
        {
            await _repositorio.EliminarAsync(id);
        }
    }
}
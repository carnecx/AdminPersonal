using AdminPersonal.Entities;
using AdminPersonal.Repository;
using System.Text.RegularExpressions;

namespace AdminPersonal.Services
{
    // servicio encargado de manejar la logica de instituciones educativas
    public class InstitucionService
    {
        // repositorio que se comunica con la base de datos
        private readonly InstitucionRepository _repositorio;

        // constructor que recibe el repositorio mediante inyeccion de dependencias
        public InstitucionService(InstitucionRepository repositorio)
        {
            _repositorio = repositorio;
        }

        // obtiene todas las instituciones educativas
        public async Task<IEnumerable<InstitucionEducativa>> ObtenerTodosAsync()
        {
            return await _repositorio.ObtenerTodosAsync();
        }

        // obtiene una institucion por su id
        public async Task<InstitucionEducativa?> ObtenerPorIdAsync(int id)
        {
            // valida que el id sea correcto
            if (id <= 0)
                throw new Exception("El id de la institucion no es valido.");

            return await _repositorio.ObtenerPorIdAsync(id);
        }

        // inserta una nueva institucion
        public async Task InsertarAsync(InstitucionEducativa item)
        {
            // valida los datos antes de guardar
            ValidarInstitucion(item);

            await _repositorio.InsertarAsync(item);
        }

        // actualiza una institucion existente
        public async Task ActualizarAsync(InstitucionEducativa item)
        {
            // valida que el id sea correcto
            if (item.id_institucion <= 0)
                throw new Exception("El id de la institucion no es valido.");

            // valida los datos antes de actualizar
            ValidarInstitucion(item);

            await _repositorio.ActualizarAsync(item);
        }

        // elimina una institucion por id
        public async Task EliminarAsync(int id)
        {
            // valida que el id sea correcto
            if (id <= 0)
                throw new Exception("El id de la institucion no es valido.");

            await _repositorio.EliminarAsync(id);
        }

        // valida las reglas de negocio de instituciones
        private void ValidarInstitucion(InstitucionEducativa item)
        {
            // valida que el objeto no venga vacio
            if (item == null)
                throw new Exception("Los datos de la institucion son requeridos.");

            // valida que el codigo no este vacio
            if (string.IsNullOrWhiteSpace(item.codigo))
                throw new Exception("El codigo de la institucion es requerido.");

            // valida que el nombre no este vacio
            if (string.IsNullOrWhiteSpace(item.nombre))
                throw new Exception("El nombre de la institucion es requerido.");

            // valida que el nombre no supere 150 caracteres
            if (item.nombre.Length > 150)
                throw new Exception("El nombre maximo es de 150 caracteres.");

            // valida que el nombre solo tenga letras y espacios
            if (!Regex.IsMatch(item.nombre, @"^[A-Za-z·ÈÌÛ˙¡…Õ”⁄Ò— ]+$"))
                throw new Exception("El nombre solo debe tener letras y espacios.");
        }
    }
}
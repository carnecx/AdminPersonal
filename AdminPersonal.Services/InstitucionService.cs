using AdminPersonal.Entities;
using AdminPersonal.Repository;
using System.Text.RegularExpressions;

namespace AdminPersonal.Services
{
    public class InstitucionService
    {
        private readonly InstitucionRepository _repositorio;

        public InstitucionService(InstitucionRepository repositorio)
        {
            _repositorio = repositorio;
        }

        public async Task<IEnumerable<InstitucionEducativa>> ObtenerTodosAsync()
        {
            return await _repositorio.ObtenerTodosAsync();
        }

        public async Task<InstitucionEducativa?> ObtenerPorIdAsync(int id)
        {
            if (id <= 0)
                throw new Exception("El id de la institucion no es valido.");

            return await _repositorio.ObtenerPorIdAsync(id);
        }

        public async Task InsertarAsync(InstitucionEducativa item)
        {
            ValidarInstitucion(item);
            await _repositorio.InsertarAsync(item);
        }

        public async Task ActualizarAsync(InstitucionEducativa item)
        {
            if (item.id_institucion <= 0)
                throw new Exception("El id de la institucion no es valido.");

            ValidarInstitucion(item);
            await _repositorio.ActualizarAsync(item);
        }

        public async Task EliminarAsync(int id)
        {
            if (id <= 0)
                throw new Exception("El id de la institucion no es valido.");

            await _repositorio.EliminarAsync(id);
        }

        private void ValidarInstitucion(InstitucionEducativa item)
        {
            if (item == null)
                throw new Exception("Los datos de la institucion son requeridos.");

            if (string.IsNullOrWhiteSpace(item.codigo))
                throw new Exception("El codigo de la institucion es requerido.");

            if (string.IsNullOrWhiteSpace(item.nombre))
                throw new Exception("El nombre de la institucion es requerido.");

            if (item.nombre.Length > 150)
                throw new Exception("El nombre maximo es de 150 caracteres.");

            if (!Regex.IsMatch(item.nombre, @"^[A-Za-z·ÈÌÛ˙¡…Õ”⁄Ò— ]+$"))
                throw new Exception("El nombre solo debe tener letras y espacios.");
        }
    }
}
using AdminPersonal.Entities;
using AdminPersonal.Repository;
using AdminPersonal.Services.Abstract;

namespace AdminPersonal.Services
{
    public class RolService : IRolService
    {
        private readonly RolRepository _repositorio;

        public RolService(RolRepository repositorio)
        {
            _repositorio = repositorio;
        }

        public async Task<IEnumerable<Rol>> ObtenerTodosAsync()
            => await _repositorio.ObtenerTodosAsync();

        public async Task<Rol?> ObtenerPorIdAsync(int id)
            => await _repositorio.ObtenerPorIdAsync(id);

        public async Task<string?> ValidarYCrearAsync(Rol rol)
        {
            if (string.IsNullOrWhiteSpace(rol.nombre_rol))
                return "El nombre del rol es obligatorio.";

            if (rol.nombre_rol.Length > 40)
                return "El nombre del rol no puede superar los 40 caracteres.";

            if (!System.Text.RegularExpressions.Regex.IsMatch(rol.nombre_rol, @"^[a-zA-ZáéíóúÁÉÍÓÚñÑ ]+$"))
                return "El nombre del rol solo debe contener letras y espacios.";

            if (await _repositorio.NombreExisteAsync(rol.nombre_rol))
                return "Ya existe un rol con ese nombre.";

            await _repositorio.InsertarAsync(rol);
            return null;
        }

        public async Task<string?> ValidarYActualizarAsync(Rol rol)
        {
            if (string.IsNullOrWhiteSpace(rol.nombre_rol))
                return "El nombre del rol es obligatorio.";

            if (rol.nombre_rol.Length > 40)
                return "El nombre del rol no puede superar los 40 caracteres.";

            if (!System.Text.RegularExpressions.Regex.IsMatch(rol.nombre_rol, @"^[a-zA-ZáéíóúÁÉÍÓÚñÑ ]+$"))
                return "El nombre del rol solo debe contener letras y espacios.";

            if (await _repositorio.NombreExisteAsync(rol.nombre_rol, rol.id_rol))
                return "Ya existe un rol con ese nombre.";

            await _repositorio.ActualizarAsync(rol);
            return null;
        }

        public async Task<string?> EliminarAsync(int id)
        {
            if (await _repositorio.EstaAsignadoAsync(id))
                return "No se puede eliminar un registro con datos relacionados.";

            await _repositorio.EliminarAsync(id);
            return null;
        }
    }
}
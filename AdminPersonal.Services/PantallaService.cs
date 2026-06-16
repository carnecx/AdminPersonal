using AdminPersonal.Entities;
using AdminPersonal.Repository;
using AdminPersonal.Services.Abstract;

namespace AdminPersonal.Services
{
    public class PantallaService : IPantallaService
    {
        private readonly PantallaRepository _repositorio;

        public PantallaService(PantallaRepository repositorio)
        {
            _repositorio = repositorio;
        }

        public IEnumerable<Pantalla> ObtenerTodos()
            => _repositorio.ObtenerTodos();

        public Pantalla? ObtenerPorId(int id)
            => _repositorio.ObtenerPorId(id);

        public string? ValidarYCrear(Pantalla pantalla, List<int> rolesSeleccionados)
        {
            if (string.IsNullOrWhiteSpace(pantalla.nombre_pantalla))
                return "El nombre de la pantalla es obligatorio.";

            if (pantalla.nombre_pantalla.Length > 100)
                return "El nombre de la pantalla no puede superar los 100 caracteres.";

            if (!System.Text.RegularExpressions.Regex.IsMatch(pantalla.nombre_pantalla, @"^[a-zA-ZáéíóúÁÉÍÓÚñÑ ]+$"))
                return "El nombre de la pantalla solo debe contener letras y espacios.";

            if (_repositorio.BuscarDuplicado(pantalla.nombre_pantalla) != null)
                return "Ya existe una pantalla con ese nombre.";

            var id = _repositorio.Crear(pantalla);

            if (rolesSeleccionados.Any())
                _repositorio.AsignarRoles(id, rolesSeleccionados);

            return null;
        }

        public string? ValidarYActualizar(Pantalla pantalla, List<int> rolesSeleccionados)
        {
            if (string.IsNullOrWhiteSpace(pantalla.nombre_pantalla))
                return "El nombre de la pantalla es obligatorio.";

            if (pantalla.nombre_pantalla.Length > 100)
                return "El nombre de la pantalla no puede superar los 100 caracteres.";

            if (!System.Text.RegularExpressions.Regex.IsMatch(pantalla.nombre_pantalla, @"^[a-zA-ZáéíóúÁÉÍÓÚñÑ ]+$"))
                return "El nombre de la pantalla solo debe contener letras y espacios.";

            if (_repositorio.BuscarDuplicadoEditar(pantalla.nombre_pantalla, pantalla.id_pantalla) != null)
                return "Ya existe una pantalla con ese nombre.";

            _repositorio.Actualizar(pantalla);
            _repositorio.EliminarAsignaciones(pantalla.id_pantalla);

            if (rolesSeleccionados.Any())
                _repositorio.AsignarRoles(pantalla.id_pantalla, rolesSeleccionados);

            return null;
        }

        public string? Eliminar(int id)
        {
            if (_repositorio.EstaAsignada(id))
                return "No se puede eliminar un registro con datos relacionados.";

            _repositorio.Eliminar(id);
            return null;
        }

        public IEnumerable<RolPantalla> ObtenerRolesConAsignacion(int id_pantalla)
            => _repositorio.ObtenerRolesConAsignacion(id_pantalla);

        public IEnumerable<string> ObtenerNombresPorRol(int id_rol)
            => _repositorio.ObtenerNombresPorRol(id_rol);
    }
}
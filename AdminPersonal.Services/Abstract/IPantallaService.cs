using AdminPersonal.Entities;

namespace AdminPersonal.Services.Abstract
{
    public interface IPantallaService
    {
        IEnumerable<Pantalla> ObtenerTodos();
        Pantalla? ObtenerPorId(int id);
        string? ValidarYCrear(Pantalla pantalla, List<int> rolesSeleccionados);
        string? ValidarYActualizar(Pantalla pantalla, List<int> rolesSeleccionados);
        string? Eliminar(int id);
        IEnumerable<RolPantalla> ObtenerRolesConAsignacion(int id_pantalla);
        IEnumerable<string> ObtenerNombresPorRol(int id_rol);
    }
}
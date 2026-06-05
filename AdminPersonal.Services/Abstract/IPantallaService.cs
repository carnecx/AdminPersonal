using AdminPersonal.Entities;

namespace AdminPersonal.Services.Abstract
{
    public interface IPantallaService
    {
        IEnumerable<Pantalla> ObtenerTodos();
        Pantalla? ObtenerPorId(int id);
        Pantalla? BuscarDuplicado(string nombre_pantalla);
        Pantalla? BuscarDuplicadoEditar(string nombre_pantalla, int id_pantalla);
        bool EstaAsignada(int id);
        int Crear(Pantalla pantalla);
        void Actualizar(Pantalla pantalla);
        void Eliminar(int id);
        IEnumerable<RolPantalla> ObtenerRolesConAsignacion(int id_pantalla);
        void EliminarAsignaciones(int id_pantalla);
        void AsignarRoles(int id_pantalla, List<int> rolesSeleccionados);
        IEnumerable<string> ObtenerNombresPorRol(int id_rol);
    }
}

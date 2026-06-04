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
        void Crear(Pantalla pantalla);
        void Actualizar(Pantalla pantalla);
        void Eliminar(int id);
    }
}

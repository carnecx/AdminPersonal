using AdminPersonal.Entities;

namespace AdminPersonal.Services.Abstract
{
    public interface IRequisitoPuestoService
    {
        IEnumerable<RequisitoPuesto> ObtenerTodos();
        RequisitoPuesto? ObtenerPorId(int id);
        IEnumerable<dynamic> ObtenerPuestos();
        RequisitoPuesto? BuscarDuplicado(string nombre_requisito, int id_puesto);
        RequisitoPuesto? BuscarDuplicadoEditar(string nombre_requisito, int id_puesto, int id_requisito);
        void Crear(RequisitoPuesto requisito);
        void Actualizar(RequisitoPuesto requisito);
        void Eliminar(int id);
    }
}

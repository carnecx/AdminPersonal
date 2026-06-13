using AdminPersonal.Entities;

namespace AdminPersonal.Services.Abstract
{
    public interface IRequisitoPuestoService
    {
        IEnumerable<RequisitoPuesto> ObtenerTodos();
        RequisitoPuesto? ObtenerPorId(int id);
        IEnumerable<dynamic> ObtenerPuestos();
        string? ValidarYCrear(RequisitoPuesto requisito);
        string? ValidarYActualizar(RequisitoPuesto requisito);
        void Eliminar(int id);
    }
}
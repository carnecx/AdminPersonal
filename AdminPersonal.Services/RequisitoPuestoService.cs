using AdminPersonal.Entities;
using AdminPersonal.Repository;
using AdminPersonal.Services.Abstract;

namespace AdminPersonal.Services
{
    public class RequisitoPuestoService : IRequisitoPuestoService
    {
        private readonly RequisitoPuestoRepository _repositorio;

        public RequisitoPuestoService(RequisitoPuestoRepository repositorio)
        {
            _repositorio = repositorio;
        }

        public IEnumerable<RequisitoPuesto> ObtenerTodos()
            => _repositorio.ObtenerTodos();

        public RequisitoPuesto? ObtenerPorId(int id)
            => _repositorio.ObtenerPorId(id);

        public IEnumerable<dynamic> ObtenerPuestos()
            => _repositorio.ObtenerPuestos();

        public string? ValidarYCrear(RequisitoPuesto requisito)
        {
            if (string.IsNullOrWhiteSpace(requisito.nombre_requisito))
                return "El nombre del requisito es obligatorio.";

            if (requisito.nombre_requisito.Length > 200)
                return "El nombre del requisito no puede superar los 200 caracteres.";

            if (requisito.id_puesto == 0)
                return "Debe seleccionar un puesto.";

            if (_repositorio.RequisitoExiste(requisito.nombre_requisito, requisito.id_puesto))
                return "Ya existe ese requisito para este puesto.";

            _repositorio.Crear(requisito);
            return null;
        }

        public string? ValidarYActualizar(RequisitoPuesto requisito)
        {
            if (string.IsNullOrWhiteSpace(requisito.nombre_requisito))
                return "El nombre del requisito es obligatorio.";

            if (requisito.nombre_requisito.Length > 200)
                return "El nombre del requisito no puede superar los 200 caracteres.";

            if (requisito.id_puesto == 0)
                return "Debe seleccionar un puesto.";

            if (_repositorio.RequisitoExiste(requisito.nombre_requisito, requisito.id_puesto, requisito.id_requisito))
                return "Ya existe ese requisito para este puesto.";

            _repositorio.Actualizar(requisito);
            return null;
        }

        public void Eliminar(int id)
            => _repositorio.Eliminar(id);
    }
}
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
        {
            return _repositorio.ObtenerTodos();
        }

        public RequisitoPuesto? ObtenerPorId(int id)
        {
            return _repositorio.ObtenerPorId(id);
        }

        public IEnumerable<dynamic> ObtenerPuestos()
        {
            return _repositorio.ObtenerPuestos();
        }

        public RequisitoPuesto? BuscarDuplicado(string nombre_requisito, int id_puesto)
        {
            return _repositorio.BuscarDuplicado(nombre_requisito, id_puesto);
        }

        public RequisitoPuesto? BuscarDuplicadoEditar(string nombre_requisito, int id_puesto, int id_requisito)
        {
            return _repositorio.BuscarDuplicadoEditar(nombre_requisito, id_puesto, id_requisito);
        }

        public void Crear(RequisitoPuesto requisito)
        {
            _repositorio.Crear(requisito);
        }

        public void Actualizar(RequisitoPuesto requisito)
        {
            _repositorio.Actualizar(requisito);
        }

        public void Eliminar(int id)
        {
            _repositorio.Eliminar(id);
        }
    }
}

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

        public IEnumerable<Pantalla> ObtenerTodos() => _repositorio.ObtenerTodos();
        public Pantalla? ObtenerPorId(int id) => _repositorio.ObtenerPorId(id);
        public Pantalla? BuscarDuplicado(string nombre_pantalla) => _repositorio.BuscarDuplicado(nombre_pantalla);
        public Pantalla? BuscarDuplicadoEditar(string nombre_pantalla, int id_pantalla) => _repositorio.BuscarDuplicadoEditar(nombre_pantalla, id_pantalla);
        public bool EstaAsignada(int id) => _repositorio.EstaAsignada(id);
        public void Crear(Pantalla pantalla) => _repositorio.Crear(pantalla);
        public void Actualizar(Pantalla pantalla) => _repositorio.Actualizar(pantalla);
        public void Eliminar(int id) => _repositorio.Eliminar(id);
    }
}

using AdminPersonal.Entities;
using AdminPersonal.Repository;

namespace AdminPersonal.Services
{
    public class BitacoraService
    {
        private readonly BitacoraRepository _repositorio;

        public BitacoraService(BitacoraRepository repositorio)
        {
            _repositorio = repositorio;
        }

        public async Task RegistrarAsync(int idUsuario, string descripcion)
        {
            await _repositorio.RegistrarAsync(idUsuario, descripcion);
        }

        public async Task<int> ContarAsync(string? usuario, string? descripcion)
        {
            return await _repositorio.ContarAsync(usuario, descripcion);
        }

        public async Task<IEnumerable<Bitacora>> ListarAsync(string? usuario, string? descripcion, string orden, int pagina, int tamanoPagina)
        {
            return await _repositorio.ListarAsync(usuario, descripcion, orden, pagina, tamanoPagina);
        }
    }
}

using Dapper;
using Microsoft.Extensions.Configuration;
using MySqlConnector;
using System.Data;

namespace AdminPersonal.Repository
{
    public class UbicacionRepository
    {
        private readonly string cadenaConexion;

        public UbicacionRepository(IConfiguration config)
        {
            cadenaConexion = config.GetConnectionString("DefaultConnection")!;
        }

        private MySqlConnection AbrirConexion()
        {
            return new MySqlConnection(cadenaConexion);
        }

        public async Task<int> CargarCsvAsync(Stream archivo)
        {
            using var reader = new StreamReader(archivo);
            using var conexion = AbrirConexion();
            int contador = 0;

            while (!reader.EndOfStream)
            {
                var linea = await reader.ReadLineAsync();
                if (string.IsNullOrWhiteSpace(linea)) continue;
                if (linea.ToLower().StartsWith("provincia")) continue;

                var partes = linea.Split(';');
                if (partes.Length < 3) continue;

                string provincia = partes[0].Trim();
                string canton = partes[1].Trim();
                string distrito = partes[2].Trim();

                int idProvincia = await ObtenerOCrearProvincia(conexion, provincia);
                int idCanton = await ObtenerOCrearCanton(conexion, canton, idProvincia);
                await ObtenerOCrearDistrito(conexion, distrito, idCanton);

                contador++;
            }

            return contador;
        }

        private async Task<int> ObtenerOCrearProvincia(IDbConnection conexion, string nombre)
        {
            int? id = await conexion.QueryFirstOrDefaultAsync<int?>(
                "SELECT id_provincia FROM provincia WHERE nombre = @nombre", new { nombre });
            if (id != null) return id.Value;

            int nuevoId = await conexion.QueryFirstAsync<int>(
                "SELECT IFNULL(MAX(id_provincia), 0) + 1 FROM provincia");
            await conexion.ExecuteAsync(
                "INSERT INTO provincia (id_provincia, nombre) VALUES (@id, @nombre)", new { id = nuevoId, nombre });
            return nuevoId;
        }

        private async Task<int> ObtenerOCrearCanton(IDbConnection conexion, string nombre, int idProvincia)
        {
            int? id = await conexion.QueryFirstOrDefaultAsync<int?>(
                "SELECT id_canton FROM canton WHERE nombre = @nombre AND id_provincia = @idProvincia",
                new { nombre, idProvincia });
            if (id != null) return id.Value;

            int nuevoId = await conexion.QueryFirstAsync<int>(
                "SELECT IFNULL(MAX(id_canton), 0) + 1 FROM canton");
            await conexion.ExecuteAsync(
                "INSERT INTO canton (id_canton, nombre, id_provincia) VALUES (@id, @nombre, @idProvincia)",
                new { id = nuevoId, nombre, idProvincia });
            return nuevoId;
        }

        private async Task<int> ObtenerOCrearDistrito(IDbConnection conexion, string nombre, int idCanton)
        {
            int? id = await conexion.QueryFirstOrDefaultAsync<int?>(
                "SELECT id_distrito FROM distrito WHERE nombre = @nombre AND id_canton = @idCanton",
                new { nombre, idCanton });
            if (id != null) return id.Value;

            int nuevoId = await conexion.QueryFirstAsync<int>(
                "SELECT IFNULL(MAX(id_distrito), 0) + 1 FROM distrito");
            await conexion.ExecuteAsync(
                "INSERT INTO distrito (id_distrito, nombre, id_canton) VALUES (@id, @nombre, @idCanton)",
                new { id = nuevoId, nombre, idCanton });
            return nuevoId;
        }
    }
}

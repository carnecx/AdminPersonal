using Dapper;
using Microsoft.Extensions.Configuration;
using MySqlConnector;
using System.Data;

namespace AdminPersonal.Repository
{
    // repositorio encargado de cargar provincias, cantones y distritos desde un archivo csv
    public class UbicacionRepository
    {
        // almacena la cadena de conexion a mysql
        private readonly string cadenaConexion;

        // constructor que obtiene la cadena de conexion desde appsettings.json
        public UbicacionRepository(IConfiguration config)
        {
            cadenaConexion = config.GetConnectionString("DefaultConnection")!;
        }

        // abre una conexion a la base de datos
        private MySqlConnection AbrirConexion()
        {
            return new MySqlConnection(cadenaConexion);
        }

        // procesa el archivo csv y registra la informacion en la base de datos
        public async Task<int> CargarCsvAsync(Stream archivo)
        {
            using var reader = new StreamReader(archivo);
            using var conexion = AbrirConexion();

            // contador de registros procesados
            int contador = 0;

            // recorre el archivo linea por linea
            while (!reader.EndOfStream)
            {
                var linea = await reader.ReadLineAsync();

                // ignora lineas vacias
                if (string.IsNullOrWhiteSpace(linea))
                    continue;

                // ignora el encabezado del csv
                if (linea.ToLower().StartsWith("provincia"))
                    continue;

                // separa los valores usando ;
                var partes = linea.Split(';');

                // valida que existan provincia, canton y distrito
                if (partes.Length < 3)
                    continue;

                // obtiene los datos del archivo
                string provincia = partes[0].Trim();
                string canton = partes[1].Trim();
                string distrito = partes[2].Trim();

                // obtiene o crea la provincia
                int idProvincia = await ObtenerOCrearProvincia(conexion, provincia);

                // obtiene o crea el canton
                int idCanton = await ObtenerOCrearCanton(conexion, canton, idProvincia);

                // obtiene o crea el distrito
                await ObtenerOCrearDistrito(conexion, distrito, idCanton);

                contador++;
            }

            // retorna la cantidad de registros procesados
            return contador;
        }

        // busca una provincia por nombre y la crea si no existe
        private async Task<int> ObtenerOCrearProvincia(IDbConnection conexion, string nombre)
        {
            // verifica si la provincia ya existe
            int? id = await conexion.QueryFirstOrDefaultAsync<int?>(
                "SELECT id_provincia FROM provincia WHERE nombre = @nombre",
                new { nombre });

            if (id != null)
                return id.Value;

            // genera un nuevo id para la provincia
            int nuevoId = await conexion.QueryFirstAsync<int>(
                "SELECT IFNULL(MAX(id_provincia), 0) + 1 FROM provincia");

            // inserta la nueva provincia
            await conexion.ExecuteAsync(
                "INSERT INTO provincia (id_provincia, nombre) VALUES (@id, @nombre)",
                new { id = nuevoId, nombre });

            return nuevoId;
        }

        // busca un canton por nombre y provincia y lo crea si no existe
        private async Task<int> ObtenerOCrearCanton(IDbConnection conexion, string nombre, int idProvincia)
        {
            // verifica si el canton ya existe
            int? id = await conexion.QueryFirstOrDefaultAsync<int?>(
                "SELECT id_canton FROM canton WHERE nombre = @nombre AND id_provincia = @idProvincia",
                new { nombre, idProvincia });

            if (id != null)
                return id.Value;

            // genera un nuevo id para el canton
            int nuevoId = await conexion.QueryFirstAsync<int>(
                "SELECT IFNULL(MAX(id_canton), 0) + 1 FROM canton");

            // inserta el nuevo canton
            await conexion.ExecuteAsync(
                "INSERT INTO canton (id_canton, nombre, id_provincia) VALUES (@id, @nombre, @idProvincia)",
                new { id = nuevoId, nombre, idProvincia });

            return nuevoId;
        }

        // busca un distrito por nombre y canton y lo crea si no existe
        private async Task<int> ObtenerOCrearDistrito(IDbConnection conexion, string nombre, int idCanton)
        {
            // verifica si el distrito ya existe
            int? id = await conexion.QueryFirstOrDefaultAsync<int?>(
                "SELECT id_distrito FROM distrito WHERE nombre = @nombre AND id_canton = @idCanton",
                new { nombre, idCanton });

            if (id != null)
                return id.Value;

            // genera un nuevo id para el distrito
            int nuevoId = await conexion.QueryFirstAsync<int>(
                "SELECT IFNULL(MAX(id_distrito), 0) + 1 FROM distrito");

            // inserta el nuevo distrito
            await conexion.ExecuteAsync(
                "INSERT INTO distrito (id_distrito, nombre, id_canton) VALUES (@id, @nombre, @idCanton)",
                new { id = nuevoId, nombre, idCanton });

            return nuevoId;
        }
    }
}
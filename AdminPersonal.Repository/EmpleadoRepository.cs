using AdminPersonal.Entities;
using Dapper;
using MySqlConnector;
using System.Data;

namespace AdminPersonal.Repository
{
    public class EmpleadoRepository
    {
        private readonly IDbConnectionFactory _dbFactory;

        public EmpleadoRepository(IDbConnectionFactory dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public async Task<List<Oferente>> ObtenerOferentesDisponiblesAsync()
        {
            using var conexion = _dbFactory.CrearConexion();
            var resultado = await conexion.QueryAsync<Oferente>(
                @"SELECT o.id_oferente, o.identificacion, o.nombre_completo AS NombreCompleto,
                         o.fecha_nacimiento AS FechaNacimiento, o.tipo_identificacion AS TipoIdentificacion
                  FROM oferente o
                  WHERE o.id_oferente NOT IN (SELECT id_oferente FROM empleado)");
            return resultado.ToList();
        }

        public async Task<List<Puesto>> ObtenerPuestosAsync()
        {
            using var conexion = _dbFactory.CrearConexion();
            var resultado = await conexion.QueryAsync<Puesto>(
                "SELECT id_puesto AS IdPuesto, codigo AS Codigo, nombre AS Nombre, salario AS Salario FROM puesto");
            return resultado.ToList();
        }

        public async Task<string> GenerarNumeroEmpleadoAsync()
        {
            using var conexion = _dbFactory.CrearConexion();
            var count = await conexion.QueryFirstAsync<int>("SELECT COUNT(*) FROM empleado");
            return $"EMP-{(count + 1):D4}";
        }

        public async Task ContratarEmpleadoAsync(int idOferente, int idPuesto, string numeroEmpleado)
        {
            using var conexion = (MySqlConnection)_dbFactory.CrearConexion();
            await conexion.OpenAsync();
            using var transaction = await conexion.BeginTransactionAsync();
            try
            {
                var idEmpleado = await conexion.QueryFirstAsync<int>(
                    @"INSERT INTO empleado (numero_empleado, id_oferente, id_puesto)
                      VALUES (@numero, @idOferente, @idPuesto);
                      SELECT LAST_INSERT_ID();",
                    new { numero = numeroEmpleado, idOferente, idPuesto }, transaction);

                var conteo = await conexion.QueryFirstAsync<int>(
                    "SELECT COUNT(*) FROM accion_personal", transaction: transaction);
                var codigoAccion = $"ACC-{(conteo + 1):D4}";

                await conexion.ExecuteAsync(
                    @"INSERT INTO accion_personal (codigo, fecha_accion, descripcion, id_empleado, id_jefatura)
                      VALUES (@codigo, @fecha, @desc, @idEmp, @idJef)",
                    new
                    {
                        codigo = codigoAccion,
                        fecha = DateTime.Now.Date,
                        desc = "Contratación del empleado",
                        idEmp = idEmpleado,
                        idJef = idEmpleado
                    }, transaction);

                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}
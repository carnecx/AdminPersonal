using AdminPersonal.Entities;
using MySqlConnector;

namespace AdminPersonal.Repository
{
    public class EmpleadoRepository
    {
        private readonly string _connectionString;

        public EmpleadoRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<List<Oferente>> ObtenerOferentesDisponiblesAsync()
        {
            var lista = new List<Oferente>();
            using var conn = new MySqlConnection(_connectionString);
            await conn.OpenAsync();
            // Solo oferentes que NO son empleados todavía
            var sql = @"SELECT o.id_oferente, o.identificacion, o.nombre_completo, 
                               o.fecha_nacimiento, o.tipo_identificacion
                        FROM oferente o
                        WHERE o.id_oferente NOT IN (SELECT id_oferente FROM empleado)";
            using var cmd = new MySqlCommand(sql, conn);
            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                lista.Add(new Oferente
                {
                    id_oferente = reader.GetInt32("id_oferente"),
                    Identificacion = reader.GetString("identificacion"),
                    NombreCompleto = reader.GetString("nombre_completo"),
                    FechaNacimiento = reader.GetDateTime("fecha_nacimiento"),
                    TipoIdentificacion = reader.GetString("tipo_identificacion")
                });
            }
            return lista;
        }

        public async Task<List<Puesto>> ObtenerPuestosAsync()
        {
            var lista = new List<Puesto>();
            using var conn = new MySqlConnection(_connectionString);
            await conn.OpenAsync();
            var sql = "SELECT id_puesto, codigo, nombre, salario FROM puesto";
            using var cmd = new MySqlCommand(sql, conn);
            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                lista.Add(new Puesto
                {
                    IdPuesto = reader.GetInt32("id_puesto"),
                    Codigo = reader.GetString("codigo"),
                    Nombre = reader.GetString("nombre"),
                    Salario = reader.GetDecimal("salario")
                });
            }
            return lista;
        }

        public async Task<string> GenerarNumeroEmpleadoAsync()
        {
            using var conn = new MySqlConnection(_connectionString);
            await conn.OpenAsync();
            var sql = "SELECT COUNT(*) FROM empleado";
            using var cmd = new MySqlCommand(sql, conn);
            var count = Convert.ToInt32(await cmd.ExecuteScalarAsync());
            return $"EMP-{(count + 1):D4}";
        }

        public async Task ContratarEmpleadoAsync(int idOferente, int idPuesto, string numeroEmpleado)
        {
            using var conn = new MySqlConnection(_connectionString);
            await conn.OpenAsync();
            using var transaction = await conn.BeginTransactionAsync();
            try
            {
                // 1. Insertar empleado
                var sqlEmpleado = @"INSERT INTO empleado (numero_empleado, id_oferente, id_puesto)
                                    VALUES (@numero, @idOferente, @idPuesto)";
                using var cmdEmp = new MySqlCommand(sqlEmpleado, conn, transaction);
                cmdEmp.Parameters.AddWithValue("@numero", numeroEmpleado);
                cmdEmp.Parameters.AddWithValue("@idOferente", idOferente);
                cmdEmp.Parameters.AddWithValue("@idPuesto", idPuesto);
                await cmdEmp.ExecuteNonQueryAsync();

                var idEmpleado = (int)cmdEmp.LastInsertedId;

                // 2. Generar código acción de personal consecutivo
                var sqlConteo = "SELECT COUNT(*) FROM accion_personal";
                using var cmdConteo = new MySqlCommand(sqlConteo, conn, transaction);
                var conteo = Convert.ToInt32(await cmdConteo.ExecuteScalarAsync());
                var codigoAccion = $"ACC-{(conteo + 1):D4}";

                // 3. Insertar acción de personal por contratación
                var sqlAccion = @"INSERT INTO accion_personal 
                                    (codigo, fecha_accion, descripcion, id_empleado, id_jefatura)
                                  VALUES (@codigo, @fecha, @desc, @idEmp, @idJef)";
                using var cmdAcc = new MySqlCommand(sqlAccion, conn, transaction);
                cmdAcc.Parameters.AddWithValue("@codigo", codigoAccion);
                cmdAcc.Parameters.AddWithValue("@fecha", DateTime.Now.Date);
                cmdAcc.Parameters.AddWithValue("@desc", "Contratación del empleado");
                cmdAcc.Parameters.AddWithValue("@idEmp", idEmpleado);
                cmdAcc.Parameters.AddWithValue("@idJef", idEmpleado); // jefatura inicial = el mismo
                await cmdAcc.ExecuteNonQueryAsync();

                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task RegistrarBitacoraAsync(int idUsuario, string descripcion)
        {
            using var conn = new MySqlConnection(_connectionString);
            await conn.OpenAsync();
            var sql = "INSERT INTO bitacora (fecha, id_usuario, descripcion) VALUES (@fecha, @idUsuario, @descripcion)";
            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@fecha", DateTime.Now);
            cmd.Parameters.AddWithValue("@idUsuario", idUsuario);
            cmd.Parameters.AddWithValue("@descripcion", descripcion);
            await cmd.ExecuteNonQueryAsync();
        }
    }
}
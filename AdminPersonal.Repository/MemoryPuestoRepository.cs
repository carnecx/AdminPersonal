using System.Collections.Generic;
using System.Linq;
using AdminPersonal.Entities;
using MySqlConnector;
using Dapper;
using System;

namespace AdminPersonal.Repository
{
    public static class MemoryPuestoRepository
    {
        public static List<Puesto> GetAll()
        {
            BitacoraRepository.RegistrarConsulta("todos los puestos");
            using (var connection = ConnectionProvider.GetConnection())
            {
                string sql = @"SELECT p.*, p2.nombre as NombreJefatura 
                               FROM puestos p
                               LEFT JOIN puestos p2 ON p.jefatura_puesto_id = p2.id
                               ORDER BY p.codigo";
                return connection.Query<Puesto>(sql).ToList();
            }
        }

        public static Puesto? GetById(int id)
        {
            BitacoraRepository.RegistrarConsulta($"puesto {id}");
            using (var connection = ConnectionProvider.GetConnection())
            {
                string sql = "SELECT * FROM puestos WHERE id = @Id";
                return connection.QueryFirstOrDefault<Puesto>(sql, new { Id = id });
            }
        }

        public static void Add(Puesto puesto)
        {
            if (puesto.JefaturaPuestoId == 0) puesto.JefaturaPuestoId = null;
            using (var connection = ConnectionProvider.GetConnection())
            {
                string sql = @"INSERT INTO puestos (codigo, nombre, salario, jefatura_puesto_id) 
                               VALUES (@Codigo, @Nombre, @Salario, @JefaturaPuestoId)";
                connection.Execute(sql, puesto);
            }
            BitacoraRepository.RegistrarNueva(puesto);
        }

        public static void Update(Puesto puesto)
        {
            if (puesto.JefaturaPuestoId == 0) puesto.JefaturaPuestoId = null;
            var anterior = GetById(puesto.Id);
            using (var connection = ConnectionProvider.GetConnection())
            {
                string sql = @"UPDATE puestos SET 
                               codigo = @Codigo,
                               nombre = @Nombre,
                               salario = @Salario,
                               jefatura_puesto_id = @JefaturaPuestoId
                               WHERE id = @Id";
                connection.Execute(sql, puesto);
            }
            if (anterior != null)
            {
                BitacoraRepository.RegistrarActualizacion(anterior, puesto);
            }
        }

        public static void Delete(int id)
        {
            var anterior = GetById(id);
            using (var connection = ConnectionProvider.GetConnection())
            {
                // Validar que no tenga datos relacionados
                string checkSql = "SELECT COUNT(*) FROM empleados WHERE puesto_id = @Id";
                int count = connection.ExecuteScalar<int>(checkSql, new { Id = id });
                
                if (count > 0)
                {
                    var ex = new Exception("No se puede eliminar un registro con datos relacionados.");
                    BitacoraRepository.RegistrarError(ex.Message);
                    throw ex;
                }
                
                string sql = "DELETE FROM puestos WHERE id = @Id";
                connection.Execute(sql, new { Id = id });
            }
            if (anterior != null)
            {
                BitacoraRepository.RegistrarEliminacion(anterior);
            }
        }
    }
}

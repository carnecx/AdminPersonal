using System;
using System.Collections.Generic;
using System.Linq;
using AdminPersonal.Entities;
using MySqlConnector;
using Dapper;

namespace AdminPersonal.Repository
{
    public static class MemoryAccionPersonalRepository
    {
        public static List<AccionPersonal> GetAll()
        {
            BitacoraRepository.RegistrarConsulta("todas las acciones de personal");
            using (var connection = ConnectionProvider.GetConnection())
            {
                string sql = @"SELECT ap.*, 
                               e.nombre as NombreEmpleado,
                               j.nombre as NombreJefatura
                               FROM acciones_personal ap
                               LEFT JOIN empleados e ON ap.empleado_id = e.id
                               LEFT JOIN empleados j ON ap.jefatura_aprueba_id = j.id
                               ORDER BY ap.fecha DESC";
                return connection.Query<AccionPersonal>(sql).ToList();
            }
        }

        public static AccionPersonal? GetById(int id)
        {
            BitacoraRepository.RegistrarConsulta($"acción de personal {id}");
            using (var connection = ConnectionProvider.GetConnection())
            {
                string sql = @"SELECT ap.*, 
                               e.nombre as NombreEmpleado,
                               j.nombre as NombreJefatura
                               FROM acciones_personal ap
                               LEFT JOIN empleados e ON ap.empleado_id = e.id
                               LEFT JOIN empleados j ON ap.jefatura_aprueba_id = j.id
                               WHERE ap.id = @Id";
                return connection.QueryFirstOrDefault<AccionPersonal>(sql, new { Id = id });
            }
        }

        public static void Add(AccionPersonal accion)
        {
            using (var connection = ConnectionProvider.GetConnection())
            {
                int count = connection.ExecuteScalar<int>("SELECT COUNT(*) FROM accion_personal") + 1;
                accion.Codigo = $"ACT{count:D3}";
                accion.FechaCreacion = DateTime.Now;

                string sql = @"INSERT INTO accion_personal 
                               (codigo, fecha_accion, descripcion, id_empleado, id_jefatura) 
                               VALUES (@Codigo, @Fecha, @Descripcion, @EmpleadoId, @JefaturaApruebaId)";
                connection.Execute(sql, accion);
            }
            BitacoraRepository.RegistrarNueva(accion);
        }

        public static void Update(AccionPersonal accion)
        {
            var anterior = GetById(accion.Id);
            using (var connection = ConnectionProvider.GetConnection())
            {
                string sql = @"UPDATE accion_personal SET 
                               fecha_accion = @Fecha,
                               descripcion = @Descripcion,
                               id_empleado = @EmpleadoId,
                               id_jefatura = @JefaturaApruebaId
                               WHERE id_accion = @Id";
                connection.Execute(sql, accion);
            }
            if (anterior != null)
            {
                BitacoraRepository.RegistrarActualizacion(anterior, accion);
            }
        }

        public static void Delete(int id)
        {
            var anterior = GetById(id);
            using (var connection = ConnectionProvider.GetConnection())
            {
                string sql = "DELETE FROM accion_personal WHERE id_accion = @Id";
                connection.Execute(sql, new { Id = id });
            }
            if (anterior != null)
            {
                BitacoraRepository.RegistrarEliminacion(anterior);
            }
        }

        public static List<EmpleadoSimple> GetEmpleadosList()
        {
            BitacoraRepository.RegistrarConsulta("lista de empleados para acciones de personal");
            using (var connection = ConnectionProvider.GetConnection())
            {
                string sql = "SELECT id as Id, nombre as Nombre FROM empleados ORDER BY nombre";
                return connection.Query<EmpleadoSimple>(sql).ToList();
            }
        }
    }
}

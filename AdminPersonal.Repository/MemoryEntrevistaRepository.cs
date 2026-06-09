using System;
using System.Collections.Generic;
using System.Linq;
using AdminPersonal.Entities;
using MySqlConnector;
using Dapper;

namespace AdminPersonal.Repository
{
    public static class MemoryEntrevistaRepository
    {
        public static List<Entrevista> GetAll()
        {
            BitacoraRepository.RegistrarConsulta("todas las entrevistas");
            using (var connection = ConnectionProvider.GetConnection())
            {
                string sql = @"SELECT e.*, 
                               o.nombre as NombreOferente,
                               emp.nombre as NombreEntrevistador
                               FROM entrevistas e
                               LEFT JOIN oferentes o ON e.oferente_id = o.id
                               LEFT JOIN empleados emp ON e.empleado_entrevistador_id = emp.id
                               ORDER BY e.fecha_entrevista";
                return connection.Query<Entrevista>(sql).ToList();
            }
        }

        public static Entrevista? GetById(int id)
        {
            BitacoraRepository.RegistrarConsulta($"entrevista {id}");
            using (var connection = ConnectionProvider.GetConnection())
            {
                string sql = @"SELECT e.*, 
                               o.nombre as NombreOferente,
                               emp.nombre as NombreEntrevistador
                               FROM entrevistas e
                               LEFT JOIN oferentes o ON e.oferente_id = o.id
                               LEFT JOIN empleados emp ON e.empleado_entrevistador_id = emp.id
                               WHERE e.id = @Id";
                return connection.QueryFirstOrDefault<Entrevista>(sql, new { Id = id });
            }
        }

        public static void Add(Entrevista entrevista)
        {
            using (var connection = ConnectionProvider.GetConnection())
            {
                string sql = @"INSERT INTO entrevista 
                               (id_oferente, id_empleado, fecha_entrevista, estado) 
                               VALUES (@OferenteId, @EmpleadoEntrevistadorId, @FechaEntrevista, @Estado)";
                connection.Execute(sql, entrevista);
            }
            BitacoraRepository.RegistrarNueva(entrevista);
        }

        public static void Update(Entrevista entrevista)
        {
            var anterior = GetById(entrevista.Id);
            using (var connection = ConnectionProvider.GetConnection())
            {
                string sql = @"UPDATE entrevista SET 
                               id_empleado = @EmpleadoEntrevistadorId,
                               fecha_entrevista = @FechaEntrevista,
                               estado = @Estado
                               WHERE id_entrevista = @Id";
                connection.Execute(sql, entrevista);
            }
            if (anterior != null)
            {
                BitacoraRepository.RegistrarActualizacion(anterior, entrevista);
            }
        }

        public static void Delete(int id)
        {
            var anterior = GetById(id);
            using (var connection = ConnectionProvider.GetConnection())
            {
                string sql = "DELETE FROM entrevista WHERE id_entrevista = @Id";
                connection.Execute(sql, new { Id = id });
            }
            if (anterior != null)
            {
                BitacoraRepository.RegistrarEliminacion(anterior);
            }
        }

        public static void MarcarComoRealizada(int id)
        {
            var anterior = GetById(id);
            using (var connection = ConnectionProvider.GetConnection())
            {
                string sql = "UPDATE entrevista SET estado = 'Realizada' WHERE id_entrevista = @Id";
                connection.Execute(sql, new { Id = id });
            }
            if (anterior != null)
            {
                var actual = GetById(id);
                if (actual != null)
                {
                    BitacoraRepository.RegistrarActualizacion(anterior, actual);
                }
            }
        }

        public static List<OferenteItem> GetOferentesList()
        {
            BitacoraRepository.RegistrarConsulta("lista de oferentes para entrevistas");
            using (var connection = ConnectionProvider.GetConnection())
            {
                string sql = "SELECT id as Id, nombre as Nombre FROM oferentes ORDER BY nombre";
                return connection.Query<OferenteItem>(sql).ToList();
            }
        }

        public static List<EmpleadoItem> GetEmpleadosList()
        {
            BitacoraRepository.RegistrarConsulta("lista de empleados para entrevistas");
            using (var connection = ConnectionProvider.GetConnection())
            {
                string sql = "SELECT id as Id, nombre as Nombre FROM empleados ORDER BY nombre";
                return connection.Query<EmpleadoItem>(sql).ToList();
            }
        }
    }
}

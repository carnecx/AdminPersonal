using System;
using System.Collections.Generic;
using System.Linq;
using AdminPersonal.Entities;
using MySqlConnector;
using Dapper;

namespace AdminPersonal.Repository
{
    public static class MemoryExperienciaLaboralRepository
    {
        public static List<ExperienciaLaboral> GetByOferenteId(int oferenteId)
        {
            BitacoraRepository.RegistrarConsulta($"experiencia laboral del oferente {oferenteId}");
            using (var connection = ConnectionProvider.GetConnection())
            {
                string sql = @"SELECT el.*, o.nombre as NombreOferente
                               FROM experiencia_laboral el
                               LEFT JOIN oferentes o ON el.oferente_id = o.id
                               WHERE el.oferente_id = @OferenteId
                               ORDER BY el.fecha_inicio DESC";
                return connection.Query<ExperienciaLaboral>(sql, new { OferenteId = oferenteId }).ToList();
            }
        }

        public static ExperienciaLaboral? GetById(int id)
        {
            BitacoraRepository.RegistrarConsulta($"experiencia laboral {id}");
            using (var connection = ConnectionProvider.GetConnection())
            {
                string sql = @"SELECT el.*, o.nombre as NombreOferente
                               FROM experiencia_laboral el
                               LEFT JOIN oferentes o ON el.oferente_id = o.id
                               WHERE el.id = @Id";
                return connection.QueryFirstOrDefault<ExperienciaLaboral>(sql, new { Id = id });
            }
        }

        public static void Add(ExperienciaLaboral experiencia)
        {
            using (var connection = ConnectionProvider.GetConnection())
            {
                string sql = @"INSERT INTO experiencia_laboral 
                               (oferente_id, nombre_empresa, puesto_desempenado, fecha_inicio, fecha_fin) 
                               VALUES (@OferenteId, @NombreEmpresa, @PuestoDesempenado, @FechaInicio, @FechaFin)";
                connection.Execute(sql, experiencia);
            }
            BitacoraRepository.RegistrarNueva(experiencia);
        }

        public static void Update(ExperienciaLaboral experiencia)
        {
            var anterior = GetById(experiencia.Id);
            using (var connection = ConnectionProvider.GetConnection())
            {
                string sql = @"UPDATE experiencia_laboral SET 
                               nombre_empresa = @NombreEmpresa,
                               puesto_desempenado = @PuestoDesempenado,
                               fecha_inicio = @FechaInicio,
                               fecha_fin = @FechaFin
                               WHERE id = @Id";
                connection.Execute(sql, experiencia);
            }
            if (anterior != null)
            {
                BitacoraRepository.RegistrarActualizacion(anterior, experiencia);
            }
        }

        public static void Delete(int id)
        {
            var anterior = GetById(id);
            using (var connection = ConnectionProvider.GetConnection())
            {
                string sql = "DELETE FROM experiencia_laboral WHERE id = @Id";
                connection.Execute(sql, new { Id = id });
            }
            if (anterior != null)
            {
                BitacoraRepository.RegistrarEliminacion(anterior);
            }
        }
    }
}

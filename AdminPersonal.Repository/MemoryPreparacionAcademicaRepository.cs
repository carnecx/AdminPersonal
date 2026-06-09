using System;
using System.Collections.Generic;
using System.Linq;
using AdminPersonal.Entities;
using MySqlConnector;
using Dapper;

namespace AdminPersonal.Repository
{
    public static class MemoryPreparacionAcademicaRepository
    {
        public static List<PreparacionAcademica> GetByOferenteId(int oferenteId)
        {
            BitacoraRepository.RegistrarConsulta($"preparación académica del oferente {oferenteId}");
            using (var connection = ConnectionProvider.GetConnection())
            {
                string sql = @"SELECT pa.*, 
                               i.nombre as NombreInstitucion,
                               o.nombre as NombreOferente
                               FROM preparacion_academica pa
                               LEFT JOIN instituciones_educativas i ON pa.institucion_educativa_id = i.id
                               LEFT JOIN oferentes o ON pa.oferente_id = o.id
                               WHERE pa.oferente_id = @OferenteId
                               ORDER BY pa.fecha_inicio DESC";
                return connection.Query<PreparacionAcademica>(sql, new { OferenteId = oferenteId }).ToList();
            }
        }

        public static PreparacionAcademica? GetById(int id)
        {
            BitacoraRepository.RegistrarConsulta($"preparación académica {id}");
            using (var connection = ConnectionProvider.GetConnection())
            {
                string sql = @"SELECT pa.*, 
                               i.nombre as NombreInstitucion,
                               o.nombre as NombreOferente
                               FROM preparacion_academica pa
                               LEFT JOIN instituciones_educativas i ON pa.institucion_educativa_id = i.id
                               LEFT JOIN oferentes o ON pa.oferente_id = o.id
                               WHERE pa.id = @Id";
                return connection.QueryFirstOrDefault<PreparacionAcademica>(sql, new { Id = id });
            }
        }

        public static void Add(PreparacionAcademica preparacion)
        {
            using (var connection = ConnectionProvider.GetConnection())
            {
                string sql = @"INSERT INTO preparacion_academica 
                               (oferente_id, institucion_educativa_id, titulo, fecha_inicio, fecha_fin) 
                               VALUES (@OferenteId, @InstitucionEducativaId, @Titulo, @FechaInicio, @FechaFin)";
                connection.Execute(sql, preparacion);
            }
            BitacoraRepository.RegistrarNueva(preparacion);
        }

        public static void Update(PreparacionAcademica preparacion)
        {
            var anterior = GetById(preparacion.Id);
            using (var connection = ConnectionProvider.GetConnection())
            {
                string sql = @"UPDATE preparacion_academica SET 
                               institucion_educativa_id = @InstitucionEducativaId,
                               titulo = @Titulo,
                               fecha_inicio = @FechaInicio,
                               fecha_fin = @FechaFin
                               WHERE id = @Id";
                connection.Execute(sql, preparacion);
            }
            if (anterior != null)
            {
                BitacoraRepository.RegistrarActualizacion(anterior, preparacion);
            }
        }

        public static void Delete(int id)
        {
            var anterior = GetById(id);
            using (var connection = ConnectionProvider.GetConnection())
            {
                string sql = "DELETE FROM preparacion_academica WHERE id = @Id";
                connection.Execute(sql, new { Id = id });
            }
            if (anterior != null)
            {
                BitacoraRepository.RegistrarEliminacion(anterior);
            }
        }

        public static List<InstitucionSimple> GetInstitucionesList()
        {
            BitacoraRepository.RegistrarConsulta("lista de instituciones educativas");
            using (var connection = ConnectionProvider.GetConnection())
            {
                string sql = "SELECT id as Id, nombre as Nombre FROM instituciones_educativas ORDER BY nombre";
                return connection.Query<InstitucionSimple>(sql).ToList();
            }
        }
    }
}

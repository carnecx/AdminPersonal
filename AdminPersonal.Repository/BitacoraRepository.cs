using System;
using System.Text.Json;
using MySqlConnector;
using Dapper;

namespace AdminPersonal.Repository
{
    public static class BitacoraRepository
    {
        public static void Inicializar()
        {
            if (string.IsNullOrEmpty(ConnectionProvider.ConnectionString)) return;

            try
            {
                using (var connection = ConnectionProvider.GetConnection())
                {
                    string sql = @"
                        CREATE TABLE IF NOT EXISTS bitacora (
                            id INT AUTO_INCREMENT PRIMARY KEY,
                            fecha DATETIME NOT NULL,
                            usuario VARCHAR(100) NOT NULL,
                            descripcion TEXT NOT NULL
                        );";
                    connection.Execute(sql);
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("Advertencia: No se pudo conectar a la base de datos para inicializar la tabla de bitácora: " + ex.Message);
            }
        }

        public static void RegistrarNueva(object actual)
        {
            try
            {
                string json = JsonSerializer.Serialize(actual);
                InsertarBitacora("Registro nuevo: " + json);
            }
            catch {}
        }

        public static void RegistrarActualizacion(object anterior, object actual)
        {
            try
            {
                string jsonAnterior = JsonSerializer.Serialize(anterior);
                string jsonActual = JsonSerializer.Serialize(actual);
                InsertarBitacora($"Actualización - Anterior: {jsonAnterior} | Actual: {jsonActual}");
            }
            catch {}
        }

        public static void RegistrarEliminacion(object anterior)
        {
            try
            {
                string json = JsonSerializer.Serialize(anterior);
                InsertarBitacora("Eliminación: " + json);
            }
            catch {}
        }

        public static void RegistrarConsulta(string elemento)
        {
            try
            {
                InsertarBitacora($"El usuario consulta {elemento}");
            }
            catch {}
        }

        public static void RegistrarError(string error)
        {
            try
            {
                InsertarBitacora("ERROR TÉCNICO: " + error);
            }
            catch {}
        }

        private static void InsertarBitacora(string descripcion)
        {
            if (string.IsNullOrEmpty(ConnectionProvider.ConnectionString)) return;

            using (var connection = ConnectionProvider.GetConnection())
            {
                string sql = "INSERT INTO bitacora (fecha, usuario, descripcion) VALUES (@Fecha, @Usuario, @Descripcion)";
                connection.Execute(sql, new
                {
                    Fecha = DateTime.Now,
                    Usuario = "UsuarioActual", // default user
                    Descripcion = descripcion
                });
            }
        }
    }
}

using System.Data;
using MySqlConnector;
using Microsoft.Extensions.Configuration;

namespace AdminPersonal.Repository
{
    public class DbConnectionFactory : IDbConnectionFactory
    {
        private readonly IConfiguration _configuracion;

        public DbConnectionFactory(IConfiguration configuracion)
        {
            _configuracion = configuracion;
        }

        public IDbConnection CrearConexion()
        {
            return new MySqlConnection(_configuracion.GetConnectionString("DefaultConnection"));
        }
    }
}
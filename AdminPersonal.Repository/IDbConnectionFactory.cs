using System.Data;

namespace AdminPersonal.Repository
{
    public interface IDbConnectionFactory
    {
        IDbConnection CrearConexion();
    }
}
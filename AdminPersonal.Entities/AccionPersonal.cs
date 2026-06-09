using System;

namespace AdminPersonal.Entities
{
    public class AccionPersonal
    {
        public int Id { get; set; }
        public string Codigo { get; set; } = "";
        public DateTime Fecha { get; set; }
        public string Descripcion { get; set; } = "";
        public int EmpleadoId { get; set; }
        public string? NombreEmpleado { get; set; }
        public int JefaturaApruebaId { get; set; }
        public string? NombreJefatura { get; set; }
        public DateTime FechaCreacion { get; set; }
    }
}

using System;

namespace AdminPersonal.Entities
{
    public class Entrevista
    {
        public int Id { get; set; }
        public int OferenteId { get; set; }
        public string? NombreOferente { get; set; }
        public int EmpleadoEntrevistadorId { get; set; }
        public string? NombreEntrevistador { get; set; }
        public DateTime FechaEntrevista { get; set; }
        public string Estado { get; set; } = "Pendiente";
        public DateTime FechaCreacion { get; set; }
    }
}

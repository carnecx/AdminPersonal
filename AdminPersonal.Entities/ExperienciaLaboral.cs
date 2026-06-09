using System;

namespace AdminPersonal.Entities
{
    public class ExperienciaLaboral
    {
        public int Id { get; set; }
        public int OferenteId { get; set; }
        public string? NombreOferente { get; set; }
        public string NombreEmpresa { get; set; } = "";
        public string PuestoDesempenado { get; set; } = "";
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public DateTime FechaCreacion { get; set; }
    }
}

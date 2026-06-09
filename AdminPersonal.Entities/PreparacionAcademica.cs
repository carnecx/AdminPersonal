using System;

namespace AdminPersonal.Entities
{
    public class PreparacionAcademica
    {
        public int Id { get; set; }
        public int OferenteId { get; set; }
        public string? NombreOferente { get; set; }
        public int InstitucionEducativaId { get; set; }
        public string? NombreInstitucion { get; set; }
        public string Titulo { get; set; } = "";
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public DateTime FechaCreacion { get; set; }
    }
}

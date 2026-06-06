namespace AdminPersonal.Entities
{
    public class Concurso
    {
        public int id_concurso { get; set; }
        public string Codigo { get; set; } = "";
        public string Nombre { get; set; } = "";
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public string Estado { get; set; } = "Vigente";
    }
}
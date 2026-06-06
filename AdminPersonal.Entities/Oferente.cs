namespace AdminPersonal.Entities
{
    public class Oferente
    {
        public int id_oferente { get; set; }
        public string Identificacion { get; set; } = "";
        public string TipoIdentificacion { get; set; } = "";
        public string NombreCompleto { get; set; } = "";
        public DateTime FechaNacimiento { get; set; }
        public List<string> Correos { get; set; } = new();
        public List<string> Telefonos { get; set; } = new();
        public List<int> ConcursosIds { get; set; } = new();
    }
}
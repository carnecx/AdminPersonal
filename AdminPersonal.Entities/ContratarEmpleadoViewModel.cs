namespace AdminPersonal.Entities
{
    public class ContratarEmpleadoViewModel
    {
        public int IdOferente { get; set; }
        public string NombreOferente { get; set; } = string.Empty;
        public string Identificacion { get; set; } = string.Empty;
        public int IdPuesto { get; set; }
    }
}
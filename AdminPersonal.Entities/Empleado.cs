namespace AdminPersonal.Entities
{
    public class Empleado
    {
        public int IdEmpleado { get; set; }
        public string NumeroEmpleado { get; set; } = string.Empty;
        public int IdOferente { get; set; }
        public int IdPuesto { get; set; }
        public int? IdArea { get; set; }
    }
}
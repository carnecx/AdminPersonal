namespace AdminPersonal.Entities
{
    public class Puesto
    {
        public int Id { get; set; }
        public string Codigo { get; set; } = "";
        public string Nombre { get; set; } = "";
        public decimal Salario { get; set; }
        public int? JefaturaPuestoId { get; set; }
        public string? NombreJefatura { get; set; }
    }
}

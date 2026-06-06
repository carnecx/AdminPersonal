namespace AdminPersonal.Entities
{
    public class Puesto
    {
        public int IdPuesto { get; set; }
        public string Codigo { get; set; } = "";
        public string Nombre { get; set; } = "";
        public decimal Salario { get; set; }
        public int? IdPuestoJefatura { get; set; }
    }
}
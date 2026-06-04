namespace AdminPersonal.Entities
{
    public class RequisitoPuesto
    {
        public int id_requisito { get; set; }
        public string nombre_requisito { get; set; } = "";
        public int id_puesto { get; set; }

        public string nombre_puesto { get; set; } = "";
    }
}

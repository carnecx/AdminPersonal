namespace AdminPersonal.Entities
{
    public class Bitacora
    {
        public int id_bitacora { get; set; }
        public DateTime fecha { get; set; }
        public int id_usuario { get; set; }
        public string nombre_usuario { get; set; } = "";
        public string descripcion { get; set; } = "";
    }
}

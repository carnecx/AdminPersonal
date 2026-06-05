namespace AdminPersonal.Entities
{
    public class RolPantalla
    {
        public int id_rol { get; set; }
        public int id_pantalla { get; set; }
        public string nombre_rol { get; set; } = "";
        public bool Seleccionado { get; set; }
    }
}
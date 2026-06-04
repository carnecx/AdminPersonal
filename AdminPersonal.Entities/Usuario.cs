namespace AdminPersonal.Entities
{
    public class Usuario
    {
        public int id_usuario { get; set; }
        public string nombre_usuario { get; set; } = "";
        public string nombre_completo { get; set; } = "";
        public string correo { get; set; } = "";
        public string contrasena { get; set; } = "";
        public string estado { get; set; } = "Activo";
        public int intentos_fallidos { get; set; }
    }
}

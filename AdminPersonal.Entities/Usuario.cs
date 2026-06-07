namespace AdminPersonal.Entities
{
    // modelo que representa un usuario del sistema
    public class Usuario
    {
        // identificador unico del usuario
        public int IdUsuario { get; set; }

        // nombre utilizado para iniciar sesion
        public string NombreUsuario { get; set; } = string.Empty;

        // nombre completo de la persona
        public string NombreCompleto { get; set; } = string.Empty;

        // correo electronico del usuario
        public string Correo { get; set; } = string.Empty;

        // contraseña del usuario
        // se almacena en formato encriptado en la base de datos
        public string Contrasena { get; set; } = string.Empty;

        // estado actual del usuario
        // puede ser activo, inactivo o bloqueado
        public string Estado { get; set; } = "Activo";

        // cantidad de intentos fallidos de inicio de sesion
        public int IntentosFallidos { get; set; } = 0;

        // lista de roles seleccionados para el usuario
        // se utiliza al crear o editar usuarios
        public List<int> RolesSeleccionados { get; set; } = new();

        // nombres de los roles asociados al usuario
        // se utiliza para mostrar los roles en los listados
        public string RolesNombres { get; set; } = string.Empty;

        // propiedades auxiliares para mantener compatibilidad
        // con consultas y codigo que utiliza nombres en minuscula

        public int id_usuario => IdUsuario;

        public string nombre_usuario => NombreUsuario;

        public string nombre_completo => NombreCompleto;

        public string contrasena => Contrasena;

        public string estado => Estado;
    }
}
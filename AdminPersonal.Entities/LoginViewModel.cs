using System.ComponentModel.DataAnnotations;

namespace AdminPersonal.Entities
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "El usuario es requerido")]
        public string NombreUsuario { get; set; } = "";

        [Required(ErrorMessage = "La contraseña es requerida")]
        public string Contrasena { get; set; } = "";
    }
}

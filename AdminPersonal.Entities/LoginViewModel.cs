using System.ComponentModel.DataAnnotations;

namespace AdminPersonal.Entities
{
    // modelo utilizado para capturar los datos del formulario de inicio de sesion
    public class LoginViewModel
    {
        // nombre de usuario ingresado por la persona que intenta iniciar sesion
        // es un campo obligatorio
        [Required(ErrorMessage = "El usuario es requerido")]
        public string NombreUsuario { get; set; } = "";

        // contraseña ingresada por el usuario
        // es un campo obligatorio
        [Required(ErrorMessage = "La contraseña es requerida")]
        public string Contrasena { get; set; } = "";
    }
}
using System.ComponentModel.DataAnnotations;

namespace AdminPersonal.Entities
{
    // modelo que representa una institucion educativa dentro del sistema
    public class InstitucionEducativa
    {
        // identificador unico de la institucion
        public int id_institucion { get; set; }

        // codigo de la institucion
        // es obligatorio para registrar una institucion
        [Required(ErrorMessage = "El codigo es requerido")]
        public string codigo { get; set; } = "";

        // nombre de la institucion
        // es obligatorio
        [Required(ErrorMessage = "El nombre es requerido")]

        // limita el nombre a un maximo de 150 caracteres
        [StringLength(150, ErrorMessage = "El nombre maximo es de 150 caracteres")]

        // valida que solo se permitan letras y espacios
        [RegularExpression(
            @"^[A-Za-záéíóúÁÉÍÓÚñÑ ]+$",
            ErrorMessage = "El nombre solo debe tener letras y espacios")]
        public string nombre { get; set; } = "";
    }
}
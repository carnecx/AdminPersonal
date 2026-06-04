using System.ComponentModel.DataAnnotations;

namespace AdminPersonal.Entities
{
    public class InstitucionEducativa
    {
        public int id_institucion { get; set; }

        [Required(ErrorMessage = "El codigo es requerido")]
        public string codigo { get; set; } = "";

        [Required(ErrorMessage = "El nombre es requerido")]
        [StringLength(150, ErrorMessage = "El nombre maximo es de 150 caracteres")]
        [RegularExpression(@"^[A-Za-záéíóúÁÉÍÓÚñÑ ]+$", ErrorMessage = "El nombre solo debe tener letras y espacios")]
        public string nombre { get; set; } = "";
    }
}

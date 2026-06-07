using Microsoft.AspNetCore.Http;

namespace AdminPersonal.Entities
{
    // modelo utilizado para recibir el archivo csv cargado por el usuario
    public class UbicacionUploadViewModel
    {
        // representa el archivo seleccionado desde el formulario web
        // normalmente corresponde a un archivo csv con provincias, cantones y distritos
        public IFormFile? Archivo { get; set; }
    }
}
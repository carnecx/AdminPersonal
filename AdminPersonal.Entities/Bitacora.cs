namespace AdminPersonal.Entities
{
    // modelo que representa un registro de la tabla bitacora
    public class Bitacora
    {
        // identificador unico del registro de bitacora
        public int id_bitacora { get; set; }

        // fecha y hora en que se realizo la accion
        public DateTime fecha { get; set; }

        // identificador del usuario que realizo la accion
        public int id_usuario { get; set; }

        // nombre del usuario que realizo la accion
        public string nombre_usuario { get; set; } = "";

        // descripcion de la accion realizada dentro del sistema
        public string descripcion { get; set; } = "";
    }
}
using System.Security.Cryptography;
using System.Text;

namespace AdminPersonal.Services
{
    // servicio encargado de encriptar, desencriptar y validar contraseñas
    public class PasswordService
    {
        // clave utilizada por el algoritmo aes gcm
        // debe tener 32 caracteres para trabajar con aes 256
        private readonly string clave = "12345678901234567890123456789012";

        // encripta una contraseña antes de almacenarla en la base de datos
        public string Encriptar(string texto)
        {
            // convierte la clave a bytes
            byte[] key = Encoding.UTF8.GetBytes(clave);

            // genera un nonce aleatorio para aumentar la seguridad
            byte[] nonce = RandomNumberGenerator.GetBytes(12);

            // convierte la contraseña a bytes
            byte[] textoBytes = Encoding.UTF8.GetBytes(texto);

            // arreglo donde se almacenara el texto cifrado
            byte[] cifrado = new byte[textoBytes.Length];

            // etiqueta de autenticacion utilizada por aes gcm
            byte[] tag = new byte[16];

            // crea una instancia del algoritmo aes gcm
            using var aes = new AesGcm(key, 16);

            // realiza el proceso de cifrado
            aes.Encrypt(nonce, textoBytes, cifrado, tag);

            // devuelve nonce, texto cifrado y tag en formato base64
            return Convert.ToBase64String(nonce) + "." +
                   Convert.ToBase64String(cifrado) + "." +
                   Convert.ToBase64String(tag);
        }

        // desencripta una contraseña almacenada en la base de datos
        public string Desencriptar(string textoEncriptado)
        {
            // convierte la clave a bytes
            byte[] key = Encoding.UTF8.GetBytes(clave);

            // separa las tres partes almacenadas
            string[] partes = textoEncriptado.Split('.');

            // obtiene nonce, texto cifrado y tag
            byte[] nonce = Convert.FromBase64String(partes[0]);
            byte[] cifrado = Convert.FromBase64String(partes[1]);
            byte[] tag = Convert.FromBase64String(partes[2]);

            // arreglo donde se almacenara el texto original
            byte[] texto = new byte[cifrado.Length];

            // crea una instancia de aes gcm
            using var aes = new AesGcm(key, 16);

            // realiza el proceso de descifrado
            aes.Decrypt(nonce, cifrado, tag, texto);

            // convierte los bytes nuevamente a texto
            return Encoding.UTF8.GetString(texto);
        }

        // valida que la contraseña digitada coincida con la almacenada
        public bool Verify(string contrasenaDigitada, string contrasenaBD)
        {
            try
            {
                // desencripta la contraseña guardada en la base de datos
                string contrasenaOriginal = Desencriptar(contrasenaBD);

                // compara ambas contraseñas
                return contrasenaDigitada == contrasenaOriginal;
            }
            catch
            {
                // si ocurre un error durante la validacion devuelve false
                return false;
            }
        }
    }
}
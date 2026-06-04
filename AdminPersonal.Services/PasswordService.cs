using System.Security.Cryptography;
using System.Text;

namespace AdminPersonal.Services
{
    public class PasswordService
    {
        private readonly string clave = "12345678901234567890123456789012";

        public string Encriptar(string texto)
        {
            byte[] key = Encoding.UTF8.GetBytes(clave);
            byte[] nonce = RandomNumberGenerator.GetBytes(12);
            byte[] textoBytes = Encoding.UTF8.GetBytes(texto);
            byte[] cifrado = new byte[textoBytes.Length];
            byte[] tag = new byte[16];

            using var aes = new AesGcm(key, 16);
            aes.Encrypt(nonce, textoBytes, cifrado, tag);

            return Convert.ToBase64String(nonce) + "." +
                   Convert.ToBase64String(cifrado) + "." +
                   Convert.ToBase64String(tag);
        }

        public string Desencriptar(string textoEncriptado)
        {
            byte[] key = Encoding.UTF8.GetBytes(clave);
            string[] partes = textoEncriptado.Split('.');
            byte[] nonce = Convert.FromBase64String(partes[0]);
            byte[] cifrado = Convert.FromBase64String(partes[1]);
            byte[] tag = Convert.FromBase64String(partes[2]);
            byte[] texto = new byte[cifrado.Length];

            using var aes = new AesGcm(key, 16);
            aes.Decrypt(nonce, cifrado, tag, texto);

            return Encoding.UTF8.GetString(texto);
        }

        public bool Verify(string contrasenaDigitada, string contrasenaBD)
        {
            try
            {
                return contrasenaDigitada == Desencriptar(contrasenaBD);
            }
            catch
            {
                return false;
            }
        }
    }
}

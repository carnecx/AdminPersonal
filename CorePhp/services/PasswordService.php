<?php

// servicio encargado de validar contrasenas
class PasswordService
{
    // misma llave usada en el proyecto c#
    private string $aesKey = "AdminPersonalKey1AdminPersonalK1";

    // valida la contrasena digitada contra la contrasena almacenada
    public function validar(string $contrasenaDigitada, string $contrasenaBD): bool
    {
        if ($contrasenaDigitada === "" || $contrasenaBD === "") {
            return false;
        }

        // intenta validar como aes gcm segun el formato usado en c#
        $desencriptada = $this->desencriptarAesGcm($contrasenaBD);

        if ($desencriptada !== null) {
            return hash_equals($desencriptada, $contrasenaDigitada);
        }

        // soporte por si en alguna prueba usan hash de php
        if (password_verify($contrasenaDigitada, $contrasenaBD)) {
            return true;
        }

        // soporte solo para pruebas locales con contrasenas sin cifrar
        return hash_equals($contrasenaBD, $contrasenaDigitada);
    }

    // desencripta contrasena aes gcm guardada como base64 nonce + tag + cipher
    private function desencriptarAesGcm(string $cipherText): ?string
    {
        $combined = base64_decode($cipherText, true);

        if ($combined === false) {
            return null;
        }

        $nonceSize = 12;
        $tagSize = 16;

        if (strlen($combined) <= ($nonceSize + $tagSize)) {
            return null;
        }

        $nonce = substr($combined, 0, $nonceSize);
        $tag = substr($combined, $nonceSize, $tagSize);
        $cipher = substr($combined, $nonceSize + $tagSize);

        $plain = openssl_decrypt(
            $cipher,
            "aes-256-gcm",
            $this->aesKey,
            OPENSSL_RAW_DATA,
            $nonce,
            $tag
        );

        if ($plain === false) {
            return null;
        }

        return $plain;
    }
}

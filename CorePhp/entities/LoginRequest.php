<?php

// entidad que representa la solicitud de login enviada al servicio
class LoginRequest
{
    public string $usuario;
    public string $contrasena;

    public function __construct(array $data)
    {
        $this->usuario = trim($data["usuario"] ?? "");
        $this->contrasena = trim($data["contrasena"] ?? "");
    }
}

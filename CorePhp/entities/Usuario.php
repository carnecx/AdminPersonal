<?php

// entidad que representa un usuario del sistema
class Usuario
{
    public int $idUsuario;
    public string $nombreUsuario;
    public string $nombreCompleto;
    public string $correo;
    public string $contrasena;
    public string $estado;
    public int $intentosFallidos;

    // constructor que carga los datos desde la base de datos
    public function __construct(array $data)
    {
        $this->idUsuario = (int)($data["IdUsuario"] ?? 0);
        $this->nombreUsuario = $data["NombreUsuario"] ?? "";
        $this->nombreCompleto = $data["NombreCompleto"] ?? "";
        $this->correo = $data["Correo"] ?? "";
        $this->contrasena = $data["Contrasena"] ?? "";
        $this->estado = $data["Estado"] ?? "";
        $this->intentosFallidos = (int)($data["IntentosFallidos"] ?? 0);
    }
}

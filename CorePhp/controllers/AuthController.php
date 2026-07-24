<?php

require_once __DIR__ . "/../config/Database.php";
require_once __DIR__ . "/../entities/LoginRequest.php";
require_once __DIR__ . "/../repositories/UsuarioRepository.php";
require_once __DIR__ . "/../services/PasswordService.php";
require_once __DIR__ . "/../services/UsuarioService.php";

// controlador encargado de recibir solicitudes de autenticacion
class AuthController
{
    private UsuarioService $usuarioService;

    public function __construct()
    {
        // crea conexion
        $database = new Database();
        $connection = $database->getConnection();

        // crea repositorio y servicios
        $repository = new UsuarioRepository($connection);
        $passwordService = new PasswordService();

        $this->usuarioService = new UsuarioService($repository, $passwordService);
    }

    // procesa el login enviado al servicio
    public function login(array $request): array
    {
        $datos = new LoginRequest($request);

        return $this->usuarioService->autenticar(
            $datos->usuario,
            $datos->contrasena
        );
    }
}

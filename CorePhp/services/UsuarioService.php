<?php

require_once __DIR__ . "/../repositories/UsuarioRepository.php";
require_once __DIR__ . "/PasswordService.php";

// servicio encargado de la logica de autenticacion del core
class UsuarioService
{
    private UsuarioRepository $repository;
    private PasswordService $passwordService;

    public function __construct(UsuarioRepository $repository, PasswordService $passwordService)
    {
        $this->repository = $repository;
        $this->passwordService = $passwordService;
    }

    // autentica un usuario contra la base de datos
    public function autenticar(string $nombreUsuario, string $contrasena): array
    {
        // valida datos requeridos
        if (trim($nombreUsuario) === "" || trim($contrasena) === "") {
            return [
                "ok" => false,
                "mensaje" => "Usuario y/o contraseña incorrectos."
            ];
        }

        // busca el usuario en la bd
        $usuario = $this->repository->buscarPorUsuario($nombreUsuario);

        // valida si existe
        if ($usuario === null) {
            return [
                "ok" => false,
                "mensaje" => "Usuario y/o contraseña incorrectos."
            ];
        }

        // valida usuario bloqueado
        if ($usuario->estado === "Bloqueado") {
            return [
                "ok" => false,
                "mensaje" => "El usuario se encuentra bloqueado."
            ];
        }

        // valida usuario inactivo
        if ($usuario->estado === "Inactivo") {
            return [
                "ok" => false,
                "mensaje" => "El usuario se encuentra inactivo."
            ];
        }

        // valida contrasena
        if (!$this->passwordService->validar($contrasena, $usuario->contrasena)) {

            // obtiene el maximo de intentos desde parametros
            $maxIntentos = $this->repository->obtenerParametroEntero("INTENTOS_LOGIN_MAX", 3);

            // registra intento fallido
            $this->repository->registrarFallo($usuario, $maxIntentos);

            // registra error en bitacora
            $this->repository->registrarBitacora(
                $usuario->idUsuario,
                json_encode([
                    "accion" => "login core",
                    "resultado" => "credenciales incorrectas",
                    "usuario" => $usuario->nombreUsuario
                ], JSON_UNESCAPED_UNICODE)
            );

            return [
                "ok" => false,
                "mensaje" => "Usuario y/o contraseña incorrectos."
            ];
        }

        // reinicia intentos fallidos
        $this->repository->reiniciarIntentos($usuario->idUsuario);

        // obtiene rol
        $rol = $this->repository->obtenerRol($usuario->idUsuario);

        // registra login correcto en bitacora
        $this->repository->registrarBitacora(
            $usuario->idUsuario,
            json_encode([
                "accion" => "login core",
                "resultado" => "autenticacion correcta",
                "usuario" => $usuario->nombreUsuario
            ], JSON_UNESCAPED_UNICODE)
        );

        // retorna respuesta correcta
        return [
            "ok" => true,
            "mensaje" => "Autenticado correctamente.",
            "usuario" => [
                "idUsuario" => $usuario->idUsuario,
                "nombreUsuario" => $usuario->nombreUsuario,
                "nombreCompleto" => $usuario->nombreCompleto,
                "correo" => $usuario->correo,
                "rol" => $rol
            ]
        ];
    }
}

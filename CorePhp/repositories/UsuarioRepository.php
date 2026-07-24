<?php

require_once __DIR__ . "/../entities/Usuario.php";

// repositorio encargado de consultar y actualizar usuarios en la base de datos
class UsuarioRepository
{
    // conexion a mysql
    private PDO $connection;

    // constructor que recibe la conexion
    public function __construct(PDO $connection)
    {
        $this->connection = $connection;
    }

    // busca un usuario por nombre de usuario
    public function buscarPorUsuario(string $nombreUsuario): ?Usuario
    {
        $sql = "SELECT id_usuario AS IdUsuario,
                       nombre_usuario AS NombreUsuario,
                       nombre_completo AS NombreCompleto,
                       correo AS Correo,
                       contrasena AS Contrasena,
                       estado AS Estado,
                       intentos_fallidos AS IntentosFallidos
                FROM usuario
                WHERE nombre_usuario = :nombreUsuario
                LIMIT 1";

        $stmt = $this->connection->prepare($sql);
        $stmt->execute([
            ":nombreUsuario" => $nombreUsuario
        ]);

        $data = $stmt->fetch();

        if (!$data) {
            return null;
        }

        return new Usuario($data);
    }

    // registra un intento fallido y bloquea si llega al maximo permitido
    public function registrarFallo(Usuario $usuario, int $maxIntentos): void
    {
        $intentos = $usuario->intentosFallidos + 1;

        $estado = $intentos >= $maxIntentos
            ? "Bloqueado"
            : $usuario->estado;

        $sql = "UPDATE usuario
                SET intentos_fallidos = :intentos,
                    estado = :estado
                WHERE id_usuario = :idUsuario";

        $stmt = $this->connection->prepare($sql);
        $stmt->execute([
            ":intentos" => $intentos,
            ":estado" => $estado,
            ":idUsuario" => $usuario->idUsuario
        ]);
    }

    // reinicia los intentos fallidos despues de un login correcto
    public function reiniciarIntentos(int $idUsuario): void
    {
        $sql = "UPDATE usuario
                SET intentos_fallidos = 0
                WHERE id_usuario = :idUsuario";

        $stmt = $this->connection->prepare($sql);
        $stmt->execute([
            ":idUsuario" => $idUsuario
        ]);
    }

    // obtiene el primer rol asignado al usuario
    public function obtenerRol(int $idUsuario): ?string
    {
        $sql = "SELECT r.nombre_rol
                FROM rol r
                INNER JOIN usuario_rol ur ON ur.id_rol = r.id_rol
                WHERE ur.id_usuario = :idUsuario
                LIMIT 1";

        $stmt = $this->connection->prepare($sql);
        $stmt->execute([
            ":idUsuario" => $idUsuario
        ]);

        $rol = $stmt->fetchColumn();

        return $rol ? (string)$rol : null;
    }

    // obtiene un parametro entero desde la tabla parametro
    public function obtenerParametroEntero(string $codigo, int $valorPorDefecto): int
    {
        $sql = "SELECT valor
                FROM parametro
                WHERE codigo = :codigo
                LIMIT 1";

        $stmt = $this->connection->prepare($sql);
        $stmt->execute([
            ":codigo" => $codigo
        ]);

        $valor = $stmt->fetchColumn();

        if ($valor === false || (int)$valor <= 0) {
            return $valorPorDefecto;
        }

        return (int)$valor;
    }

    // registra una accion en la bitacora
    public function registrarBitacora(int $idUsuario, string $descripcion): void
    {
        $sql = "INSERT INTO bitacora (id_usuario, descripcion)
                VALUES (:idUsuario, :descripcion)";

        $stmt = $this->connection->prepare($sql);
        $stmt->execute([
            ":idUsuario" => $idUsuario,
            ":descripcion" => $descripcion
        ]);
    }
}

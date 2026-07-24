<?php

// clase encargada de crear la conexion a la base de datos
class Database
{
    // datos de conexion a mysql
    private string $host = "localhost";
    private string $dbName = "adminpersonal";
    private string $user = "root";
    private string $password = "root123";

    // variable donde se guarda la conexion
    private ?PDO $connection = null;

    // crea y retorna la conexion a mysql
    public function getConnection(): PDO
    {
        // valida si la conexion aun no ha sido creada
        if ($this->connection === null) {

            // cadena de conexion para mysql
            $dsn = "mysql:host={$this->host};dbname={$this->dbName};charset=utf8mb4";

            // crea la conexion con pdo
            $this->connection = new PDO($dsn, $this->user, $this->password);

            // configura el manejo de errores
            $this->connection->setAttribute(PDO::ATTR_ERRMODE, PDO::ERRMODE_EXCEPTION);

            // configura que los resultados se devuelvan como arreglo asociativo
            $this->connection->setAttribute(PDO::ATTR_DEFAULT_FETCH_MODE, PDO::FETCH_ASSOC);
        }

        // retorna la conexion
        return $this->connection;
    }
}

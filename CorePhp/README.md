# CorePhp - Avance 2 Persona 1

Este proyecto corresponde a la parte de Persona 1 para el avance 2.

## Historias cubiertas

- CORE4: servicio web para autenticar usuarios.
- CORE5: pantalla de login del sistema Core usando el servicio CORE4.

## Estructura

```txt
CorePhp
├── api
├── config
├── controllers
├── entities
├── repositories
├── services
├── utils
├── pages
├── public
└── docs
```

## Configuración

Editar el archivo:

```txt
config/Database.php
```

y colocar los datos correctos de MySQL:

```php
private string $host = "localhost";
private string $dbName = "adminpersonal";
private string $user = "root";
private string $password = "";
```

## Probar CORE4

Endpoint:

```txt
POST /CorePhp/api/auth.php
```

Body:

```json
{
  "usuario": "admin",
  "contrasena": "123"
}
```

## Probar CORE5

Abrir en el navegador:

```txt
/CorePhp/pages/login.php
```

## Nota importante

El servicio valida contraseñas AES GCM usando la misma llave que el proyecto C#:

```txt
AdminPersonalKey1AdminPersonalK1
```

# pruebas técnicas core4 y core5

## core4 servicio de autenticación

### prueba 1
criterio: el servicio debe solicitar usuario y contraseña  
evidencia: enviar post a api/auth.php con usuario y contraseña

### prueba 2
criterio: si las credenciales son incorrectas debe indicar usuario y/o contraseña incorrectos  
evidencia: enviar contraseña incorrecta y verificar respuesta json

### prueba 3
criterio: si hay 3 intentos fallidos el usuario debe bloquearse  
evidencia: revisar en base de datos estado bloqueado e intentos_fallidos en 3

### prueba 4
criterio: contraseña encriptada en bd  
evidencia: verificar campo contrasena en tabla usuario y validar login correcto

## core5 pantalla de login

### prueba 5
criterio: la pantalla debe solicitar usuario y contraseña  
evidencia: pantallazo de pages/login.php

### prueba 6
criterio: debe consumir el servicio core4  
evidencia: login correcto desde la pantalla

### prueba 7
criterio: debe mostrar mensaje de error cuando los datos son incorrectos  
evidencia: pantallazo del mensaje en la pantalla

<?php
// url del servicio de autenticacion core4
$apiUrl = "../api/auth.php";
?>

<!DOCTYPE html>
<html lang="es">
<head>
    <meta charset="UTF-8">
    <title>Login Core</title>
    <link rel="stylesheet" href="../public/css/site.css">
</head>
<body>

<div class="login-container">
    <div class="login-card">

        <div class="logo">SM</div>

        <h2>Servicios Medicos SA</h2>
        <p>Ingreso al sistema Core</p>

        <div id="mensaje" class="mensaje"></div>

        <form id="loginForm">
            <label>Usuario</label>
            <input type="text" id="usuario" name="usuario" required>

            <label>Contraseña</label>
            <input type="password" id="contrasena" name="contrasena" required>

            <button type="submit">Aceptar</button>
        </form>

    </div>
</div>

<script>
const apiUrl = "<?php echo $apiUrl; ?>";
</script>
<script src="../public/js/login.js"></script>

</body>
</html>

document.getElementById("loginForm").addEventListener("submit", async function (event) {
    event.preventDefault();

    const mensaje = document.getElementById("mensaje");

    mensaje.className = "mensaje";
    mensaje.innerText = "";

    const usuario = document.getElementById("usuario").value;
    const contrasena = document.getElementById("contrasena").value;

    try {
        const response = await fetch(apiUrl, {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify({
                usuario: usuario,
                contrasena: contrasena
            })
        });

        const data = await response.json();

        if (data.ok) {
            mensaje.className = "mensaje exito";
            mensaje.innerText = data.mensaje + " Bienvenido " + data.usuario.nombreCompleto;
        } else {
            mensaje.className = "mensaje error";
            mensaje.innerText = data.mensaje;
        }

    } catch (error) {
        mensaje.className = "mensaje error";
        mensaje.innerText = "No se pudo conectar con el servicio.";
    }
});

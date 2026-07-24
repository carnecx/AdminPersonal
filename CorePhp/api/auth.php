<?php

require_once __DIR__ . "/../controllers/AuthController.php";
require_once __DIR__ . "/../utils/Response.php";

// permite que wordpress consuma el servicio
header("Access-Control-Allow-Origin: *");
header("Access-Control-Allow-Headers: Content-Type");
header("Access-Control-Allow-Methods: POST, OPTIONS");

// responde solicitudes preflight
if ($_SERVER["REQUEST_METHOD"] === "OPTIONS") {
    http_response_code(200);
    exit;
}

// valida metodo post
if ($_SERVER["REQUEST_METHOD"] !== "POST") {
    Response::json([
        "ok" => false,
        "mensaje" => "Metodo no permitido."
    ], 405);
}

// obtiene json enviado
$body = file_get_contents("php://input");
$request = json_decode($body, true);

// valida json
if (!is_array($request)) {
    Response::json([
        "ok" => false,
        "mensaje" => "Solicitud invalida."
    ], 400);
}

try {
    // ejecuta controlador
    $controller = new AuthController();
    $response = $controller->login($request);

    // retorna respuesta
    Response::json($response, $response["ok"] ? 200 : 401);

} catch (Throwable $ex) {

    // retorna error tecnico controlado
    Response::json([
        "ok" => false,
        "mensaje" => "Error tecnico en el servicio.",
        "detalle" => $ex->getMessage()
    ], 500);
}

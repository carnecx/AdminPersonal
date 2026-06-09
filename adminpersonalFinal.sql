CREATE DATABASE IF NOT EXISTS adminpersonal;
USE adminpersonal;

CREATE TABLE IF NOT EXISTS rol (
  id_rol        INT AUTO_INCREMENT PRIMARY KEY,
  nombre_rol    VARCHAR(40) NOT NULL,
  CONSTRAINT chk_nombre_rol CHECK (nombre_rol REGEXP '^[A-Za-záéíóúÁÉÍÓÚñÑ ]+$')
);

CREATE TABLE IF NOT EXISTS pantalla (
  id_pantalla     INT AUTO_INCREMENT PRIMARY KEY,
  nombre_pantalla VARCHAR(100) NOT NULL,
  CONSTRAINT chk_nombre_pantalla CHECK (nombre_pantalla REGEXP '^[A-Za-záéíóúÁÉÍÓÚñÑ ]+$')
);

CREATE TABLE IF NOT EXISTS rol_pantalla (
  id_rol      INT NOT NULL,
  id_pantalla INT NOT NULL,
  PRIMARY KEY (id_rol, id_pantalla),
  CONSTRAINT fk_rolpantalla_rol      FOREIGN KEY (id_rol)      REFERENCES rol(id_rol),
  CONSTRAINT fk_rolpantalla_pantalla FOREIGN KEY (id_pantalla) REFERENCES pantalla(id_pantalla)
);

CREATE TABLE IF NOT EXISTS usuario (
  id_usuario        INT AUTO_INCREMENT PRIMARY KEY,
  nombre_usuario    VARCHAR(100) NOT NULL UNIQUE,
  nombre_completo   VARCHAR(150) NOT NULL,
  correo            VARCHAR(150) NOT NULL,
  contrasena        VARCHAR(255) NOT NULL,
  estado            ENUM('Activo','Inactivo','Bloqueado') NOT NULL DEFAULT 'Activo',
  intentos_fallidos TINYINT NOT NULL DEFAULT 0
);

CREATE TABLE IF NOT EXISTS usuario_rol (
  id_usuario INT NOT NULL,
  id_rol     INT NOT NULL,
  PRIMARY KEY (id_usuario, id_rol),
  CONSTRAINT fk_usuariorol_usuario FOREIGN KEY (id_usuario) REFERENCES usuario(id_usuario),
  CONSTRAINT fk_usuariorol_rol     FOREIGN KEY (id_rol)     REFERENCES rol(id_rol)
);

CREATE TABLE IF NOT EXISTS parametro (
  id_parametro INT AUTO_INCREMENT PRIMARY KEY,
  codigo       VARCHAR(100) NOT NULL UNIQUE,
  valor        VARCHAR(500) NOT NULL
);

CREATE TABLE IF NOT EXISTS compania (
  id_compania INT AUTO_INCREMENT PRIMARY KEY,
  codigo      VARCHAR(100) NOT NULL UNIQUE,
  nombre      VARCHAR(150) NOT NULL
);

CREATE TABLE IF NOT EXISTS bitacora (
  id_bitacora INT AUTO_INCREMENT PRIMARY KEY,
  fecha       DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
  id_usuario  INT      NOT NULL,
  descripcion TEXT     NOT NULL,
  CONSTRAINT fk_bitacora_usuario FOREIGN KEY (id_usuario) REFERENCES usuario(id_usuario)
);

CREATE TABLE IF NOT EXISTS provincia (
  id_provincia INT AUTO_INCREMENT PRIMARY KEY,
  nombre       VARCHAR(100) NOT NULL
);

CREATE TABLE IF NOT EXISTS canton (
  id_canton    INT AUTO_INCREMENT PRIMARY KEY,
  nombre       VARCHAR(100) NOT NULL,
  id_provincia INT NOT NULL,
  CONSTRAINT fk_canton_provincia FOREIGN KEY (id_provincia) REFERENCES provincia(id_provincia)
);

CREATE TABLE IF NOT EXISTS distrito (
  id_distrito INT AUTO_INCREMENT PRIMARY KEY,
  nombre      VARCHAR(100) NOT NULL,
  id_canton   INT NOT NULL,
  CONSTRAINT fk_distrito_canton FOREIGN KEY (id_canton) REFERENCES canton(id_canton)
);

CREATE TABLE IF NOT EXISTS institucion_educativa (
  id_institucion INT AUTO_INCREMENT PRIMARY KEY,
  codigo         VARCHAR(100) NOT NULL UNIQUE,
  nombre         VARCHAR(150) NOT NULL,
  CONSTRAINT chk_nombre_inst CHECK (nombre REGEXP '^[A-Za-záéíóúÁÉÍÓÚñÑ ]+$')
);

CREATE TABLE IF NOT EXISTS puesto (
  id_puesto          INT AUTO_INCREMENT PRIMARY KEY,
  codigo             VARCHAR(50)    NOT NULL UNIQUE,
  nombre             VARCHAR(150)   NOT NULL,
  salario            DECIMAL(12, 2) NOT NULL,
  id_puesto_jefatura INT            NULL,
  CONSTRAINT fk_puesto_jefatura FOREIGN KEY (id_puesto_jefatura) REFERENCES puesto(id_puesto)
);

CREATE TABLE IF NOT EXISTS requisito_puesto (
  id_requisito     INT AUTO_INCREMENT PRIMARY KEY,
  nombre_requisito VARCHAR(200) NOT NULL,
  id_puesto        INT          NOT NULL,
  CONSTRAINT fk_requisito_puesto FOREIGN KEY (id_puesto) REFERENCES puesto(id_puesto)
);

CREATE TABLE IF NOT EXISTS area (
  id_area     INT AUTO_INCREMENT PRIMARY KEY,
  codigo      VARCHAR(50)  NOT NULL UNIQUE,
  nombre      VARCHAR(100) NOT NULL,
  id_jefatura INT          NULL,
  CONSTRAINT chk_nombre_area CHECK (nombre REGEXP '^[A-Za-záéíóúÁÉÍÓÚñÑ ]+$')
);

CREATE TABLE IF NOT EXISTS concurso (
  id_concurso  INT AUTO_INCREMENT PRIMARY KEY,
  codigo       VARCHAR(50)  NOT NULL UNIQUE,
  nombre       VARCHAR(150) NOT NULL,
  fecha_inicio DATE         NOT NULL,
  fecha_fin    DATE         NOT NULL,
  estado       ENUM('Vigente','Vencido') NOT NULL DEFAULT 'Vigente',
  CONSTRAINT chk_fechas_concurso CHECK (fecha_fin >= fecha_inicio)
);

CREATE TABLE IF NOT EXISTS oferente (
  id_oferente         INT AUTO_INCREMENT PRIMARY KEY,
  identificacion      VARCHAR(20)  NOT NULL UNIQUE,
  tipo_identificacion ENUM('Cédula de identidad','DIMEX','Pasaporte') NOT NULL,
  nombre_completo     VARCHAR(150) NOT NULL,
  fecha_nacimiento    DATE         NOT NULL
);

CREATE TABLE IF NOT EXISTS oferente_correo (
  id_correo   INT AUTO_INCREMENT PRIMARY KEY,
  id_oferente INT          NOT NULL,
  correo      VARCHAR(150) NOT NULL,
  CONSTRAINT fk_correo_oferente FOREIGN KEY (id_oferente) REFERENCES oferente(id_oferente)
);

CREATE TABLE IF NOT EXISTS oferente_telefono (
  id_telefono INT AUTO_INCREMENT PRIMARY KEY,
  id_oferente INT         NOT NULL,
  telefono    VARCHAR(20) NOT NULL,
  CONSTRAINT fk_telefono_oferente FOREIGN KEY (id_oferente) REFERENCES oferente(id_oferente)
);

CREATE TABLE IF NOT EXISTS oferente_concurso (
  id_oferente INT NOT NULL,
  id_concurso INT NOT NULL,
  PRIMARY KEY (id_oferente, id_concurso),
  CONSTRAINT fk_ofcon_oferente FOREIGN KEY (id_oferente) REFERENCES oferente(id_oferente),
  CONSTRAINT fk_ofcon_concurso FOREIGN KEY (id_concurso) REFERENCES concurso(id_concurso)
);

CREATE TABLE IF NOT EXISTS preparacion_academica (
  id             INT AUTO_INCREMENT PRIMARY KEY,
  oferente_id    INT NOT NULL,
  institucion_educativa_id INT NOT NULL,
  titulo         VARCHAR(100) NOT NULL,
  fecha_inicio   DATE         NOT NULL,
  fecha_fin      DATE         NOT NULL,
  fecha_creacion DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
  CONSTRAINT chk_titulo_prep     CHECK (titulo REGEXP '^[A-Za-záéíóúÁÉÍÓÚñÑ ]+$'),
  CONSTRAINT chk_fechas_prep     CHECK (fecha_fin >= fecha_inicio),
  CONSTRAINT fk_prep_oferente    FOREIGN KEY (oferente_id)    REFERENCES oferente(id_oferente),
  CONSTRAINT fk_prep_institucion FOREIGN KEY (institucion_educativa_id) REFERENCES institucion_educativa(id_institucion)
);

CREATE TABLE IF NOT EXISTS experiencia_laboral (
  id                 INT AUTO_INCREMENT PRIMARY KEY,
  oferente_id        INT          NOT NULL,
  nombre_empresa     VARCHAR(100) NOT NULL,
  puesto_desempenado VARCHAR(100) NOT NULL,
  fecha_inicio       DATE         NOT NULL,
  fecha_fin          DATE         NOT NULL,
  fecha_creacion     DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
  CONSTRAINT chk_fechas_exp  CHECK (fecha_fin >= fecha_inicio),
  CONSTRAINT fk_exp_oferente FOREIGN KEY (oferente_id) REFERENCES oferente(id_oferente)
);

CREATE TABLE IF NOT EXISTS empleado (
  id_empleado     INT AUTO_INCREMENT PRIMARY KEY,
  numero_empleado VARCHAR(20) NOT NULL UNIQUE,
  id_oferente     INT         NOT NULL UNIQUE,
  id_puesto       INT         NOT NULL,
  id_area         INT         NULL,
  CONSTRAINT fk_empleado_oferente FOREIGN KEY (id_oferente) REFERENCES oferente(id_oferente),
  CONSTRAINT fk_empleado_puesto   FOREIGN KEY (id_puesto)   REFERENCES puesto(id_puesto),
  CONSTRAINT fk_empleado_area     FOREIGN KEY (id_area)     REFERENCES area(id_area)
);

ALTER TABLE area
  ADD CONSTRAINT fk_area_jefatura FOREIGN KEY (id_jefatura) REFERENCES empleado(id_empleado);

CREATE TABLE IF NOT EXISTS accion_personal (
  id_accion    INT AUTO_INCREMENT PRIMARY KEY,
  codigo       VARCHAR(20)  NOT NULL UNIQUE,
  fecha_accion DATE         NOT NULL,
  descripcion  TEXT         NOT NULL,
  id_empleado  INT          NOT NULL,
  id_jefatura  INT          NOT NULL,
  CONSTRAINT fk_accion_empleado FOREIGN KEY (id_empleado) REFERENCES empleado(id_empleado),
  CONSTRAINT fk_accion_jefatura FOREIGN KEY (id_jefatura) REFERENCES empleado(id_empleado)
);

CREATE TABLE IF NOT EXISTS entrevista (
  id_entrevista    INT AUTO_INCREMENT PRIMARY KEY,
  id_oferente      INT  NOT NULL,
  id_empleado      INT  NOT NULL,
  fecha_entrevista DATE NOT NULL,
  estado           ENUM('Pendiente','Realizada') NOT NULL DEFAULT 'Pendiente',
  CONSTRAINT fk_entrevista_oferente FOREIGN KEY (id_oferente) REFERENCES oferente(id_oferente),
  CONSTRAINT fk_entrevista_empleado FOREIGN KEY (id_empleado) REFERENCES empleado(id_empleado)
);

INSERT INTO rol (nombre_rol) VALUES
  ('Administrador'),
  ('Usuario');

INSERT INTO usuario (nombre_usuario, nombre_completo, correo, contrasena, estado, intentos_fallidos) VALUES
  ('admin',       'Administrador del Sistema', 'admin@serviciosmedicos.com',              'tttVWE641sUyFdJQ.d5kj5U6RNQ5G.opJ7+dGxvEMW7p/iGVFUYg==', 'Activo', 0),
  ('reclutador1', 'Maria Rodriguez',           'maria.rodriguez@serviciosmedicos.com',    '0WPIOeih6FMBWWGQ.TL7YUWd+tBMemKb2c2g=.2RIoLnfS35OJf/L0KDs7vA==', 'Activo', 0);

INSERT INTO usuario_rol (id_usuario, id_rol) VALUES
  (1, 1),
  (2, 2);

INSERT INTO parametro (codigo, valor) VALUES
  ('INTENTOS_LOGIN_MAX', '3'),
  ('SESION_TIMEOUT_MIN', '5');

INSERT INTO pantalla (nombre_pantalla) VALUES
  ('Compania'),
  ('Parametros'),
  ('Roles'),
  ('Pantallas'),
  ('Requisitos de Puesto'),
  ('Instituciones'),
  ('Cargar Ubicaciones'),
  ('Bitacora'),
  ('Oferentes'),
  ('Concursos'),
  ('Areas'),
  ('Contratar Empleado'),
  ('Usuarios');

INSERT INTO rol_pantalla (id_rol, id_pantalla)
SELECT 1, id_pantalla FROM pantalla;

-- ====================================================
-- Vistas de compatibilidad para el proyecto C# (Dapper)
-- ====================================================
CREATE OR REPLACE VIEW puestos AS 
SELECT 
    id_puesto AS id, 
    codigo, 
    nombre, 
    salario, 
    id_puesto_jefatura AS jefatura_puesto_id 
FROM puesto;

CREATE OR REPLACE VIEW instituciones_educativas AS
SELECT 
    id_institucion AS id, 
    nombre 
FROM institucion_educativa;

CREATE OR REPLACE VIEW oferentes AS
SELECT 
    id_oferente AS id,
    nombre_completo AS nombre,
    identificacion,
    tipo_identificacion,
    fecha_nacimiento,
    '' AS correos,
    '' AS telefonos
FROM oferente;

CREATE OR REPLACE VIEW empleados AS
SELECT 
    e.id_empleado AS id,
    o.nombre_completo AS nombre,
    e.id_puesto AS puesto_id
FROM empleado e
JOIN oferente o ON e.id_oferente = o.id_oferente;

CREATE OR REPLACE VIEW entrevistas AS
SELECT 
    id_entrevista AS id,
    id_oferente AS oferente_id,
    id_empleado AS empleado_entrevistador_id,
    fecha_entrevista,
    estado,
    CURRENT_TIMESTAMP AS fecha_creacion
FROM entrevista;

CREATE OR REPLACE VIEW acciones_personal AS
SELECT 
    id_accion AS id,
    codigo,
    fecha_accion AS fecha,
    descripcion,
    id_empleado AS empleado_id,
    id_jefatura AS jefatura_aprueba_id,
    CURRENT_TIMESTAMP AS fecha_creacion
FROM accion_personal;

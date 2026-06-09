USE adminpersonal;

-- Limpiar tablas en orden inverso de claves foráneas si es necesario
SET FOREIGN_KEY_CHECKS = 0;
TRUNCATE TABLE accion_personal;
TRUNCATE TABLE entrevista;
TRUNCATE TABLE area;
TRUNCATE TABLE empleado;
TRUNCATE TABLE experiencia_laboral;
TRUNCATE TABLE preparacion_academica;
TRUNCATE TABLE oferente_concurso;
TRUNCATE TABLE oferente_telefono;
TRUNCATE TABLE oferente_correo;
TRUNCATE TABLE oferente;
TRUNCATE TABLE concurso;
TRUNCATE TABLE requisito_puesto;
TRUNCATE TABLE puesto;
TRUNCATE TABLE institucion_educativa;
TRUNCATE TABLE distrito;
TRUNCATE TABLE canton;
TRUNCATE TABLE provincia;
TRUNCATE TABLE compania;
SET FOREIGN_KEY_CHECKS = 1;

-- 1. Provincias
INSERT INTO provincia (id_provincia, nombre) VALUES
(1, 'San José'),
(2, 'Alajuela'),
(3, 'Cartago'),
(4, 'Heredia'),
(5, 'Guanacaste'),
(6, 'Puntarenas'),
(7, 'Limón');

-- 2. Cantones
INSERT INTO canton (id_canton, nombre, id_provincia) VALUES
(1, 'San José', 1),
(2, 'Escazú', 1),
(3, 'Alajuela', 2),
(4, 'San Ramón', 2),
(5, 'Cartago', 3),
(6, 'Heredia', 4),
(7, 'Liberia', 5);

-- 3. Distritos
INSERT INTO distrito (id_distrito, nombre, id_canton) VALUES
(1, 'El Carmen', 1),
(2, 'Merced', 1),
(3, 'Escazú Centro', 2),
(4, 'Alajuela Centro', 3),
(5, 'San Ramón Centro', 4),
(6, 'Cartago Centro', 5),
(7, 'Heredia Centro', 6);

-- 4. Instituciones Educativas
INSERT INTO institucion_educativa (id_institucion, codigo, nombre) VALUES
(1, 'IE-001', 'Universidad de Costa Rica'),
(2, 'IE-002', 'Instituto Tecnológico de Costa Rica'),
(3, 'IE-003', 'Universidad Nacional'),
(4, 'IE-004', 'Universidad Técnica Nacional');

-- 5. Puestos (Con jerarquía)
INSERT INTO puesto (id_puesto, codigo, nombre, salario, id_puesto_jefatura) VALUES
(1, 'P-001', 'Gerente General', 5000000.00, NULL),
(2, 'P-002', 'Director de TI', 3500000.00, 1),
(3, 'P-003', 'Director de Recursos Humanos', 3000000.00, 1),
(4, 'P-004', 'Desarrollador Senior .NET', 2200000.00, 2),
(5, 'P-005', 'Analista de QA', 1500000.00, 2),
(6, 'P-006', 'Generalista de Recursos Humanos', 1200000.00, 3);

-- 6. Requisitos de Puesto
INSERT INTO requisito_puesto (id_requisito, nombre_requisito, id_puesto) VALUES
(1, 'Bachillerato Universitario en Computación o afín', 4),
(2, 'Mínimo 5 años de experiencia en desarrollo .NET y C#', 4),
(3, 'Conocimiento avanzado en MySQL / SQL Server', 4),
(4, 'Bachillerato Universitario en Ingeniería de Software o afín', 5),
(5, 'Experiencia mínima de 2 años en pruebas de software (QA)', 5),
(6, 'Licenciatura en Recursos Humanos o Administración', 3);

-- 7. Concursos
INSERT INTO concurso (id_concurso, codigo, nombre, fecha_inicio, fecha_fin, estado) VALUES
(1, 'CON-2026-001', 'Plaza Desarrollador Senior .NET', '2026-06-01', '2026-06-30', 'Vigente'),
(2, 'CON-2026-002', 'Plaza Analista de QA', '2026-06-05', '2026-06-25', 'Vigente'),
(3, 'CON-2026-003', 'Plaza Temporal Generalista RH', '2026-05-01', '2026-05-31', 'Vencido');

-- 8. Oferentes
INSERT INTO oferente (id_oferente, identificacion, tipo_identificacion, nombre_completo, fecha_nacimiento) VALUES
(1, '111111111', 'Cédula de identidad', 'Juan Pérez Gómez', '1990-05-15'),
(2, '222222222', 'Cédula de identidad', 'María López Rodríguez', '1993-08-22'),
(3, '333333333', 'DIMEX', 'John Smith Watson', '1988-12-05'),
(4, '444444444', 'Pasaporte', 'Ana Silva Pereira', '1995-03-10');

-- 9. Oferente Correo
INSERT INTO oferente_correo (id_correo, id_oferente, correo) VALUES
(1, 1, 'juan.perez@email.com'),
(2, 2, 'maria.lopez@email.com'),
(3, 3, 'john.smith@email.com'),
(4, 4, 'ana.silva@email.com');

-- 10. Oferente Teléfono
INSERT INTO oferente_telefono (id_telefono, id_oferente, telefono) VALUES
(1, 1, '8888-8888'),
(2, 2, '7777-7777'),
(3, 3, '6666-6666'),
(4, 4, '5555-5555');

-- 11. Oferente Concurso (Postulaciones)
INSERT INTO oferente_concurso (id_oferente, id_concurso) VALUES
(1, 1), -- Juan Pérez en Concurso .NET
(3, 1), -- John Smith en Concurso .NET
(2, 2), -- María López en Concurso QA
(4, 2); -- Ana Silva en Concurso QA

-- 12. Preparación Académica (Usando la tabla real del script final)
INSERT INTO preparacion_academica (id, oferente_id, institucion_educativa_id, titulo, fecha_inicio, fecha_fin) VALUES
(1, 1, 1, 'Bachillerato en Ingeniería de Computadores', '2010-03-01', '2014-12-15'),
(2, 2, 2, 'Bachillerato en Ingeniería en Computación', '2012-03-01', '2016-11-30'),
(3, 3, 3, 'Bachillerato en Ciencias de la Computación', '2008-01-15', '2012-06-20');

-- 13. Experiencia Laboral (Usando la tabla real del script final)
INSERT INTO experiencia_laboral (id, oferente_id, nombre_empresa, puesto_desempenado, fecha_inicio, fecha_fin) VALUES
(1, 1, 'Tech Solutions', 'Desarrollador Junior', '2015-01-15', '2018-03-30'),
(2, 1, 'Global Code', 'Desarrollador Mid', '2018-04-01', '2022-12-31'),
(3, 2, 'QA Experts', 'Analista de QA Junior', '2017-02-01', '2021-05-30');

-- 14. Empleados (Para jefaturas y personal activo)
-- Primero creamos los empleados asociados a oferentes para poder asignar jefaturas
INSERT INTO empleado (id_empleado, numero_empleado, id_oferente, id_puesto, id_area) VALUES
(1, 'EMP-001', 1, 1, NULL), -- Juan Pérez como Gerente General (id_puesto = 1)
(2, 'EMP-002', 2, 3, NULL); -- María López como Director de Recursos Humanos (id_puesto = 3)

-- 15. Áreas (Con sus jefaturas asignadas de la tabla empleado)
INSERT INTO area (id_area, codigo, nombre, id_jefatura) VALUES
(1, 'A-TI', 'Tecnología de Información', 1),
(2, 'A-RH', 'Recursos Humanos', 2);

-- Actualizar los empleados para asociarlos a sus áreas ahora que las áreas existen
UPDATE empleado SET id_area = 1 WHERE id_empleado = 1;
UPDATE empleado SET id_area = 2 WHERE id_empleado = 2;

-- 16. Entrevistas
INSERT INTO entrevista (id_entrevista, id_oferente, id_empleado, fecha_entrevista, estado) VALUES
(1, 3, 1, '2026-06-12', 'Pendiente'), -- Juan Pérez entrevista a John Smith
(2, 4, 2, '2026-06-10', 'Pendiente'); -- María López entrevista a Ana Silva

-- 17. Acciones de Personal
INSERT INTO accion_personal (id_accion, codigo, fecha_accion, descripcion, id_empleado, id_jefatura) VALUES
(1, 'ACC-001', '2026-06-08', 'Contratación inicial del Gerente General', 1, 1),
(2, 'ACC-002', '2026-06-08', 'Nombramiento de Directora de Recursos Humanos', 2, 1);

-- 18. Compañía de Prueba
INSERT INTO compania (id_compania, codigo, nombre) VALUES
(1, 'COMP-01', 'Servicios Médicos S.A.');

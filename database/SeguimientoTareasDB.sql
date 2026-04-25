CREATE DATABASE SeguimientoTareasDB;
GO
USE SeguimientoTareasDB;
GO

CREATE TABLE Estados (
    EstadoID INT PRIMARY KEY IDENTITY(1,1),
    NombreEstado NVARCHAR(50) NOT NULL
);

CREATE TABLE Roles (
    RolID INT PRIMARY KEY IDENTITY(1,1),
    NombreRol NVARCHAR(50) NOT NULL
);

-- 3. TABLA DE USUARIOS
CREATE TABLE Usuarios (
    UsuarioID INT PRIMARY KEY IDENTITY(1,1),
    Nombre NVARCHAR(100) NOT NULL,
    Correo NVARCHAR(100) UNIQUE NOT NULL,
    Password NVARCHAR(100) NOT NULL,
    RolID INT FOREIGN KEY REFERENCES Roles(RolID)
);

CREATE TABLE Proyectos (
    ProyectoID INT PRIMARY KEY IDENTITY(1,1),
    NombreProyecto NVARCHAR(100) NOT NULL,
    Descripcion NVARCHAR(MAX),
    FechaCreacion DATETIME DEFAULT GETDATE()
);


CREATE TABLE Proyecto_Integrantes (
    ProyectoID INT FOREIGN KEY REFERENCES Proyectos(ProyectoID),
    UsuarioID INT FOREIGN KEY REFERENCES Usuarios(UsuarioID),
    FechaUnion DATETIME DEFAULT GETDATE(),
    PRIMARY KEY (ProyectoID, UsuarioID)
);


CREATE TABLE Tareas (
    TareaID INT PRIMARY KEY IDENTITY(1,1),
    Titulo NVARCHAR(100) NOT NULL,
    Descripcion NVARCHAR(MAX),
    Prioridad NVARCHAR(20), -- Baja, Media, Alta
    FechaVencimiento DATE,
    EstadoID INT FOREIGN KEY REFERENCES Estados(EstadoID),
    ProyectoID INT FOREIGN KEY REFERENCES Proyectos(ProyectoID),
    CreadoPorID INT FOREIGN KEY REFERENCES Usuarios(UsuarioID),
    AsignadoAID INT FOREIGN KEY REFERENCES Usuarios(UsuarioID)
);


INSERT INTO Roles (NombreRol) VALUES ('Administrador'), ('Desarrollador');
INSERT INTO Estados (NombreEstado) VALUES ('Pendiente'), ('En Proceso'), ('Finalizado'), ('Bloqueado');

INSERT INTO Usuarios (Nombre, Correo, Password, RolID) VALUES 
('Juan Perez', 'juan.admin@correo.com', 'admin123', 1),
('Henry', 'henry.dev@correo.com', 'henry123', 2),
('Alexander', 'alex.dev@correo.com', 'alex123', 2);

INSERT INTO Proyectos (NombreProyecto, Descripcion) VALUES 
('Venta de Carros', 'Plataforma para inventario y venta de veh�culos usados.'),
('Ecommerce Zapatos', 'Tienda en l�nea con carrito de compras y pagos.'),
('Landing Page Universidad', 'P�gina informativa para captaci�n de nuevos estudiantes.');

INSERT INTO Proyecto_Integrantes (ProyectoID, UsuarioID) VALUES 
(1, 2), (2, 3), (3, 2), (3, 3);

INSERT INTO Tareas (Titulo, Descripcion, Prioridad, EstadoID, ProyectoID, CreadoPorID, AsignadoAID) VALUES 
('Dise�o de BD', 'Crear tablas de marcas y modelos', 'Alta', 3, 1, 1, 2),
('Login de usuario', 'Implementar autenticaci�n ADO.NET', 'Alta', 2, 1, 1, 2),
('Filtro de b�squeda', 'Buscar por a�o y precio', 'Media', 1, 1, 1, 2),
('Maquetaci�n Home', 'Dise�o responsivo con Bootstrap', 'Media', 3, 1, 1, 2),
('Subida de im�genes', 'M�dulo para fotos de carros', 'Alta', 1, 1, 1, 2),
('Reporte de ventas', 'Exportar a PDF', 'Baja', 1, 1, 1, 2),
('Validaci�n de formularios', 'Usar RequiredFieldValidator', 'Media', 1, 1, 1, 2);

INSERT INTO Tareas (Titulo, Descripcion, Prioridad, EstadoID, ProyectoID, CreadoPorID, AsignadoAID) VALUES 
('Pasarela de pagos', 'Integraci�n con PayPal', 'Alta', 4, 2, 1, 3),
('Carrito de compras', 'L�gica de sesi�n para productos', 'Alta', 2, 2, 1, 3),
('Cat�logo de productos', 'Mostrar lista desde SQL', 'Media', 3, 2, 1, 3),
('Detalle de producto', 'Vista individual con stock', 'Baja', 1, 2, 1, 3),
('Configurar SSL', 'Seguridad del sitio', 'Alta', 1, 2, 1, 3),
('Correos de confirmaci�n', 'Env�o autom�tico tras compra', 'Media', 1, 2, 1, 3),
('Panel de Admin', 'CRUD de productos', 'Alta', 2, 2, 1, 3);

INSERT INTO Tareas (Titulo, Descripcion, Prioridad, EstadoID, ProyectoID, CreadoPorID, AsignadoAID) VALUES 
('Formulario de contacto', 'Captar datos de prospectos', 'Alta', 2, 3, 1, 2),
('SEO inicial', 'Configurar meta tags', 'Media', 1, 3, 1, 2),
('Mapa de ubicaci�n', 'Integrar Google Maps', 'Baja', 3, 3, 1, 2),
('Secci�n de Carreras', 'Lista desplegable de oferta', 'Media', 1, 3, 1, 2),
('Optimizaci�n de carga', 'Reducir peso de im�genes', 'Alta', 2, 3, 1, 3),
('Bot�n de WhatsApp', 'Chat directo con soporte', 'Baja', 3, 3, 1, 3),
('Footer informativo', 'Direcci�n y redes sociales', 'Baja', 3, 3, 1, 3);


CREATE PROCEDURE sp_ListarTareasPorUsuario
    @UsuarioID INT
AS
BEGIN
    SELECT 
        T.TareaID, 
        T.Titulo, 
        T.Descripcion, 
        E.NombreEstado, 
        P.NombreProyecto, 
        T.Prioridad,
        T.FechaVencimiento
    FROM Tareas T
    INNER JOIN Estados E ON T.EstadoID = E.EstadoID
    INNER JOIN Proyectos P ON T.ProyectoID = P.ProyectoID
    WHERE T.AsignadoAID = @UsuarioID
    ORDER BY T.FechaVencimiento ASC;
END
GO

-- un usuario selecciona en que proyecto desea trabajar
CREATE PROCEDURE sp_ListarProyectosPorUsuario
    @UsuarioID INT
AS
BEGIN
    SELECT P.ProyectoID, P.NombreProyecto
    FROM Proyectos P
    INNER JOIN Proyecto_Integrantes PI ON P.ProyectoID = PI.ProyectoID
    WHERE PI.UsuarioID = @UsuarioID;
END
GO

-- creacion de una tarea 
CREATE PROCEDURE sp_InsertarTarea
    @Titulo NVARCHAR(100),
    @Descripcion NVARCHAR(MAX),
    @Prioridad NVARCHAR(20),
    @FechaVencimiento DATE,
    @EstadoID INT,
    @ProyectoID INT,
    @CreadoPorID INT,
    @AsignadoAID INT
AS
BEGIN
    INSERT INTO Tareas (Titulo, Descripcion, Prioridad, FechaVencimiento, EstadoID, ProyectoID, CreadoPorID, AsignadoAID)
    VALUES (@Titulo, @Descripcion, @Prioridad, @FechaVencimiento, @EstadoID, @ProyectoID, @CreadoPorID, @AsignadoAID);
END
GO

-- agregar nuevo usuario 
CREATE PROCEDURE sp_InsertarUsuario
    @Nombre NVARCHAR(100),
    @Correo NVARCHAR(100),
    @Password NVARCHAR(100),
    @RolID INT
AS
BEGIN
    INSERT INTO Usuarios (Nombre, Correo, Password, RolID)
    VALUES (@Nombre, @Correo, @Password, @RolID);
END
GO

--hacer modificacion de estados de las tareas
CREATE PROCEDURE sp_ActualizarEstadoTarea
    @TareaID INT,
    @NuevoEstadoID INT
AS
BEGIN
    UPDATE Tareas 
    SET EstadoID = @NuevoEstadoID
    WHERE TareaID = @TareaID;
END
GO

--validamos a un usuario 
CREATE PROCEDURE sp_ValidarLogin
    @Correo NVARCHAR(100),
    @Password NVARCHAR(100)
AS
BEGIN
    SELECT 
        U.UsuarioID, 
        U.Nombre, 
        U.Correo, 
        R.NombreRol
    FROM Usuarios U
    INNER JOIN Roles R ON U.RolID = R.RolID
    WHERE U.Correo = @Correo AND U.Password = @Password;
END
GO


--testeando usuarios
EXEC sp_ValidarLogin @Correo = 'henry.dev@correo.com', @Password = 'henry123';
EXEC sp_ValidarLogin @Correo = 'juan.admin@correo.com', @Password = 'admsin123'; --este falla contra mala
EXEC sp_ListarTareasPorUsuario @UsuarioID = 2;
SELECT * FROM Usuarios


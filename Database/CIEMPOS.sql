/*=========================================================
    CIEMPOS
    Script de Creación de la Base de Datos
=========================================================*/
CREATE DATABASE CIEMPOS;
GO

USE CIEMPOS;
GO

/*******************************************************
    TABLA: TB_Persona
*******************************************************/

CREATE TABLE TB_Persona (
    IdPersona INT IDENTITY(1,1) NOT NULL,

    Nombre VARCHAR(100) NOT NULL,
    Apellido VARCHAR(100) NOT NULL,
    Identificacion VARCHAR(20) NOT NULL,
    FechaNacimiento DATE NOT NULL,
    Sexo VARCHAR(50) NOT NULL,
    Email VARCHAR(100) NOT NULL,
    Telefono VARCHAR(20) NOT NULL,
    Direccion VARCHAR(255) NOT NULL,
    ContactoEmergencia VARCHAR(100) NULL,
    TelefonoEmergencia VARCHAR(20) NULL,
    FechaRegistro DATETIME NOT NULL DEFAULT GETDATE(),
    DebeCambiarContrasena BIT NOT NULL DEFAULT 1,
    Estado BIT NOT NULL DEFAULT 1,

    CONSTRAINT PK_TB_Persona
        PRIMARY KEY (IdPersona),

    CONSTRAINT UQ_TB_Persona_Identificacion
        UNIQUE (Identificacion)

);
GO

/*******************************************************
    TABLA: TB_Rol
*******************************************************/

CREATE TABLE TB_Rol (

    IdRol INT IDENTITY(1,1) NOT NULL,

    Nombre VARCHAR(50) NOT NULL,
    Descripcion VARCHAR(255) NULL,
    Estado BIT NOT NULL DEFAULT 1,

    CONSTRAINT PK_TB_Rol
        PRIMARY KEY (IdRol),

    CONSTRAINT UQ_TB_Rol_Nombre
        UNIQUE (Nombre)
);
GO

/*******************************************************
    TABLA: TB_Permiso
*******************************************************/

CREATE TABLE TB_Permiso (

    IdPermiso INT IDENTITY(1,1) NOT NULL,

    Nombre VARCHAR(50) NOT NULL,
    Descripcion VARCHAR(255) NULL,
    Estado BIT NOT NULL DEFAULT 1,

    CONSTRAINT PK_TB_Permiso
        PRIMARY KEY (IdPermiso),

    CONSTRAINT UQ_TB_Permiso_Nombre
        UNIQUE (Nombre)
);
Go

/*******************************************************
    TABLA: TB_RolPermiso
*******************************************************/

CREATE TABLE TB_RolPermiso (

    IdRol INT NOT NULL,
    IdPermiso INT NOT NULL,

    CONSTRAINT PK_TB_RolPermiso
        PRIMARY KEY (IdRol, IdPermiso),

    CONSTRAINT FK_TB_RolPermiso_TB_Rol
        FOREIGN KEY (IdRol)
        REFERENCES TB_Rol (IdRol),

    CONSTRAINT FK_TB_RolPermiso_TB_Permiso
        FOREIGN KEY (IdPermiso)
        REFERENCES TB_Permiso (IdPermiso)
);
Go

/*******************************************************
    TABLA: TB_Usuario
*******************************************************/

CREATE TABLE TB_Usuario (

    IdUsuario INT IDENTITY(1,1) NOT NULL,

    IdPersona INT NOT NULL,
    IdRol INT NOT NULL,
    Contrasena VARCHAR(255) NOT NULL,
    DebeCambiarContrasena BIT NOT NULL DEFAULT 1,
    Estado BIT NOT NULL DEFAULT 1,

    CONSTRAINT PK_TB_Usuario
        PRIMARY KEY (IdUsuario),

    CONSTRAINT FK_TB_Usuario_TB_Persona
        FOREIGN KEY (IdPersona)
        REFERENCES TB_Persona (IdPersona),

    CONSTRAINT FK_TB_Usuario_TB_Rol
        FOREIGN KEY (IdRol)
        REFERENCES TB_Rol (IdRol),

    CONSTRAINT UQ_TB_Usuario_IdPersona
        UNIQUE (IdPersona)
);
GO

/*******************************************************
    TABLA: TB_Paciente
*******************************************************/

CREATE TABLE TB_Paciente (

    IdPaciente INT IDENTITY(1,1) NOT NULL,

    IdPersona INT NOT NULL,
    InformacionClinica VARCHAR(MAX) NULL,
    Estado BIT NOT NULL DEFAULT 1,

    CONSTRAINT PK_TB_Paciente
        PRIMARY KEY (IdPaciente),

    CONSTRAINT FK_TB_Paciente_TB_Persona
        FOREIGN KEY (IdPersona)
        REFERENCES TB_Persona (IdPersona),

    CONSTRAINT UQ_TB_Paciente_IdPersona
        UNIQUE (IdPersona)
);
GO

/*******************************************************
    TABLA: TB_EvaluacionFisica
*******************************************************/

CREATE TABLE TB_EvaluacionFisica (

    IdEvaluacion INT IDENTITY(1,1) NOT NULL,

    IdPaciente INT NOT NULL,
    IdUsuario INT NOT NULL,
    FechaEvaluacion DATETIME NOT NULL DEFAULT GETDATE(),
    Peso DECIMAL(5,2) NOT NULL,
    Estatura DECIMAL(5,2) NOT NULL,
    IMC DECIMAL(5,2) NOT NULL,
    PorcentajeGrasa DECIMAL(5,2) NOT NULL,
    MasaMuscular DECIMAL(5,2) NOT NULL,
    Observaciones VARCHAR(MAX) NULL,

    CONSTRAINT PK_TB_EvaluacionFisica
        PRIMARY KEY (IdEvaluacion),

    CONSTRAINT FK_TB_EvaluacionFisica_TB_Paciente
        FOREIGN KEY (IdPaciente)
        REFERENCES TB_Paciente (IdPaciente),

    CONSTRAINT FK_TB_EvaluacionFisica_TB_Usuario
        FOREIGN KEY (IdUsuario)
        REFERENCES TB_Usuario (IdUsuario)
);
GO

/*******************************************************
    TABLA: TB_Prescripcion
*******************************************************/

CREATE TABLE TB_Prescripcion (

    IdPrescripcion INT IDENTITY(1,1) NOT NULL,

    IdPaciente INT NOT NULL,
    IdUsuario INT NOT NULL,
    FechaPrescripcion DATETIME NOT NULL DEFAULT GETDATE(),
    Cardio VARCHAR(MAX) NULL,
    Fuerza VARCHAR(MAX) NULL,
    Estiramiento VARCHAR(MAX) NULL,
    Observaciones VARCHAR(MAX) NULL,
    Estado BIT NOT NULL DEFAULT 1,

    CONSTRAINT PK_TB_Prescripcion
        PRIMARY KEY (IdPrescripcion),

    CONSTRAINT FK_TB_Prescripcion_TB_Paciente
        FOREIGN KEY (IdPaciente)
        REFERENCES TB_Paciente (IdPaciente),

    CONSTRAINT FK_TB_Prescripcion_TB_Usuario
        FOREIGN KEY (IdUsuario)
        REFERENCES TB_Usuario (IdUsuario)
);
GO

/*******************************************************
    TABLA: TB_Pago
*******************************************************/

CREATE TABLE TB_Pago (

    IdPago INT IDENTITY(1,1) NOT NULL,

    IdPaciente INT NOT NULL,
    IdUsuario INT NOT NULL,
    FechaPago DATETIME NOT NULL DEFAULT GETDATE(),
    Monto DECIMAL(10,2) NOT NULL,
    NumeroAutorizacion VARCHAR(50) NOT NULL,
    Estado BIT NOT NULL DEFAULT 1,

    CONSTRAINT PK_TB_Pago
        PRIMARY KEY (IdPago),

    CONSTRAINT FK_TB_Pago_TB_Paciente
        FOREIGN KEY (IdPaciente)
        REFERENCES TB_Paciente (IdPaciente),

    CONSTRAINT FK_TB_Pago_TB_Usuario
        FOREIGN KEY (IdUsuario)
        REFERENCES TB_Usuario (IdUsuario)
);
GO

/*******************************************************
    TABLA: TB_BitacoraIngreso
*******************************************************/

CREATE TABLE TB_BitacoraIngreso (

    IdBitacora INT IDENTITY(1,1) NOT NULL,

    IdUsuario INT NOT NULL,
    FechaHoraIngreso DATETIME NOT NULL DEFAULT GETDATE(),
    FechaHoraSalida DATETIME NULL,

    CONSTRAINT PK_TB_BitacoraIngreso
        PRIMARY KEY (IdBitacora),

    CONSTRAINT FK_TB_BitacoraIngreso_TB_Usuario
        FOREIGN KEY (IdUsuario)
        REFERENCES TB_Usuario (IdUsuario)
);
GO

/*******************************************************
    TABLA: TB_BitacoraMovimiento
*******************************************************/

CREATE TABLE TB_BitacoraMovimiento (

    IdMovimiento INT IDENTITY(1,1) NOT NULL,

    IdUsuario INT NOT NULL,
    Modulo VARCHAR(100) NOT NULL,
    IdRegistroAfectado INT NOT NULL,
    TipoMovimiento VARCHAR(50) NOT NULL,
    FechaHora DATETIME NOT NULL DEFAULT GETDATE(),

    CONSTRAINT PK_TB_BitacoraMovimiento
        PRIMARY KEY (IdMovimiento),

    CONSTRAINT FK_TB_BitacoraMovimiento_TB_Usuario
        FOREIGN KEY (IdUsuario)
        REFERENCES TB_Usuario (IdUsuario)
);
GO
-- ==============================================================================
-- Script Maestro de Despliegue: 00_master_deploy.sql
-- Proyecto: Agile Gatekeeper
-- Base de Datos: db_agile_gatekeeper
-- Módulo: Seguridad, Roles y Autenticación Inicial (Login Local y Microsoft)
-- Convención: snake_case estricto con prefijos de tipo de dato
-- Regla Obligatoria: Toda tabla incluye los 5 campos de auditoría y estado:
--   [bo_flag_activo], [dt_fecha_registro], [vc_usuario_creacion],
--   [dt_fecha_actualizacion], [vc_usuario_actualizacion]
-- ==============================================================================

USE master;
GO

IF NOT EXISTS (SELECT 1 FROM sys.databases WHERE name = N'db_agile_gatekeeper')
BEGIN
    CREATE DATABASE db_agile_gatekeeper;
    PRINT 'Base de datos db_agile_gatekeeper creada.';
END
GO

USE db_agile_gatekeeper;
GO

SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO

-- 1. Esquema de Seguridad
IF SCHEMA_ID(N'security') IS NULL
    EXEC(N'CREATE SCHEMA security');
GO

-- ------------------------------------------------------------------------------
-- 2. Limpieza de tablas existentes para recreación limpia
-- ------------------------------------------------------------------------------
IF OBJECT_ID(N'security.usuario_rol', N'U') IS NOT NULL
    DROP TABLE security.usuario_rol;
GO

IF OBJECT_ID(N'security.usuario', N'U') IS NOT NULL
    DROP TABLE security.usuario;
GO

IF OBJECT_ID(N'security.rol', N'U') IS NOT NULL
    DROP TABLE security.rol;
GO

-- ------------------------------------------------------------------------------
-- 3. Tabla: security.rol (Roles del Sistema)
-- ------------------------------------------------------------------------------
CREATE TABLE security.rol
(
    in_rol_id INT IDENTITY(1,1) NOT NULL,
    vc_codigo VARCHAR(50) NOT NULL,
    vc_nombre VARCHAR(100) NOT NULL,
    vc_descripcion VARCHAR(255) NULL,
    
    -- 5 Campos obligatorios de auditoría y estado
    bo_flag_activo BIT NOT NULL 
        CONSTRAINT df_rol_estado DEFAULT (1),
    dt_fecha_registro DATETIME2(3) NOT NULL 
        CONSTRAINT df_rol_fecha_registro DEFAULT (SYSUTCDATETIME()),
    vc_usuario_creacion VARCHAR(100) NULL,
    dt_fecha_actualizacion DATETIME2(3) NULL,
    vc_usuario_actualizacion VARCHAR(100) NULL,

    CONSTRAINT pk_rol
        PRIMARY KEY (in_rol_id),

    CONSTRAINT uq_rol_codigo
        UNIQUE (vc_codigo)
);
GO

-- ------------------------------------------------------------------------------
-- 4. Tabla Principal: security.usuario
-- ------------------------------------------------------------------------------
CREATE TABLE security.usuario
(
    in_usuario_id INT IDENTITY(1,1) NOT NULL,
    vc_nombre VARCHAR(100) NOT NULL,
    vc_apellido VARCHAR(100) NOT NULL,
    vc_correo VARCHAR(255) NOT NULL,
    vc_password_hash VARCHAR(255) NULL,
    vc_tenant_id VARCHAR(100) NULL,
    vc_ubicacion VARCHAR(150) NULL,
    vc_avatar_url VARCHAR(500) NULL,
    bo_flag_entra_id BIT NOT NULL 
        CONSTRAINT df_usuario_entra_id DEFAULT (0),
    dt_fecha_ultimo_login DATETIME2(3) NULL,

    -- 5 Campos obligatorios de auditoría y estado
    bo_flag_activo BIT NOT NULL 
        CONSTRAINT df_usuario_estado DEFAULT (1),
    dt_fecha_registro DATETIME2(3) NOT NULL 
        CONSTRAINT df_usuario_fecha_registro DEFAULT (SYSUTCDATETIME()),
    vc_usuario_creacion VARCHAR(100) NULL,
    dt_fecha_actualizacion DATETIME2(3) NULL,
    vc_usuario_actualizacion VARCHAR(100) NULL,

    CONSTRAINT pk_usuario
        PRIMARY KEY (in_usuario_id),

    CONSTRAINT uq_usuario_correo
        UNIQUE (vc_correo)
);
GO

-- ------------------------------------------------------------------------------
-- 5. Tabla Intermedia: security.usuario_rol
-- ------------------------------------------------------------------------------
CREATE TABLE security.usuario_rol
(
    in_usuario_id INT NOT NULL,
    in_rol_id INT NOT NULL,

    -- 5 Campos obligatorios de auditoría y estado
    bo_flag_activo BIT NOT NULL 
        CONSTRAINT df_usuario_rol_estado DEFAULT (1),
    dt_fecha_registro DATETIME2(3) NOT NULL 
        CONSTRAINT df_usuario_rol_fecha_registro DEFAULT (SYSUTCDATETIME()),
    vc_usuario_creacion VARCHAR(100) NULL,
    dt_fecha_actualizacion DATETIME2(3) NULL,
    vc_usuario_actualizacion VARCHAR(100) NULL,

    CONSTRAINT pk_usuario_rol
        PRIMARY KEY (in_usuario_id, in_rol_id),

    CONSTRAINT fk_usuario_rol_usuario
        FOREIGN KEY (in_usuario_id) 
        REFERENCES security.usuario (in_usuario_id)
        ON DELETE CASCADE,

    CONSTRAINT fk_usuario_rol_rol
        FOREIGN KEY (in_rol_id) 
        REFERENCES security.rol (in_rol_id)
        ON DELETE CASCADE
);
GO

-- ------------------------------------------------------------------------------
-- 6. Índices
-- ------------------------------------------------------------------------------
CREATE NONCLUSTERED INDEX ix_usuario_correo 
    ON security.usuario (vc_correo) 
    INCLUDE (bo_flag_activo, vc_nombre, vc_apellido, vc_tenant_id);
GO

CREATE NONCLUSTERED INDEX ix_usuario_tenant 
    ON security.usuario (vc_tenant_id) 
    WHERE vc_tenant_id IS NOT NULL;
GO

CREATE NONCLUSTERED INDEX ix_rol_codigo 
    ON security.rol (vc_codigo);
GO

CREATE NONCLUSTERED INDEX ix_usuario_rol_activo 
    ON security.usuario_rol (in_usuario_id, in_rol_id, bo_flag_activo);
GO

-- ------------------------------------------------------------------------------
-- 7. Stored Procedure: security.sp_login_local
-- ------------------------------------------------------------------------------
CREATE OR ALTER PROCEDURE security.sp_login_local
    @vc_correo VARCHAR(255),
    @vc_password_hash VARCHAR(255)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @in_usuario_id INT;

    SELECT TOP 1
        @in_usuario_id = in_usuario_id
    FROM security.usuario
    WHERE vc_correo = @vc_correo
      AND vc_password_hash = @vc_password_hash
      AND bo_flag_activo = 1;

    IF @in_usuario_id IS NOT NULL
    BEGIN
        UPDATE security.usuario
        SET dt_fecha_ultimo_login = SYSUTCDATETIME()
        WHERE in_usuario_id = @in_usuario_id;

        SELECT
            u.in_usuario_id,
            u.vc_nombre,
            u.vc_apellido,
            u.vc_correo,
            u.vc_tenant_id,
            u.vc_ubicacion,
            u.vc_avatar_url,
            ISNULL(r.vc_codigo, 'admin') AS vc_rol_codigo,
            ISNULL(r.vc_nombre, 'Administrador') AS vc_rol_nombre,
            u.bo_flag_activo,
            u.bo_flag_entra_id,
            u.dt_fecha_registro,
            u.vc_usuario_creacion,
            u.dt_fecha_actualizacion,
            u.vc_usuario_actualizacion,
            u.dt_fecha_ultimo_login
        FROM security.usuario u
        LEFT JOIN security.usuario_rol ur 
            ON u.in_usuario_id = ur.in_usuario_id 
           AND ur.bo_flag_activo = 1
        LEFT JOIN security.rol r 
            ON ur.in_rol_id = r.in_rol_id 
           AND r.bo_flag_activo = 1
        WHERE u.in_usuario_id = @in_usuario_id;
    END
END;
GO

-- ------------------------------------------------------------------------------
-- 8. Stored Procedure: security.sp_validar_usuario_microsoft
-- ------------------------------------------------------------------------------
CREATE OR ALTER PROCEDURE security.sp_validar_usuario_microsoft
    @vc_correo VARCHAR(255),
    @vc_tenant_id VARCHAR(100) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @in_usuario_id INT;

    SELECT TOP 1
        @in_usuario_id = in_usuario_id
    FROM security.usuario
    WHERE vc_correo = @vc_correo
      AND bo_flag_activo = 1
      AND (
          @vc_tenant_id IS NULL 
          OR vc_tenant_id IS NULL 
          OR vc_tenant_id = @vc_tenant_id
      );

    IF @in_usuario_id IS NOT NULL
    BEGIN
        UPDATE security.usuario
        SET dt_fecha_ultimo_login = SYSUTCDATETIME(),
            bo_flag_entra_id = 1,
            vc_tenant_id = COALESCE(vc_tenant_id, @vc_tenant_id),
            dt_fecha_actualizacion = SYSUTCDATETIME(),
            vc_usuario_actualizacion = 'microsoft_auth'
        WHERE in_usuario_id = @in_usuario_id;

        SELECT
            u.in_usuario_id,
            u.vc_nombre,
            u.vc_apellido,
            u.vc_correo,
            u.vc_tenant_id,
            u.vc_ubicacion,
            u.vc_avatar_url,
            ISNULL(r.vc_codigo, 'admin') AS vc_rol_codigo,
            ISNULL(r.vc_nombre, 'Administrador') AS vc_rol_nombre,
            u.bo_flag_activo,
            u.bo_flag_entra_id,
            u.dt_fecha_registro,
            u.vc_usuario_creacion,
            u.dt_fecha_actualizacion,
            u.vc_usuario_actualizacion,
            u.dt_fecha_ultimo_login
        FROM security.usuario u
        LEFT JOIN security.usuario_rol ur 
            ON u.in_usuario_id = ur.in_usuario_id 
           AND ur.bo_flag_activo = 1
        LEFT JOIN security.rol r 
            ON ur.in_rol_id = r.in_rol_id 
           AND r.bo_flag_activo = 1
        WHERE u.in_usuario_id = @in_usuario_id;
    END
END;
GO

-- ------------------------------------------------------------------------------
-- 9. Stored Procedure: security.sp_obtener_usuario_por_correo
-- ------------------------------------------------------------------------------
CREATE OR ALTER PROCEDURE security.sp_obtener_usuario_por_correo
    @vc_correo VARCHAR(255)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT TOP 1
        u.in_usuario_id,
        u.vc_nombre,
        u.vc_apellido,
        u.vc_correo,
        u.vc_tenant_id,
        u.vc_ubicacion,
        u.vc_avatar_url,
        ISNULL(r.vc_codigo, 'admin') AS vc_rol_codigo,
        ISNULL(r.vc_nombre, 'Administrador') AS vc_rol_nombre,
        u.bo_flag_activo,
        u.bo_flag_entra_id,
        u.dt_fecha_registro,
        u.vc_usuario_creacion,
        u.dt_fecha_actualizacion,
        u.vc_usuario_actualizacion,
        u.dt_fecha_ultimo_login
    FROM security.usuario u
    LEFT JOIN security.usuario_rol ur 
        ON u.in_usuario_id = ur.in_usuario_id 
       AND ur.bo_flag_activo = 1
    LEFT JOIN security.rol r 
        ON ur.in_rol_id = r.in_rol_id 
       AND r.bo_flag_activo = 1
    WHERE u.vc_correo = @vc_correo;
END;
GO

-- ------------------------------------------------------------------------------
-- 10. Datos Iniciales: Roles y Usuario de Prueba admin@agile.com
-- ------------------------------------------------------------------------------
IF NOT EXISTS (SELECT 1 FROM security.rol WHERE vc_codigo = 'admin')
BEGIN
    INSERT INTO security.rol (vc_codigo, vc_nombre, vc_descripcion, bo_flag_activo, vc_usuario_creacion)
    VALUES ('admin', 'Administrador', 'Administrador General con acceso a todos los módulos y configuraciones.', 1, 'sistema');
END
GO

IF NOT EXISTS (SELECT 1 FROM security.rol WHERE vc_codigo = 'usuario')
BEGIN
    INSERT INTO security.rol (vc_codigo, vc_nombre, vc_descripcion, bo_flag_activo, vc_usuario_creacion)
    VALUES ('usuario', 'Usuario', 'Usuario estándar del sistema.', 1, 'sistema');
END
GO

IF NOT EXISTS (SELECT 1 FROM security.usuario WHERE vc_correo = 'admin@agile.com')
BEGIN
    INSERT INTO security.usuario
    (
        vc_nombre,
        vc_apellido,
        vc_correo,
        vc_password_hash,
        vc_tenant_id,
        vc_ubicacion,
        vc_avatar_url,
        bo_flag_entra_id,
        bo_flag_activo,
        vc_usuario_creacion
    )
    VALUES
    (
        'Administrador',
        'Agile',
        'admin@agile.com',
        'password123',
        'common',
        'Sede Central',
        '/images/avatar.jpg',
        1,
        1,
        'sistema'
    );
END
GO

DECLARE @in_user_id INT = (SELECT in_usuario_id FROM security.usuario WHERE vc_correo = 'admin@agile.com');
DECLARE @in_role_id INT = (SELECT in_rol_id FROM security.rol WHERE vc_codigo = 'admin');

IF @in_user_id IS NOT NULL AND @in_role_id IS NOT NULL
BEGIN
    IF NOT EXISTS (SELECT 1 FROM security.usuario_rol WHERE in_usuario_id = @in_user_id AND in_rol_id = @in_role_id)
    BEGIN
        INSERT INTO security.usuario_rol 
        (
            in_usuario_id, 
            in_rol_id, 
            bo_flag_activo, 
            vc_usuario_creacion
        )
        VALUES 
        (
            @in_user_id, 
            @in_role_id, 
            1, 
            'sistema'
        );
    END
END
GO

PRINT '=======================================================';
PRINT 'Despliegue inicial de seguridad completado con éxito.';
PRINT '=======================================================';
GO
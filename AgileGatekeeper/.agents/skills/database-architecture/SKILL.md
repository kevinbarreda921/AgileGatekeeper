---
name: database-architecture
description: >-
  Enforces SQL Server database architecture and Dapper-only data access rules for Agile Gatekeeper.
  Mandates strict lowercase snake_case naming for all database objects, domain-based schemas,
  mandatory column data type prefixes (vc_, in_, dt_, bo_, etc.), sp_ stored procedures,
  vw_ views, fn_ functions, and idiomatic C# .NET mapping without Entity Framework.
triggers:
  - "database"
  - "base de datos"
  - "sql server"
  - "tabla"
  - "stored procedure"
  - "dapper"
  - "script sql"
  - "migracion sql"
  - "snake_case"
---

# Database Architecture Skill — Mandatory snake_case & Dapper-Only Convention

This skill governs all SQL Server database design, script authoring, and data access implementation in the **Agile Gatekeeper** project.

---

## 1. Mandatory Naming Convention Change (Strict snake_case)

All previous PascalCase naming requirements for SQL Server database objects are strictly removed.

Every SQL Server database object MUST use lowercase `snake_case`:
- Database names (`db_agile_gatekeeper`)
- Schema names (`security`, `project_management`, etc.)
- Table names (singular, e.g., `security.usuario`)
- Column names (with mandatory data type prefixes)
- Stored procedures (`sp_` + lowercase `snake_case`)
- Views (`vw_` + lowercase `snake_case`)
- Functions (`fn_` + lowercase `snake_case`)
- Indexes (`ix_` + lowercase `snake_case`)
- Constraints (`pk_`, `fk_`, `uq_`, `ck_`, `df_`)
- SQL parameter names (`@` + prefix or matching column name)

Never use PascalCase or camelCase for any SQL Server object.

---

## 2. Database Naming

- The target application database name is: `db_agile_gatekeeper`.
- Never use PascalCase (`AgileGatekeeperDB`) or camelCase (`agileGatekeeperDb`).

---

## 3. Domain-Based Schemas (lowercase snake_case)

Organize all application objects into domain-driven SQL Server schemas:

- `security`: Authentication, authorization, users, roles, permissions, Entra ID integration.
- `configuration`: System and workspace settings, feature flags.
- `organization`: Companies, branches, departments, corporate hierarchy.
- `project_management`: Projects, portfolios, roadmaps, project members.
- `product_management`: Products, epics, release tracks.
- `backlog`: User stories, backlog items, acceptance criteria.
- `sprint`: Sprints, sprint tasks, ceremonies, velocity tracking.
- `workflow`: States, transitions, Kanban boards, lifecycle rules.
- `meeting`: Agile meetings, daily standups, sprint reviews, retrospectives.
- `notification`: In-app notifications, alerts, message queues.
- `reporting`: Metrics, analytical snapshots, KPIs.
- `integration`: Webhooks, external sync, third-party connectors.
- `audit`: Change data tracking, security logs, access audit.

### Schema Architecture Rules:
1. Each schema represents a cohesive business domain.
2. There is no arbitrary maximum number of schemas.
3. Never create one schema per table.
4. Never create schemas for individual UI screens.
5. Reuse existing schemas when appropriate.
6. Create new schemas only when justified.
7. Always use schema-qualified object names (`security.usuario`, `sprint.sprint`).
8. Avoid `dbo` for application business objects.
9. Allow cross-schema foreign keys when justified by domain relationships.
10. Avoid unnecessary domain fragmentation.
11. Do not create schemas per customer (use tenant identifier columns or independent customer databases).

---

## 4. Table Naming (Singular lowercase snake_case)

- All table names must be in lowercase `snake_case` and in singular form.
- Examples:
  - `security.usuario`
  - `security.rol`
  - `security.permiso`
  - `security.usuario_rol`
  - `project_management.proyecto`
  - `sprint.sprint`
  - `sprint.sprint_tarea`
- Banned patterns:
  - `Usuario`, `Usuarios`, `SprintTask`, `ProjectManagement`.

---

## 5. Mandatory Column Data Type Prefixes

Every column name in every table must use lowercase `snake_case` and begin with the prefix corresponding to its SQL Server data type:

| SQL Server Type | Prefix | Description / Example |
|---|---|---|
| `VARCHAR` | `vc_` | `vc_nombre`, `vc_correo`, `vc_codigo` |
| `NVARCHAR` | `nv_` | `nv_descripcion`, `nv_mensaje`, `nv_objetivo` |
| `CHAR` | `ch_` | `ch_codigo`, `ch_tipo` |
| `NCHAR` | `nc_` | `nc_codigo` |
| `INT` | `in_` | `in_usuario_id`, `in_rol_id`, `in_proyecto_id` |
| `BIGINT` | `bi_` | `bi_auditoria_id`, `bi_secuencia` |
| `SMALLINT` | `si_` | `si_orden`, `si_estimacion_horas` |
| `TINYINT` | `ti_` | `ti_nivel`, `ti_prioridad` |
| `BIT` | `bo_` | `bo_flag_activo`, `bo_flag_leido`, `bo_flag_estado` |
| `DATE` | `da_` | `da_fecha_inicio`, `da_fecha_fin` |
| `DATETIME` | `dt_` | `dt_fecha_evento` |
| `DATETIME2` | `dt_` | `dt_fecha_registro`, `dt_fecha_actualizacion` |
| `TIME` | `tm_` | `tm_hora_inicio`, `tm_hora_fin` |
| `DECIMAL` | `dc_` | `dc_monto`, `dc_porcentaje` |
| `NUMERIC` | `nu_` | `nu_valor` |
| `FLOAT` | `fl_` | `fl_resultado` |
| `REAL` | `rl_` | `rl_medida` |
| `UNIQUEIDENTIFIER` | `ui_` | `ui_identificador`, `ui_guid` |
| `VARBINARY` | `vb_` | `vb_archivo`, `vb_imagen` |
| `ROWVERSION` | `rv_` | `rv_version` |

### Column Prefix Rules:
- Primary keys must follow the convention (`in_usuario_id`, `bi_auditoria_id`).
- Foreign keys must maintain the prefix matching their data type (`in_organizacion_id`, `in_proyecto_id`).
- The prefix must match the actual SQL Server data type.
- Never mix naming conventions or use unprefixed columns.
- Never change a column data type merely to match a preferred prefix.

---

## 6. Mandatory 5 Standard Audit and Status Columns for EVERY Table

**CRITICAL ARCHITECTURAL MANDATE:**
Every single table created in the system—without exception (including primary entity tables, secondary tables, domain catalogs, and junction/relationship tables such as `security.usuario_rol`)—MUST strictly include the following 5 columns:

| Column Name | SQL Server Type | Nullability & Default | Purpose |
|---|---|---|---|
| `[bo_flag_activo]` | `BIT` | `NOT NULL CONSTRAINT df_<tabla>_estado DEFAULT (1)` | Soft-delete / active status flag |
| `[dt_fecha_registro]` | `DATETIME2(3)` | `NOT NULL CONSTRAINT df_<tabla>_fecha_registro DEFAULT (SYSUTCDATETIME())` | UTC creation timestamp |
| `[vc_usuario_creacion]` | `VARCHAR(100)` | `NULL` | Creator username / actor |
| `[dt_fecha_actualizacion]` | `DATETIME2(3)` | `NULL` | UTC last modification timestamp |
| `[vc_usuario_actualizacion]` | `VARCHAR(100)` | `NULL` | Last updater username / actor |

### Rules for the 5 Mandatory Columns:
1. **Never omit any of the 5 columns** on any table (even many-to-many intermediate/junction tables like `usuario_rol`).
2. Primary keys, foreign keys, and business attributes go first; these 5 audit and status columns are positioned uniformly at the end of the table definition.
3. When performing soft-deletes or deactivating records, update `bo_flag_activo = 0`, `dt_fecha_actualizacion = SYSUTCDATETIME()`, and `vc_usuario_actualizacion = @usuario`.
4. In all `SELECT` queries, stored procedures, and joins, filter active records by default (`WHERE bo_flag_activo = 1` and `JOIN ... ON ... AND <alias>.bo_flag_activo = 1`).

---

## 7. Stored Procedures, Views, Functions & Constraints

### Stored Procedures: `sp_` + lowercase snake_case
- Every user-defined stored procedure must use: `sp_` + lowercase `snake_case`.
- Always include the schema.
- Examples:
  - `security.sp_login`
  - `security.sp_obtener_usuario_por_correo`
  - `security.sp_listar_usuarios`
  - `security.sp_registrar_usuario`
  - `sprint.sp_crear_sprint`
- *Note:* The developer explicitly mandates the `sp_` prefix despite general recommendations, as an architectural standard for this system.

### Views: `vw_` + lowercase snake_case
- Examples: `security.vw_usuarios_activos`, `sprint.vw_resumen_sprints`.

### Functions: `fn_` + lowercase snake_case
- Examples: `sprint.fn_calcular_dias`, `sprint.fn_calcular_avance`.

### Constraints & Indexes:
- Primary Keys: `pk_<tabla>` (e.g., `pk_usuario`, `pk_sprint_tarea`)
- Foreign Keys: `fk_<tabla_origen>_<tabla_destino>` (e.g., `fk_usuario_organizacion`)
- Unique Constraints: `uq_<tabla>_<columna>` (e.g., `uq_usuario_correo`)
- Check Constraints: `ck_<tabla>_<descripcion>` (e.g., `ck_sprint_fechas`)
- Default Constraints: `df_<tabla>_<columna>` (e.g., `df_usuario_estado`)
- Indexes: `ix_<tabla>_<columna>` (e.g., `ix_usuario_correo`)

---

## 8. Mandatory SQL Standard Pattern

```sql
IF SCHEMA_ID(N'security') IS NULL
    EXEC(N'CREATE SCHEMA security');
GO

CREATE TABLE security.usuario
(
    in_usuario_id INT IDENTITY(1,1) NOT NULL,
    vc_nombre VARCHAR(100) NOT NULL,
    vc_apellido VARCHAR(100) NOT NULL,
    vc_correo VARCHAR(255) NOT NULL,
    vc_password_hash VARCHAR(255) NULL,
    vc_tenant_id VARCHAR(100) NULL,
    
    -- Mandatory 5 audit and status columns for EVERY table
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
```

---

## 9. Dapper-Only Architecture (Strict No Entity Framework)

All data access in Agile Gatekeeper must follow these rules:

1. **Strictly Dapper & Microsoft.Data.SqlClient**:
   - Use `Microsoft.Data.SqlClient` and `Dapper` exclusively.
   - **Banned:** Entity Framework, EF Core, `DbContext`, `DbSet`, EF migrations.
2. **Parameterized Queries & Execution**:
   - Always use parameterized queries (`DynamicParameters` or anonymous objects) to prevent SQL Injection.
   - Call stored procedures using `CommandType.StoredProcedure`.
3. **Async & Cancellation Support**:
   - Use asynchronous operations (`QueryAsync`, `ExecuteAsync`, `QueryFirstOrDefaultAsync`).
   - Accept and propagate `CancellationToken`.
4. **C# Naming Autonomy**:
   - C# code MUST continue using standard .NET naming conventions:
     - Classes: `PascalCase` (`UserProfile`, `ProjectDto`)
     - Properties: `PascalCase` (`UserId`, `Email`, `IsActive`)
     - Methods: `PascalCase` (`GetActiveUsersAsync`)
     - Parameters/Variables: `camelCase` (`cancellationToken`, `userId`)
   - SQL snake_case column names must not pollute C# model conventions.
   - Use explicit Dapper mapping (`SELECT vc_nombre AS FirstName, vc_correo AS Email ...`) or Dapper Custom Property Type Maps when mapping to C# models.

---

## 10. Incremental Development & Database Deployment Folder (`Database/`)

The database is built **incrementally** according to project progress. Do not generate unrequested or premature tables.

1. **Current Baseline (Security & Login)**:
   - Starts strictly with the `security` schema and the `security.usuario` table.
   - Includes stored procedures for authentication:
     - `security.sp_login_local` (validates password hash and active flag).
     - `security.sp_validar_usuario_microsoft` (validates that Microsoft email exists in `security.usuario` and is active; otherwise denies access).
     - `security.sp_obtener_usuario_por_correo`.
   - Subsequent domains (`project_management`, `sprint`, `notification`, etc.) will be added incrementally when requested.

2. **Database Deployment Scripts**:
   All database deployment assets must be saved in the project's root `Database/` folder:
   - `01_security_login_init.sql` (Database, schema security, table usuario, login procedures and seed).
   - `00_master_deploy.sql` (Master all-in-one deploy script for server installation).

3. **Authentication & JWT Security Integration**:
   - JWT tokens are issued with user claims (`sub`, `email`, `name`, `role`).
   - Authentication mode is controlled via `Authentication:Provider` in `appsettings.json` (`"Local"`, `"Microsoft"`, `"Both"`).
   - Microsoft login requires explicit existence and active status (`bo_flag_activo = 1`) in `security.usuario`.

# Naming Convention Cheatsheet — SQL Server (Agile Gatekeeper)

Every SQL Server database object must use lowercase `snake_case`.

---

## 1. Column Data Type Prefixes

| SQL Server Data Type | Prefix | Example |
| :--- | :--- | :--- |
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
| `DATETIME` / `DATETIME2` | `dt_` | `dt_fecha_registro`, `dt_fecha_actualizacion` |
| `TIME` | `tm_` | `tm_hora_inicio`, `tm_hora_fin` |
| `DECIMAL` | `dc_` | `dc_monto`, `dc_porcentaje` |
| `NUMERIC` | `nu_` | `nu_valor` |
| `FLOAT` | `fl_` | `fl_resultado` |
| `REAL` | `rl_` | `rl_medida` |
| `UNIQUEIDENTIFIER` | `ui_` | `ui_identificador`, `ui_guid` |
| `VARBINARY` | `vb_` | `vb_archivo`, `vb_imagen` |
| `ROWVERSION` | `rv_` | `rv_version` |

---

## 2. Object and Constraint Prefixes

| Object Type | Syntax Rule | Example |
| :--- | :--- | :--- |
| **Database** | `db_<name>` | `db_agile_gatekeeper` |
| **Schema** | `<domain>` | `security`, `project_management`, `sprint` |
| **Table** | `<schema>.<singular_noun>` | `security.usuario`, `sprint.sprint_tarea` |
| **Stored Procedure** | `<schema>.sp_<verb_action>` | `security.sp_login`, `sprint.sp_crear_sprint` |
| **View** | `<schema>.vw_<description>` | `security.vw_usuarios_activos` |
| **Function** | `<schema>.fn_<description>` | `sprint.fn_calcular_dias` |
| **Primary Key** | `pk_<table_name>` | `pk_usuario`, `pk_sprint_tarea` |
| **Foreign Key** | `fk_<origin_table>_<target_table>` | `fk_usuario_organizacion` |
| **Unique Constraint** | `uq_<table_name>_<column_name>` | `uq_usuario_correo` |
| **Check Constraint** | `ck_<table_name>_<rule>` | `ck_sprint_fechas` |
| **Default Constraint** | `df_<table_name>_<column_name>` | `df_usuario_estado` |
| **Index (Nonclustered)** | `ix_<table_name>_<column_name>` | `ix_usuario_correo` |
| **SQL Parameter** | `@<column_prefix_or_name>` | `@vc_correo`, `@in_usuario_id` |

---

## 3. Mandatory 5 Standard Audit and Status Columns for EVERY Table

Every table (without exception) MUST contain these 5 columns:

```sql
bo_flag_activo           BIT NOT NULL CONSTRAINT df_<tabla>_estado DEFAULT (1),
dt_fecha_registro        DATETIME2(3) NOT NULL CONSTRAINT df_<tabla>_fecha_registro DEFAULT (SYSUTCDATETIME()),
vc_usuario_creacion      VARCHAR(100) NULL,
dt_fecha_actualizacion   DATETIME2(3) NULL,
vc_usuario_actualizacion VARCHAR(100) NULL
```

# Dapper-Only Data Access Implementation Guide

This guide details best practices for implementing data access in **Agile Gatekeeper** using Dapper, Microsoft.Data.SqlClient, and SQL Server.

---

## 1. Connection Management & Factory

Always inject `IDbConnectionFactory`:

```csharp
using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace AgileGatekeeper.Services.Database;

public interface IDbConnectionFactory
{
    IDbConnection CreateConnection();
    Task<IDbConnection> CreateConnectionAsync(CancellationToken cancellationToken = default);
}

public class DbConnectionFactory : IDbConnectionFactory
{
    private readonly string _connectionString;

    public DbConnectionFactory(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
    }

    public IDbConnection CreateConnection() => new SqlConnection(_connectionString);

    public async Task<IDbConnection> CreateConnectionAsync(CancellationToken cancellationToken = default)
    {
        var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync(cancellationToken);
        return connection;
    }
}
```

---

## 2. Executing Stored Procedures with Output Parameters

```csharp
public async Task<int> RegisterUserAsync(CreateUserDto dto, CancellationToken cancellationToken = default)
{
    using var connection = _connectionFactory.CreateConnection();
    var parameters = new DynamicParameters();
    parameters.Add("@vc_nombre", dto.FirstName, DbType.AnsiString);
    parameters.Add("@vc_apellido", dto.LastName, DbType.AnsiString);
    parameters.Add("@vc_correo", dto.Email, DbType.AnsiString);
    parameters.Add("@vc_password_hash", dto.PasswordHash, DbType.AnsiString);
    parameters.Add("@in_usuario_id", dbType: DbType.Int32, direction: ParameterDirection.Output);

    var command = new CommandDefinition(
        commandText: "security.sp_registrar_usuario",
        parameters: parameters,
        commandType: CommandType.StoredProcedure,
        cancellationToken: cancellationToken
    );

    await connection.ExecuteAsync(command);
    return parameters.Get<int>("@in_usuario_id");
}
```

---

## 3. Explicit Column-to-Property Mapping

When reading snake_case SQL columns into standard PascalCase C# DTOs, use explicit SQL aliasing:

```csharp
public async Task<UserProfileDto?> GetUserByEmailAsync(string email, CancellationToken cancellationToken = default)
{
    const string sql = """
        SELECT
            u.in_usuario_id AS UserId,
            u.vc_nombre AS FirstName,
            u.vc_apellido AS LastName,
            u.vc_correo AS Email,
            u.vc_ubicacion AS Location,
            u.vc_avatar_url AS AvatarUrl,
            u.bo_flag_activo AS IsActive,
            u.dt_fecha_registro AS CreatedAt
        FROM security.usuario u
        WHERE u.vc_correo = @Email
          AND u.bo_flag_activo = 1;
        """;

    using var connection = _connectionFactory.CreateConnection();
    var command = new CommandDefinition(sql, new { Email = email }, cancellationToken: cancellationToken);
    return await connection.QueryFirstOrDefaultAsync<UserProfileDto>(command);
}
```

---

## 4. Explicit Transaction Handling

```csharp
public async Task CreateSprintWithTasksAsync(CreateSprintCommand cmd, CancellationToken cancellationToken = default)
{
    using var connection = (SqlConnection)_connectionFactory.CreateConnection();
    await connection.OpenAsync(cancellationToken);
    using var transaction = connection.BeginTransaction();

    try
    {
        // 1. Insert Sprint
        var sprintId = await connection.ExecuteScalarAsync<int>(
            "sprint.sp_crear_sprint",
            new {
                in_proyecto_id = cmd.ProjectId,
                vc_nombre = cmd.SprintName,
                nv_objetivo = cmd.Objective,
                da_fecha_inicio = cmd.StartDate,
                da_fecha_fin = cmd.EndDate
            },
            transaction: transaction,
            commandType: CommandType.StoredProcedure
        );

        // 2. Insert Tasks in batch
        // ...

        await transaction.CommitAsync(cancellationToken);
    }
    catch
    {
        await transaction.RollbackAsync(cancellationToken);
        throw;
    }
}
```

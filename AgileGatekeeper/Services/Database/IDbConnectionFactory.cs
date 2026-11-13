using System.Data;

namespace AgileGatekeeper.Services.Database;

public interface IDbConnectionFactory
{
    IDbConnection CreateConnection();
    Task<IDbConnection> CreateConnectionAsync(CancellationToken cancellationToken = default);
}

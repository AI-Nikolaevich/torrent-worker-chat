using Dapper;
using Npgsql;
using System.Data;

namespace Torrent.Chat.Storage
{
    public abstract class ContextBase(NpgsqlDataSource dataSource)
    {
        private readonly NpgsqlDataSource _dataSource = dataSource ?? throw new ArgumentNullException(nameof(dataSource));

        public async Task<int> ExecuteAsync(
            string sql,
            object? param = null,
            CommandType? commandType = null,
            int? timeout = null,
            CancellationToken ct = default)
        {
            using var connection = await _dataSource.OpenConnectionAsync(ct);

            return await connection.ExecuteAsync(
                new CommandDefinition(
                    sql,
                    param,
                    commandType: commandType,
                    commandTimeout: timeout,
                    cancellationToken: ct));
        }
    }
}

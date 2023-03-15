using Dapper;
using Npgsql;

namespace ApiPgBench.Web.Storages;

public class PgMetadataStore : IMetadataStore
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<PgMetadataStore> _logger;

    public PgMetadataStore(IConfiguration configuration, ILogger<PgMetadataStore> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    public async Task<(long, string)> GetCountAsync(string tableName)
    {
        await using var connection = await GetConnectionAsync();

        try
        {
            var count = await connection.ExecuteScalarAsync<long>($"SELECT COUNT(*) FROM {tableName}");
            return (count, string.Empty);
        }
        catch (PostgresException exception) when (exception.SqlState == "42P01")
        {
            _logger.LogWarning(exception, "Table {TableName} does not exist.", tableName);
            return (0, "Table does not exist.");
        }
    }

    protected async Task<NpgsqlConnection> GetConnectionAsync()
    {
        var connectionString = _configuration.GetConnectionString("Default");
        var connection = new NpgsqlConnection(connectionString);
        await connection.OpenAsync();

        return connection;
    }
}

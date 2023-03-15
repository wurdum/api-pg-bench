using ApiPgBench.Web.Accounts;
using ApiPgBench.Web.Families;
using ApiPgBench.Web.Transactions;
using Dapper;
using Npgsql;

namespace ApiPgBench.Web.Storages;

public class MigrationExecutionFilter : IStartupFilter
{
    private readonly IStore<TransactionEntity> _transactionsStore;
    private readonly IStore<AccountEntity> _accountStore;
    private readonly IStore<FamilyEntity> _familyStore;
    private readonly ILogger<MigrationExecutionFilter> _logger;

    public MigrationExecutionFilter(
        IStore<TransactionEntity> transactionsStore,
        IStore<AccountEntity> accountStore,
        IStore<FamilyEntity> familyStore,
        ILogger<MigrationExecutionFilter> logger)
    {
        _transactionsStore = transactionsStore;
        _accountStore = accountStore;
        _familyStore = familyStore;
        _logger = logger;
    }

    public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
    {
        return builder =>
        {
            ExecuteMigrations(builder).GetAwaiter().GetResult();

            next(builder);
        };
    }

    private async Task ExecuteMigrations(IApplicationBuilder builder)
    {
        _logger.LogInformation("Executing migrations...");

        var configuration = builder.ApplicationServices.GetRequiredService<IConfiguration>();
        var connectionString = configuration.GetConnectionString("Default");
        await using var connection = new NpgsqlConnection(connectionString);
        await connection.OpenAsync();

        var currentVersion = await GetCurrentVersionAsync(connection);
        if (currentVersion == 0)
        {
            await ExecuteMigration1Async(connection);
        }

        _logger.LogInformation("No migrations to execute.");
    }

    private async Task ExecuteMigration1Async(NpgsqlConnection connection)
    {
        await _familyStore.CreateTableAsync();
        await _accountStore.CreateTableAsync();
        await _transactionsStore.CreateTableAsync();

        await SetDbVersionAsync(connection, 1);
    }

    private async Task<int> GetCurrentVersionAsync(NpgsqlConnection connection)
    {
        var tableExists = await connection.ExecuteScalarAsync<bool>("SELECT to_regclass('public.versions') IS NOT NULL");
        if (tableExists)
        {
            var currentVersion = await connection.ExecuteScalarAsync<int>("SELECT version FROM versions ORDER BY version DESC LIMIT 1");

            _logger.LogInformation("Current db version is {Version}.", currentVersion);

            return currentVersion;
        }

        await connection.ExecuteScalarAsync("CREATE TABLE IF NOT EXISTS versions (version int, created_at timestamp DEFAULT NOW())");

        _logger.LogInformation("Versions table is created.");

        await SetDbVersionAsync(connection, 0);
        return 0;
    }

    private async Task SetDbVersionAsync(NpgsqlConnection connection, int version)
    {
        await connection.ExecuteScalarAsync("INSERT INTO versions (version) VALUES (@version)", new { version });

        _logger.LogInformation("DB version is set to {Version}.", version);
    }
}

using Dapper;
using Npgsql;

namespace ApiPgBench.Web.Storages;

public abstract class PgStore<T> : IStore<T>
    where T : IEntity
{
    protected PgStore(IConfiguration configuration, ILogger logger)
    {
        Configuration = configuration;
        Logger = logger;
    }

    protected IConfiguration Configuration { get; }
    protected ILogger Logger { get; }

    protected abstract string TableName { get; }
    protected abstract string CreateTableSql { get; }
    protected abstract string SelectEntityByKeySql { get; }
    protected virtual string SelectAllEntities => $"SELECT * FROM {TableName}";
    protected abstract string InsertEntitySql { get; }

    protected abstract object GetInsertEntityParameters(T entity);

    public virtual async Task CreateTableAsync()
    {
        await using var connection = await GetConnectionAsync();
        await connection.ExecuteScalarAsync(CreateTableSql);

        Logger.LogInformation("Table {TableName} created.", TableName);
    }

    public virtual async Task<T?> FindAsync(long id)
    {
        await using var connection = await GetConnectionAsync();
        var entity = await connection.QueryFirstOrDefaultAsync<T>(SelectEntityByKeySql, new { id });

        return entity;
    }

    public async Task<IReadOnlyCollection<T>> GetAllAsync()
    {
        await using var connection = await GetConnectionAsync();
        var entities = await connection.QueryAsync<T>(SelectAllEntities);

        return entities.ToArray();
    }

    public async Task<T> AddAsync(T entity)
    {
        await using var connection = await GetConnectionAsync();
        var id = await connection.ExecuteScalarAsync<long>(InsertEntitySql, GetInsertEntityParameters(entity));
        entity.Id = id;
        return entity;
    }

    public virtual async Task AddAllAsync(IReadOnlyCollection<T> entities)
    {
        await using var connection = await GetConnectionAsync();
        await using var tran = await connection.BeginTransactionAsync();

        try
        {
            foreach (var entity in entities)
            {
                await connection.ExecuteAsync(InsertEntitySql, GetInsertEntityParameters(entity), tran);
            }

            await tran.CommitAsync();
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "AddAllAsync for {Count} items failed.", entities.Count);
            await tran.RollbackAsync();
        }
    }

    protected async Task<NpgsqlConnection> GetConnectionAsync()
    {
        var connectionString = Configuration.GetConnectionString("Default");
        var connection = new NpgsqlConnection(connectionString);
        await connection.OpenAsync();

        return connection;
    }
}

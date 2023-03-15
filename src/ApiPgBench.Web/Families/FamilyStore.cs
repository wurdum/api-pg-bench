using ApiPgBench.Web.Storages;

namespace ApiPgBench.Web.Families;

public class FamilyStore : PgStore<FamilyEntity>
{
    public FamilyStore(IConfiguration configuration, ILogger<FamilyStore> logger) : base(configuration, logger)
    {
    }

    protected override string TableName => "families";
    protected override string CreateTableSql => $"""
        CREATE TABLE IF NOT EXISTS {TableName}
        (
            id         bigserial PRIMARY KEY,
            name       varchar(60)             NOT NULL,
            created_at timestamp DEFAULT NOW() NOT NULL
        )
    """;
    protected override string SelectEntityByKeySql => $"SELECT * FROM {TableName} WHERE id = @Id";
    protected override string InsertEntitySql => $"INSERT INTO {TableName} (name) VALUES (@name) RETURNING id";

    protected override object GetInsertEntityParameters(FamilyEntity entity)
    {
        return new { name = entity.Name };
    }
}

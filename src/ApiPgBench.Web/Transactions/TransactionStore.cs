using ApiPgBench.Web.Storages;

namespace ApiPgBench.Web.Transactions;

public class TransactionStore : PgStore<TransactionEntity>
{
    public TransactionStore(IConfiguration configuration, ILogger<TransactionStore> logger)
        : base(configuration, logger)
    {
    }

    protected override string TableName => "transactions";
    protected override string CreateTableSql => $"""
        CREATE TABLE IF NOT EXISTS {TableName}
        (
            id              bigserial PRIMARY KEY,
            account_id      bigint REFERENCES accounts (id) NOT NULL,
            description     varchar(240)                    NOT NULL,
            category        varchar(120)                    NOT NULL,
            amount          decimal                         NOT NULL,
            created_at      timestamp DEFAULT NOW()         NOT NULL
        )
    """;
    protected override string SelectEntityByKeySql => $"SELECT * FROM {TableName} WHERE id = @id";

    protected override string InsertEntitySql => $"""
        INSERT INTO {TableName} (account_id, description, category, amount, created_at)
        VALUES (@accountId, @description, @category, @amount, @createdAt)
        RETURNING id
    """;

    protected override object GetInsertEntityParameters(TransactionEntity entity)
    {
        return new
        {
            accountId = entity.AccountId,
            description = entity.Description,
            category = entity.Category,
            amount = entity.Amount,
            createdAt = entity.CreatedAt
        };
    }
}

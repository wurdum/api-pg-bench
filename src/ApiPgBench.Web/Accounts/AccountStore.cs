using ApiPgBench.Web.Storages;

namespace ApiPgBench.Web.Accounts;

public class AccountStore : PgStore<AccountEntity>
{
    public AccountStore(IConfiguration configuration, ILogger<AccountStore> logger)
        : base(configuration, logger)
    {
    }

    protected override string TableName => "accounts";
    protected override string CreateTableSql => $"""
        CREATE TABLE IF NOT EXISTS {TableName}
        (
            id            bigserial PRIMARY KEY,
            family_id     bigint REFERENCES families (id) NOT NULL,
            name          varchar(60)                     NOT NULL,
            currency      varchar(3)                      NOT NULL,
            account_group varchar(60)                     NOT NULL,
            created_at    timestamp DEFAULT NOW()         NOT NULL
        )
    """;
    protected override string SelectEntityByKeySql => $"SELECT * FROM {TableName} WHERE id = @id";
    protected override string InsertEntitySql => $"""
        INSERT INTO {TableName} (family_id, name, currency, account_group)
        VALUES (@familyId, @name, @currency, @accountGroup)
        RETURNING id
    """;

    protected override object GetInsertEntityParameters(AccountEntity entity)
    {
        return new
        {
            familyId = entity.FamilyId,
            name = entity.Name,
            currency = entity.Currency,
            accountGroup = entity.AccountGroup.ToString()
        };
    }
}

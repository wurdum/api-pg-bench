using ApiPgBench.Web.Accounts;
using ApiPgBench.Web.Families;
using ApiPgBench.Web.Metadata;
using ApiPgBench.Web.Storages;
using ApiPgBench.Web.Transactions;

Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddTransient<IStartupFilter, MigrationExecutionFilter>();
builder.Services.AddTransient<IMetadataStore, PgMetadataStore>();
builder.Services.AddTransient<IStore<TransactionEntity>, TransactionStore>();
builder.Services.AddTransient<IStore<AccountEntity>, AccountStore>();
builder.Services.AddTransient<IStore<FamilyEntity>, FamilyStore>();

var app = builder.Build();

app.MapGroup("/")
    .MapMetadata()
    .MapFamilies()
    .MapAccounts()
    .MapTransactions();

app.MapGet("/", () => "Hello World!");
app.Run();

public partial class Program { }

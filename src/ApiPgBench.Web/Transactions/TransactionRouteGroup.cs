using ApiPgBench.Web.Storages;

namespace ApiPgBench.Web.Transactions;

public static class TransactionRouteGroup
{
    public static RouteGroupBuilder MapTransactions(this RouteGroupBuilder builder)
    {
        builder.MapGet("/transactions/{transactionId:long}", async (long transactionId, IStore<TransactionEntity> store) =>
        {
            var entity = await store.FindAsync(transactionId);

            return entity == null
                ? Results.NotFound()
                : Results.Ok(entity);
        });

        builder.MapPost("/transactions", async (TransactionViewModel user, IStore<TransactionEntity> store) =>
        {
            var entity = TransactionEntity.From(user);
            var created = await store.AddAsync(entity);

            return Results.Json(created.ToViewModel(), statusCode: StatusCodes.Status201Created);
        });

        return builder;
    }
}

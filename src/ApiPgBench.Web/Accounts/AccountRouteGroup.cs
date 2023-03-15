using ApiPgBench.Web.Storages;

namespace ApiPgBench.Web.Accounts;

public static class AccountRouteGroup
{
    public static RouteGroupBuilder MapAccounts(this RouteGroupBuilder builder)
    {
        builder.MapGet("/accounts/{id:long}", async (long id, IStore<AccountEntity> store) =>
        {
            var entity = await store.FindAsync(id);

            return entity == null
                ? Results.NotFound()
                : Results.Ok(entity);
        });

        builder.MapPost("/accounts", async (AccountViewModel user, IStore<AccountEntity> store) =>
        {
            var entity = AccountEntity.From(user);
            var created = await store.AddAsync(entity);

            return Results.Json(created.ToViewModel(), statusCode: StatusCodes.Status201Created);
        });

        return builder;
    }
}

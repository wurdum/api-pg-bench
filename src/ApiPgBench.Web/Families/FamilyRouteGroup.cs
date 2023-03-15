using ApiPgBench.Web.Storages;

namespace ApiPgBench.Web.Families;

public static class FamilyRouteGroup
{
    public static RouteGroupBuilder MapFamilies(this RouteGroupBuilder builder)
    {
        builder.MapGet("/families/{id:long}", async (long id, IStore<FamilyEntity> store) =>
        {
            var entity = await store.FindAsync(id);

            return entity == null
                ? Results.NotFound()
                : Results.Ok(entity);
        });

        builder.MapPost("/families", async (FamilyViewModel user, IStore<FamilyEntity> store) =>
        {
            var entity = FamilyEntity.From(user);
            var created = await store.AddAsync(entity);

            return Results.Json(created.ToViewModel(), statusCode: StatusCodes.Status201Created);
        });

        return builder;
    }
}

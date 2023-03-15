using ApiPgBench.Web.Storages;

namespace ApiPgBench.Web.Metadata;

public static class MetadataRouteGroup
{
    public static RouteGroupBuilder MapMetadata(this RouteGroupBuilder builder)
    {
        builder.MapGet("/counts/{tableName}", async (string tableName, IMetadataStore metadataStore) =>
        {
            if (string.IsNullOrWhiteSpace(tableName))
            {
                return Results.Problem("No table name provided.", statusCode: 400);
            }

            var (count, error) = await metadataStore.GetCountAsync(tableName);
            if (!string.IsNullOrWhiteSpace(error))
            {
                return Results.Problem(error, statusCode: 400);
            }

            return Results.Json(new CountViewModel(count));
        });

        return builder;
    }
}

public record CountViewModel(long Value);

namespace ApiPgBench.Web.Storages;

public interface IMetadataStore
{
    Task<(long, string)> GetCountAsync(string tableName);
}

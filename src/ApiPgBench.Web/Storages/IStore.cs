namespace ApiPgBench.Web.Storages;

public interface IStore<T>
    where T : IEntity
{
    Task CreateTableAsync();
    Task<T?> FindAsync(long id);
    Task<IReadOnlyCollection<T>> GetAllAsync();
    Task<T> AddAsync(T entity);
    Task AddAllAsync(IReadOnlyCollection<T> entities);
}

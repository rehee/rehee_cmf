namespace ReheeCmf.DTOProcessors.Processors
{
  public interface ITypeQuery
  {
    IQueryable Query(TokenDTO user);
    IQueryable Query(TokenDTO user, string key);
    string TypeName { get; }
    Type Type { get; }
  }
  public interface IFindByQueryKey
  {
    Task<object> FindAsync(TokenDTO user, string queryKey, CancellationToken ct);
  }
  public interface IFindByQueryKey<T>
  {
    Task<T> FindAsyncWithType(TokenDTO user, string queryKey, CancellationToken ct);
  }
  public interface ITypeQuery<T> : ITypeQuery where T : IQueryKey
  {
    IQueryable<T> QueryWithType(TokenDTO user);
  }
}

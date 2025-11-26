namespace ReheeCmf.Services
{
  public interface IUserLockendStorage
  {
    Task<(bool HasRecord, DateTimeOffset? Lockend)> GetUserLockendAsync(string userName, CancellationToken ct = default);
    Task AddOrUpdateUserLockendAsync(string userName, DateTimeOffset? lockend, CancellationToken ct = default);
  }
}

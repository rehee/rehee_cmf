namespace ReheeCmf.Contexts
{
  public interface IContext : IContextRepository
  {



    TokenDTO? User { get; }

    int SaveChanges(TokenDTO? user);
    Task<int> SaveChangesAsync(TokenDTO? user, CancellationToken ct = default);
    Guid? TenantId { get; }

  }

}

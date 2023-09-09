namespace ReheeCmf.Contexts
{
  public interface IContext : ISaveChange, IRepository, IWithTenant, ITenantContext, IDisposable
  {
    object? Context { get; }
    TokenDTO? User { get; }
  }
}

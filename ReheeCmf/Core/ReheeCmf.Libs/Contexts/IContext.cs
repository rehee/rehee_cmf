namespace ReheeCmf.Contexts
{
  public interface IContext : ISaveChange, IRepository, IWithTenant, ITenantContext, IDisposable
  {
    TokenDTO? User { get; }
  }
}

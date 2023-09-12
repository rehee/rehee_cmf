using ReheeCmf.Handlers.ChangeHandlerss;

namespace ReheeCmf.Contexts
{
  public interface ICrudTracker
  {
    Task AfterSaveChangesAsync(CancellationToken ct = default);
    void AddingTracker(Type entityType, object entity);
    IEnumerable<IChangeHandler> GetHandlers(object entity);
  }
}

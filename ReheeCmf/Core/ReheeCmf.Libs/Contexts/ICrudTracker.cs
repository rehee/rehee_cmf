namespace ReheeCmf.Contexts
{
  public interface ICrudTracker
  {
    Task AfterSaveChangesAsync(CancellationToken ct = default);
    void AddingTracker(Type entityType, object entity);
    IEnumerable<IEntityChangeHandler> GetHandlers(object entity);
  }
}

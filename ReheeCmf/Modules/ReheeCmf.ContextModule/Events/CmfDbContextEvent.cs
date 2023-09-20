using Microsoft.EntityFrameworkCore.ChangeTracking;
using ReheeCmf.Attributes;
using ReheeCmf.ContextModule.Contexts;
using ReheeCmf.Handlers.ChangeHandlers;
using System.ComponentModel.DataAnnotations;

namespace ReheeCmf.ContextModule.Events
{
  public static class CmfDbContextEvent
  {
    public static ICrudTracker? TryGetTracker(object? sender)
    {
      if (sender == null)
      {
        return null;
      }
      if (sender is ChangeTracker changeTracker)
      {
        return TryGetTracker(changeTracker.Context);
      }
      if (sender is IWithContext with)
      {
        if (with.Context is ICrudTracker dbContext)
        {
          return dbContext;
        }
      }
      if (sender is ICrudTracker tracker)
      {
        return tracker;
      }
      return null;
    }
    public static void SavedChangesEventArgs(object sender, SavedChangesEventArgs e)
    {
      var db = TryGetTracker(sender);
      if (db != null && e.AcceptAllChangesOnSuccess)
      {
        db.AfterSaveChangesAsync().Wait();
      }
    }
    public static void ChangeTracker_Tracked(object sender, EntityTrackedEventArgs e)
    {

      var entity = e.Entry.Entity;
      var db = TryGetTracker(sender);
      if (db != null)
      {
        db.AddingTracker(entity.GetType(), entity);
      }
    }
    public static void ValidateEntity(IEnumerable<IChangeHandler>? handlers)
    {
      if (handlers?.Any() == true)
      {
        IEnumerable<ValidationResult>? result = null;
        Task.Run(async () =>
        {
          var r = new List<ValidationResult>();
          foreach (var h in handlers)
          {
            r.AddRange(await h.ValidationAsync());
          }
          result = r.ToArray();
          r = null;
        }).Wait();
        if (result?.Any() == true)
        {
          StatusException.Throw(result.ToArray());
        }
      }
    }
    public static void ChangeTracker_StateChanged(object sender, EntityEntryEventArgs e)
    {
      var db = TryGetTracker(sender);
      if (db != null)
      {
        var entity = e.Entry.Entity;

        switch (e.Entry.State)
        {

          case EntityState.Added:
          case EntityState.Modified:
          case EntityState.Deleted:
            db.AddingTracker(entity.GetType().ThisType(), entity);
            break;
        }
      ReCheckLogic:
        switch (e.Entry.State)
        {
          case EntityState.Added:
            var handlers = db.GetHandlers(entity);
            foreach (var handler in handlers)
            {
              handler.BeforeCreateAsync().Wait();
            }
            ValidateEntity(handlers);
            break;
          case EntityState.Modified:
            var handlersUpdate = db.GetHandlers(entity);
            var entityChanges = e.Entry.Properties.Where(b =>
           b.CurrentValue.StringValue(b.Metadata.ClrType) != b.OriginalValue.StringValue(b.Metadata.ClrType))
             .Select(p => new EntityChanges(p.Metadata.Name, p.Metadata.ClrType, p.CurrentValue, p.OriginalValue))
             .ToArray();
            foreach (var handler in handlersUpdate)
            {
              handler.BeforeUpdateAsync(entityChanges).Wait();
            }
            ValidateEntity(handlersUpdate);
            break;
          case EntityState.Deleted:
            var handlersDelete = db.GetHandlers(entity);
            var deleteHandlers = handlersDelete.Where(b => b is IDeletedHandler)
              .Select(b => b as IDeletedHandler).Select(b => b!);
            if (deleteHandlers?.Any() == true)
            {
              Task.WaitAll(deleteHandlers.Select(b => b.DeleteAsync()).ToArray());
              if (deleteHandlers.Any(b => b.IsDeleted) == false)
              {
                e.Entry.State = EntityState.Modified;
                goto ReCheckLogic;
              }
            }

            foreach (var handler in handlersDelete)
            {
              handler.BeforeDeleteAsync().Wait();
            }
            break;
        }
      }
    }

  }
}

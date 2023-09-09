using ReheeCmf.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReheeCmf.Handlers.EntityChangeHandlers
{
  public interface IEntityChangeTrackerComponent : ICmfComponent
  {
    Type EntityType { get; }
    bool NoInherit { get; }

    IEntityChangeHandler CreateEntityChangeHandler(IServiceProvider sp, object entity);
  }
  public class EntityChangeTrackerAttribute<TEntity, THandler> : CmfComponentAttribute<THandler>, IEntityChangeTrackerComponent where THandler : IEntityChangeHandler, new()
  {
    public Type EntityType => typeof(TEntity);
    public bool NoInherit { get; set; }
    public override int GetHashCode()
    {
      unchecked
      {
        return base.GetHashCode() * 11 + EntityType.GetHashCode();
      }
    }
    public IEntityChangeHandler CreateEntityChangeHandler(IServiceProvider sp, object entity)
    {
      var handler = new THandler();
      handler.Init(sp, entity, Index, SubIndex, Group);
      return handler;
    }
  }
}

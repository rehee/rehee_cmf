using ReheeCmf.Handlers.ChangeHandlers.EntityChangeHandlers;

namespace ReheeCmf.Attributes
{
  public interface IEntityChangeComponent : IChangeComponent
  {
  }

  public class EntityChangeAttribute<T> : ChangeComponentEntityAttribute<T>, IEntityChangeComponent where T : IEntityChangeHandler, new()
  {
    public override bool IsAvailable(Type type)
    {
      if (EntityType == null || EntityType.IsInterface)
      {
        return false;
      }
      return type.IsInterface ?
          EntityType.IsImplement(type) :
          NoInherit ? EntityType.Equals(type) : type.IsInheritance(EntityType);
    }
  }

  public class EntityChangeTrackerAttribute<TEntity> : ChangeComponentHandlerAttribute<TEntity>, IEntityChangeComponent
    where TEntity : class
  {
    public override bool IsAvailable(Type type)
    {
      if (EntityType == null || EntityType.IsInterface)
      {
        return false;
      }
      return type.IsInterface ?
          EntityType.IsImplement(type) :
          NoInherit ? EntityType.Equals(type) : type.IsInheritance(EntityType);
    }
  }

}

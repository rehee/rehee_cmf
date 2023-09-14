using ReheeCmf.Handlers.ChangeHandlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReheeCmf.Components.ChangeComponents
{
  public interface IChangeComponent : ICmfComponent
  {
    bool NoInherit { get; }
    bool IsAvailable(Type type);
    IChangeHandler? CreateChangeHandler(IServiceProvider sp, object entity);
  }
  public abstract class ChangeComponentAttribute : CmfComponentAttribute, IChangeComponent
  {
    public abstract bool IsAvailable(Type type);
    public bool NoInherit { get; set; }
    public virtual IChangeHandler? CreateChangeHandler(IServiceProvider sp, object entity)
    {
      var handler = CreateHandler();
      if (handler != null && handler is IChangeHandler ch)
      {
        ch.Init(sp, entity, Index, SubIndex, Group);
        return ch;
      }
      return null;
    }
  }

  public abstract class ChangeComponentEntityAttribute<THandler> : ChangeComponentAttribute, IEntityComponent
  {
    public override Type? HandlerType => typeof(THandler);
  }
  public abstract class ChangeComponentHandlerAttribute<TEntity> : ChangeComponentAttribute, IHandlerComponent
  {
    public override Type? EntityType => typeof(TEntity);
  }

}

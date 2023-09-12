using ReheeCmf.Handlers.ChangeHandlerss;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReheeCmf.Components.ChangeComponents
{
  public interface IChangeComponent : ICmfComponent
  {
    Type? EntityType { get; }
    bool NoInherit { get; }
    bool IsAvailable(Type type);
    IChangeHandler CreateChangeHandler(IServiceProvider sp, object entity);
  }
  public abstract class ChangeComponentAttribute<T> : CmfComponentAttribute<T>, IChangeComponent where T : IChangeHandler, new()
  {
    public abstract bool IsAvailable(Type type);
    public virtual Type? EntityType { get; set; }
    public bool NoInherit { get; set; }
    public virtual IChangeHandler CreateChangeHandler(IServiceProvider sp, object entity)
    {
      var handler = new T();
      handler.Init(sp, entity, Index, SubIndex, Group);
      return handler;
    }
  }

}

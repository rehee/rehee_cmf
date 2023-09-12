using ReheeCmf.Handlers.InterfaceChangeHandlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReheeCmf.Components.ChangeComponents
{
  public interface IInterfaceChangeComponent : IChangeComponent
  {
  }
  public class InterfaceChangeComponentAttribute<T> : ChangeComponentAttribute<T>, IInterfaceChangeComponent where T : IInterfaceChangeHandler, new()
  {

  }
  public class InterfaceChangeTrackerAttribute<TInterface, THandler> : InterfaceChangeComponentAttribute<THandler> where THandler : IInterfaceChangeHandler, new()
  {
    public override Type EntityType => typeof(TInterface);
    public override int GetHashCode()
    {
      unchecked
      {
        return base.GetHashCode() * 11 + EntityType.GetHashCode();
      }
    }
  }
}

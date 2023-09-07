using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReheeCmf.Attributes
{
  public class EntityChangeHandlerAttribute : Attribute
  {
    public Type? HanderType { get; set; }
    public virtual IEntityChangeHandler? GetHandler()
    {
      if (HanderType == null)
      {
        return null;
      }
      var handler = Activator.CreateInstance(HanderType);
      return (IEntityChangeHandler)handler!;
    }
  }

  public class EntityChangeHandlerAttribute<T> : EntityChangeHandlerAttribute where T : IEntityChangeHandler, new()
  {
    public IEntityChangeHandler GetTypedHandler()
    {
      return new T();
    }
    public override IEntityChangeHandler? GetHandler()
    {
      return GetTypedHandler();
    }
  }
}

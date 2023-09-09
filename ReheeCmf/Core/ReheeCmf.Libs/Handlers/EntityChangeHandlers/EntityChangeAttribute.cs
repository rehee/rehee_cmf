using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReheeCmf.Components;

namespace ReheeCmf.Handlers.EntityChangeHandlers
{
  public interface IEntityChangeComponent : ICmfComponent
  {
  }
  public class EntityChangeAttribute<T> : CmfComponentAttribute<T>, IEntityChangeComponent where T : IEntityChangeHandler, new()
  {
  }
}

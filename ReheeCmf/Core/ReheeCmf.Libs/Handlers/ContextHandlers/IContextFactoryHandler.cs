using ReheeCmf.Components;
using ReheeCmf.Components.ChangeComponents;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReheeCmf.Handlers.ContextHandlers
{
  public interface IContextFactoryComponent : ICmfComponent
  {

  }


  public class ContextFactoryComponentAttribute<T> : CmfComponentAttribute, IEntityComponent, IContextFactoryComponent where T : ICmfHandler, new()
  {
    public override Type? HandlerType => typeof(T);
  }
  public interface IContextFactoryHandler : ICmfHandler
  {
    IContext CreateContext(IServiceProvider serviceProvider);
  }
}

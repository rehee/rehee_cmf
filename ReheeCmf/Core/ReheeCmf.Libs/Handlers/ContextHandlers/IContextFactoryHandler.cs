using ReheeCmf.Components;
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

  public class ContextFactoryComponentAttribute<T> : CmfComponentAttribute<T>, IContextFactoryComponent where T : ICmfHandler, new()
  {
  }
  public interface IContextFactoryHandler : ICmfHandler
  {
    IContext CreateContext(IServiceProvider serviceProvider);
  }
}

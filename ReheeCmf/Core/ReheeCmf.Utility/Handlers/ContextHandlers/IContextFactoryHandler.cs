using ReheeCmf.Components;
using ReheeCmf.Contexts;

namespace ReheeCmf.Handlers.ContextHandlers
{
  public interface IContextFactoryHandler : ICmfHandler
  {
    IContext CreateContext(IServiceProvider serviceProvider);
  }
}

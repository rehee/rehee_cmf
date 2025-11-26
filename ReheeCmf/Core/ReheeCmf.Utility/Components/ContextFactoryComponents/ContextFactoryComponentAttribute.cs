using ReheeCmf.Attributes;
using ReheeCmf.Components;
using ReheeCmf.Handlers.ContextHandlers;

namespace ReheeCmf.Components.ContextFactoryComponents
{
  public class ContextFactoryComponentAttribute<T> : CmfComponentAttribute, IEntityComponent, IContextFactoryComponent where T : ICmfHandler, new()
  {
    public override Type? HandlerType => typeof(T);
  }
}

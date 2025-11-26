using ReheeCmf.Attributes;
using ReheeCmf.Handlers.SelectEntityHandlers;

namespace ReheeCmf.Components.SelectEntityComponents
{
  public class SelectEntityAttribute<TEntity> : CmfComponentAttribute, ISelectEntityComponent, IHandlerComponent
  {
    public override Type? EntityType => typeof(TEntity);
    public ISelectEntityHandler? GetSelectHandler()
    {
      return SingletonHandler() as ISelectEntityHandler;
    }

  }
}

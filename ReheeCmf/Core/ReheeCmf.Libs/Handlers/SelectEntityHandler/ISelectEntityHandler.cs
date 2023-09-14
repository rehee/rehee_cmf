using ReheeCmf.Components;
using ReheeCmf.Components.ChangeComponents;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReheeCmf.Handlers.SelectHandler
{
  public interface ISelectEntityHandler : ICmfHandler
  {
    IEnumerable<KeyValueItemDTO> GetSelectItem(IContext context);
  }

  public interface ISelectEntityComponent : ICmfComponent
  {
    ISelectEntityHandler? GetSelectHandler();
  }

  public class SelectEntityAttribute<TEntity> : CmfComponentAttribute, ISelectEntityComponent, IHandlerComponent
  {
    public override Type? EntityType => typeof(TEntity);
    public ISelectEntityHandler? GetSelectHandler()
    {
      return SingletonHandler() as ISelectEntityHandler;
    }

  }
  public abstract class SelectEntityHandler<T> : ISelectEntityHandler where T : class, ISelect
  {
    public abstract IEnumerable<KeyValueItemDTO> GetSelectItem(IContext context);
  }
}

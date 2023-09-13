using ReheeCmf.Components;
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
    Type EntityType { get; }
    ISelectEntityHandler? GetSelectHandler();
  }
  public class SelectEntityAttribute<TEntity, THandler> : CmfComponentAttribute<THandler>, ISelectEntityComponent
    where THandler : ISelectEntityHandler, new()
    where TEntity : class, ISelect
  {
    public Type EntityType => typeof(TEntity);
    public ISelectEntityHandler? GetSelectHandler()
    {
      return SingletonHandler<THandler>();
    }
    public override int GetHashCode()
    {
      return base.GetHashCode() * EntityType.GetHashCode();
    }
  }

  public abstract class SelectEntityHandler<TEntity> : ISelectEntityHandler where TEntity : class, ISelect
  {
    public abstract IEnumerable<KeyValueItemDTO> GetSelectItem(IContext context);
  }

}

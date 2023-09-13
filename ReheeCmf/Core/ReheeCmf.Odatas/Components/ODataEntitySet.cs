using Microsoft.OData.ModelBuilder;
using ReheeCmf.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReheeCmf.ODatas.Components
{
  public interface IODataEntitySet : ICmfComponent
  {
    Type EntityType { get; }
    IODataEntitySetHandler? GetHandler();
  }
  public class ODataEntitySetAttribute<TEntity, THandler> : CmfComponentAttribute<THandler>, IODataEntitySet
    where THandler : IODataEntitySetHandler, new()
  {
    public Type EntityType => typeof(TEntity);
    public IODataEntitySetHandler? GetHandler()
    {
      return SingletonHandler<THandler>();
    }
    public override int GetHashCode()
    {
      return base.GetHashCode() * EntityType.GetHashCode();
    }
  }

  public interface IODataEntitySetHandler : ICmfHandler
  {
    object EntitySet(ODataConventionModelBuilder builder);
  }
  public abstract class ODataEntitySetHandler<TEntity> : IODataEntitySetHandler where TEntity : class
  {
    public abstract object EntitySet(ODataConventionModelBuilder builder);
  }
}

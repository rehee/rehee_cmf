using Microsoft.OData.ModelBuilder;
using ReheeCmf.Components;
using ReheeCmf.Components.ChangeComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReheeCmf.ODatas.Components
{
  public interface IODataEntitySet : ICmfComponent
  {
    IODataEntitySetHandler? GetHandler();
  }
  public class ODataEntitySetAttribute<TEntity> : CmfComponentAttribute, IODataEntitySet, IHandlerComponent
  {
    public override Type EntityType => typeof(TEntity);
    public IODataEntitySetHandler? GetHandler()
    {
      return SingletonHandler() as IODataEntitySetHandler;
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

using Microsoft.OData.ModelBuilder;
using ReheeCmf.Components;
using ReheeCmf.Components.ChangeComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ReheeCmf.ODatas.Components
{
  public interface IODataEntitySet : ICmfComponent
  {

    IODataEntitySetHandler? GetHandler(Type calledType);
  }
  public class ODataEntitySetAttribute<TEntity> : CmfComponentAttribute, IODataEntitySet, IHandlerComponent
  {
    public override Type EntityType => typeof(TEntity);
    public IODataEntitySetHandler? GetHandler(Type actualType)
    {
      if (CreateHandler() is IODataEntitySetHandler handler)
      {
        handler!.ActualType = actualType;

        return handler;
      }
      return null;
    }
  }

  public interface IODataEntitySetHandler : ICmfHandler
  {
    Type? ActualType { get; set; }
    object EntitySet(ODataConventionModelBuilder builder, string? name = null);
    MethodInfo? GetConfigurationMethod { get; set; }
    MethodInfo? ConfigurationEntitySetMethod { get; set; }
  }
  public abstract class ODataEntitySetHandler<TEntity> : IODataEntitySetHandler where TEntity : class
  {
    public Type? ActualType { get; set; }
    protected string? OverrideName { get; set; }
    protected string? SetName => OverrideName ?? ActualType?.Name ?? string.Empty;

    public MethodInfo? GetConfigurationMethod { set; get; }
    public MethodInfo? ConfigurationEntitySetMethod { set; get; }

    public virtual EntityTypeConfiguration<T> GetConfiguration<T>(
      ODataConventionModelBuilder builder) where T : class, TEntity
    {
      return builder.EntitySet<T>(SetName).EntityType;
    }
    public virtual void ConfigurationEntitySet<T>(
      EntityTypeConfiguration<T> entitySet) where T : class, TEntity
    {

    }

    protected object GetEntitySet(ODataConventionModelBuilder builder)
    {
      var methods = this.GetType().GetMethods().FirstOrDefault(b => b.Name == "GetConfiguration");
      return methods!.MakeGenericMethod(ActualType!)
        .Invoke(this, new object[] { builder })!;
    }
    protected void ConfigEntitySet(object input)
    {
      this.GetType().GetMethod("ConfigurationEntitySet")!.MakeGenericMethod(ActualType!)
        .Invoke(this, new object[] { input });
    }
    public object EntitySet(ODataConventionModelBuilder builder, string? name = null)
    {
      OverrideName = name;
      var obj = GetEntitySet(builder);
      ConfigEntitySet(obj);
      return obj;
    }
  }
}

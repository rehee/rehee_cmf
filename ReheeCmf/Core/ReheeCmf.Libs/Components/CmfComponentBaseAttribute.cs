using ReheeCmf.Utility.CmfRegisters;

namespace ReheeCmf.Components
{
  [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
  public abstract class CmfComponentAttribute : Attribute, ICmfComponent
  {
    public int Index { get; set; }
    public int SubIndex { get; set; }
    public string? Group { get; set; }
    public bool Unique { get; set; }
    public bool SkipFollowing { get; set; }
    public virtual Type? HandlerType { get; set; }
    public virtual Type? EntityType { get; set; }
    public virtual Type? PropertyType { get; set; }

    public virtual ICmfHandler? CreateHandler()
    {
      if (HandlerType == null || !HandlerType.IsImplement<ICmfHandler>())
      {
        return null;
      }
      var handler = Activator.CreateInstance(HandlerType);
      if (handler is ICmfHandler h)
      {
        return h;
      }
      return handler as ICmfHandler;
    }

    public virtual THandler? CreateHandler<THandler>() where THandler : ICmfHandler
    {
      var handler = CreateHandler();
      if (handler == null)
      {
        return default(THandler?);
      }
      if (handler is THandler t)
      {
        return t;
      }
      return default(THandler?);
    }

    public virtual ICmfHandler? SingletonHandler()
    {
      return CmfRegister.SingletonHandlerPool.GetOrAdd(
        HandlerType?.GetHashCode() ?? 0, (key) => CreateHandler()!);
    }

    public virtual THandler? SingletonHandler<THandler>() where THandler : ICmfHandler
    {
      if (SingletonHandler() is THandler t)
      {
        return t;
      }
      return default(THandler?);
    }
    public override int GetHashCode()
    {
      unchecked
      {
        var typeHash = this.GetType().GetHashCode();
        int hash = typeHash;
        hash = hash * 21 + Index.GetHashCode();
        hash = hash * 22 + (HandlerType?.GetHashCode() ?? 0);
        hash = hash * 23 + (EntityType?.GetHashCode() ?? 0);
        return hash;
      }
    }
  }
}

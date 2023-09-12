using ReheeCmf.Components;

namespace ReheeCmf
{
  [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
  public class CmfComponentAttribute<T> : Attribute, ICmfComponent where T : ICmfHandler, new()
  {
    private static readonly Lazy<T> lazyInstance = new Lazy<T>(() => new T());

    public int Index { get; set; }
    public int SubIndex { get; set; }
    public string? Group { get; set; }
    public bool Unique { get; set; }
    public bool SkipFollowing { get; set; }
    public Type HandlerType => typeof(T);
    public ICmfHandler CreateHandler()
    {
      return new T();
    }
    public ICmfHandler SingletonHandler()
    {
      return lazyInstance.Value;
    }
    public THandler? CreateHandler<THandler>() where THandler : ICmfHandler
    {
      if (CreateHandler() is THandler handler)
      {
        return handler;
      }
      return default(THandler?);
    }
    public THandler? SingletonHandler<THandler>() where THandler : ICmfHandler
    {
      if (SingletonHandler() is THandler handler)
      {
        return handler;
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
        hash = hash * 22 + HandlerType.GetHashCode(); 
        return hash;
      }
    }
  }
}

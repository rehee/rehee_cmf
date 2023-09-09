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
        int hash = 17; // 初始哈希码值，可以是任意非零常数
        hash = hash * 23 + Index.GetHashCode(); // 考虑 Index 属性
        hash = hash * 23 + HandlerType.GetHashCode(); // 考虑 HandlerType 属性

        return hash;
      }
    }
  }
}

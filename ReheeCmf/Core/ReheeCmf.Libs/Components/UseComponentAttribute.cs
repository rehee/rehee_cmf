using ReheeCmf.Components;

namespace ReheeCmf
{
  public interface IComponentHandler
  {
    ICmfComponent CreateComponent();
    ICmfComponent SingletonComponent();
    THandler? SingletonComponent<THandler>() where THandler : ICmfComponent;
    Type ComponentType { get; }
  }

  public interface IComponentHandler<T> : IComponentHandler where T : ICmfComponent
  {

  }

  [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
  public class ComponentHandlerAttribute<T> : Attribute, IComponentHandler<T> where T : ICmfComponent, new()
  {
    private static readonly Lazy<T> lazyInstance = new Lazy<T>(() => new T());

    public int Index { get; set; }
    public Type ComponentType => typeof(T);
    public ICmfComponent CreateComponent()
    {
      return new T();
    }
    public ICmfComponent SingletonComponent()
    {
      return lazyInstance.Value;
    }

    public THandler? SingletonComponent<THandler>() where THandler : ICmfComponent
    {
      if (SingletonComponent() is THandler handler)
      {
        return handler;
      }
      return default(THandler?);
    }
  }
}

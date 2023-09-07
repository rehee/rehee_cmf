namespace ReheeCmf.Commons
{
  public interface IContextScope<T>
  {
    T? Value { get; }
    void SetValue(T? value);
    void PropertyChange();
    event EventHandler<ContextScopeEventArgs<T>> ValueChange;
  }
  public class ContextScopeEventArgs<T> : EventArgs
  {
    public T? Value { get; set; }
  }
  public class EventArgs<T> : EventArgs
  {
    public T? Value { get; set; }
  }

  public class ContextScope<T> : IContextScope<T>, IAsyncDisposable, IDisposable
  {
    public virtual T? Value => value;
    protected virtual T? value { get; set; }
    public virtual void SetValue(T? value)
    {
      this.value = value;
      if (this.valueChange != null)
      {
        this.valueChange(this, new ContextScopeEventArgs<T>() { Value = Value });
      }
    }

    List<EventHandler<ContextScopeEventArgs<T>>> delegates = new List<EventHandler<ContextScopeEventArgs<T>>>();
    private event EventHandler<ContextScopeEventArgs<T>> valueChange;
    public virtual event EventHandler<ContextScopeEventArgs<T>> ValueChange
    {
      add
      {
        valueChange += value;
        delegates.Add(value);
      }
      remove
      {
        valueChange -= value;
        delegates.Remove(value);
      }

    }
    protected bool _disposed { get; set; }
    public void Dispose()
    {
      if (_disposed)
      {
        return;
      }
      _disposed = true;
      try
      {
        var list = delegates.Select(b => b).ToArray();
        foreach (var d in list)
        {
          ValueChange -= d;
        }
        delegates.Clear();
        list = null;
      }
      catch { }
    }
    public ValueTask DisposeAsync()
    {
      Dispose();
      return ValueTask.CompletedTask;
    }

    public void PropertyChange()
    {
      if (this.valueChange != null)
      {
        this.valueChange(this, new ContextScopeEventArgs<T>() { Value = Value });
      }
    }
  }
}

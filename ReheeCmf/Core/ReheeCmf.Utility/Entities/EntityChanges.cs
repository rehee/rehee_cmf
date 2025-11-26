using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReheeCmf.Entities
{
  public class EntityChanges
  {
    public string? PropertyName { get; set; }
    public string? NewValue { get; set; }
    public string? OldValue { get; set; }
    public object? NewValueObj { get; set; }
    public object? OldValueObj { get; set; }
    public Type? ThisType { get; set; }
    public EntityChanges()
    {

    }
    public EntityChanges(string propertyName, string newValue, string oldValue, Type? type = null, object? newValueObj = null, object? oldValueObj = null)
    {
      this.PropertyName = propertyName;
      this.NewValue = newValue;
      this.OldValue = oldValue;
      this.ThisType = type;
      this.NewValueObj = newValueObj;
      this.OldValueObj = oldValueObj;
    }
    public EntityChanges(string propertyName, Type? type, object? newValueObj, object? oldValueObj)
    {
      this.PropertyName = propertyName;
      this.NewValueObj = newValueObj;
      this.OldValueObj = oldValueObj;
      //this.NewValue = newValueObj?.StringValue(type) ?? null;
      //this.OldValue = oldValueObj?.StringValue(type) ?? null;
      this.ThisType = type;
    }
  }
}

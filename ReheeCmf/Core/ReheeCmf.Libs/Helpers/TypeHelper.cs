using ReheeCmf.StandardInputs.StandardItems;
using System.Net;
using System.Reflection;

namespace ReheeCmf.Helpers
{
  public static class TypeHelperExtensions
  {
    public static ContentResponse<object> CreateObject(this StandardItem baseItem)
    {
      var result = new ContentResponse<object>();
      if (baseItem == null)
      {
        result.Status = HttpStatusCode.NotFound;
        return result;
      }
      var type = Type.GetType($"{baseItem.FullTypeName}, {baseItem.Assembly}");
      if (type == null)
      {
        result.Status = HttpStatusCode.NotFound;
        return result;
      }
      if (type.IsAbstract || type.IsInterface)
      {
        result.Status = HttpStatusCode.BadRequest;
        return result;
      }
      try
      {
        var obj = Activator.CreateInstance(type);

        result.Status = HttpStatusCode.NotFound;
        result.Success = true;
        result.Content = obj;
        return result;
      }
      catch (Exception ex)
      {
        ex.ThrowStatusException();
        result.Status = HttpStatusCode.InternalServerError;
        result.ErrorMessage = ex.Message;

        return result;
      }

    }
    public static ContentResponse<T> Invoke<T>(this object input, string name, params object[] parameters)
    {
      return input.GetType().Invoke<T>(name, input, parameters);
    }
    public static ContentResponse<T> Invoke<T>(this Type input, string name, object obj, params object[] parameters)
    {
      var result = new ContentResponse<T>();
      try
      {
        result.SetSuccess((T)InvokeMethod(input, name, obj, parameters));
      }
      catch (Exception ex)
      {
        ex.ThrowStatusException();
        result.ErrorMessage = ex.Message;
      }
      return result;
    }
    public static ContentResponse<Object> Invoke(this object input, string name, params object[] parameters)
    {
      return input.GetType().Invoke(name, input, parameters);
    }
    public static ContentResponse<object> Invoke(this Type input, string name, object obj, params object[] parameters)
    {
      var result = new ContentResponse<object>();
      try
      {
        result.SetSuccess(InvokeMethod(input, name, obj, parameters));
      }
      catch (Exception ex)
      {
        ex.ThrowStatusException();
        result.ErrorMessage = ex.Message;
      }
      return result;
    }

    private static object InvokeMethod(Type input, string name, object obj, params object[] parameters)
    {
      object result = null;
      var methods = input.GetMap().Methods.Where(b => b.Name == name);
      if (methods.Count() <= 0)
      {
        return result;
      }
      if (methods.Count() == 1)
      {
        goto invokeMethod;
      }

      if (parameters == null)
      {
        methods = methods.Where(b => b.GetParameters().Length == 0);
        if (methods.Count() <= 1)
        {
          goto invokeMethod;
        }
      }
      methods = methods.Where(b => b.GetParameters().Length == parameters.Length);
      if (methods.Count() <= 1)
      {
        goto invokeMethod;
      }
      var pType = parameters.Select(b => b != null ? Type.GetTypeCode(b.GetType()) : Type.GetTypeCode(null)).ToArray();
      methods = methods.Where(b =>
      {
        var mType = b.GetParameters();
        if (mType.Length != pType.Length)
        {
          return false;
        }
        for (var i = 0; i < pType.Length; i++)
        {
          var iPtype = pType[i];
          if (iPtype == TypeCode.Empty)
          {
            continue;
          }
          var mTypeCode = Type.GetTypeCode(mType[i].ParameterType);
          if (mTypeCode != iPtype)
          {
            return false;
          }
        }
        return true;
      });

    invokeMethod:
      var m = methods.FirstOrDefault();
      if (m == null)
      {
        return result;
      }
      try
      {
        if (m.ReturnType != null && m.ReturnType.Name != "Void")
        {
          return m.Invoke(obj, parameters);
        }
        m.Invoke(obj, parameters);
        return null;
      }
      catch (Exception ex)
      {
        ex.ThrowStatusException();
        return result;
      }
    }
    public static async Task<ContentResponse<T>> InvokeAsync<T>(this object input, string name, params object[] parameters)
    {
      return await input.GetType().InvokeAsync<T>(name, input, parameters);
    }
    public static async Task<ContentResponse<T>> InvokeAsync<T>(this Type input, string name, object obj, params object[] parameters)
    {
      var result = new ContentResponse<T>();
      try
      {
        var task = input.Invoke<Task>(name, obj, parameters);
        await Task.WhenAll(new Task[] { task.Content });
        result.SetSuccess<T>((T)(task.Content as dynamic).Result);
      }
      catch (Exception ex)
      {
        ex.ThrowStatusException();
        result.SetError(ex);
      }
      return result;

    }

    public static bool IsProxy(this Type type)
    {
      var baseType = type.BaseType;
      if (baseType == null)
      {
        return false;
      }
      return !Common.Proxies.Any(b => b == baseType.Namespace) && Common.Proxies.Any(b => b == type.Namespace);
    }
    public static bool IsProxy(this PropertyInfo property)
    {
      return property.PropertyType.IsProxy();
    }

    public static Type ThisType(this Type type)
    {
      if (type.IsProxy())
      {
        return type.BaseType;
      }
      return type;
    }

    public static Type ThisType(this object type)
    {
      return type.GetType().ThisType();
    }

    public static ContentResponse<T> GetPropertyValue<T>(this object obj, string key)
    {
      var result = new ContentResponse<T>();

      var p = obj.ThisType().GetMap().Properties.FirstOrDefault(b =>
        String.Equals(b.Name, key, StringComparison.OrdinalIgnoreCase));
      if (p == null)
      {
        return result;
      }
      var content = p.GetValue(obj);
      try
      {
        result.SetSuccess((T)content);
      }
      catch (Exception ex)
      {
        ex.ThrowStatusException();
      }
      return result;
    }
    public static ContentResponse<object> GetPropertyValue(this object obj, string key)
    {
      var result = new ContentResponse<object>();

      var p = obj.ThisType().GetMap().Properties.FirstOrDefault(b =>
        String.Equals(b.Name, key, StringComparison.OrdinalIgnoreCase));
      if (p == null)
      {
        return result;
      }
      var content = p.GetValue(obj);
      try
      {
        result.SetSuccess(content);
      }
      catch (Exception ex)
      {
        ex.ThrowStatusException();
      }
      return result;
    }
  }
}

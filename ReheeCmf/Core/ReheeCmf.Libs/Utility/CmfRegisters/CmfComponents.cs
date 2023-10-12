using ReheeCmf.Components;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReheeCmf.Utility.CmfRegisters
{
  public static partial class CmfRegister
  {
    public static ConcurrentDictionary<int, ICmfComponent> ComponentPool =
      new ConcurrentDictionary<int, ICmfComponent>();
    public static ConcurrentDictionary<int, ICmfHandler> SingletonHandlerPool =
      new ConcurrentDictionary<int, ICmfHandler>();
    public static ConcurrentDictionary<Type, IRegistrableEntity> RegistrableEntityPool =
      new ConcurrentDictionary<Type, IRegistrableEntity>();

    public static void RegisterComponent(Attribute attribute, Type decorate)
    {
      if (attribute is ICmfComponent != true)
      {
        return;
      }
      var component = attribute as ICmfComponent;
      if (component is IHandlerComponent)
      {
        component.HandlerType = decorate;
      }
      if (component is IEntityComponent)
      {
        component.EntityType = decorate;
      }
      ComponentPool.TryAdd(component!.GetHashCode(), component);

    }

    public static void RegisterEntityComponent(Attribute attribute, Type decorate)
    {
      if (attribute is not RegistrableEntityAttribute)
      {
        return;
      }
      var handler = Activator.CreateInstance(decorate);
      if (handler is IRegistrableEntity typedHandler)
      {
        RegistrableEntityPool.AddOrUpdate(decorate, typedHandler, (a, b) => typedHandler);
      }

    }

    public const string ControllerName = "Controller";
    public const string ControllerTypeName = "Microsoft.AspNetCore.Mvc.ControllerBase";
    public const string ControllerAssembly = "Microsoft.AspNetCore.Mvc.Core";
    private static bool? HasControllerType = null;
    private static Type? controllerType = null;
    public static Type? ControllerType
    {
      get
      {
        if (HasControllerType == null)
        {
          controllerType = Type.GetType($"{ControllerTypeName}, {ControllerAssembly}");
          HasControllerType = controllerType != null;
        }
        return controllerType;
      }
    }
    public static ConcurrentDictionary<string, Type> ControllerPool = new ConcurrentDictionary<string, Type>();
    public static void RegistController(Type? type)
    {
      if (type == null || ControllerType == null)
      {
        return;
      }
      if (type.IsInheritance(ControllerType))
      {
        ControllerPool.TryAdd(type.Name.ToUpper(), type);
      }
    }
    public static bool TryGetController(string? name, out Type? controller)
    {
      if (string.IsNullOrEmpty(name))
      {
        controller = null;
        return false;
      }
      var nomolizeName1 = $"{name}{ControllerName}".ToUpper();
      if (ControllerPool.TryGetValue(nomolizeName1, out controller))
      {
        return true;
      }
      var nomolizeName2 = name.ToUpper();
      if (ControllerPool.TryGetValue(nomolizeName2, out controller))
      {
        return true;
      }
      controller = null;
      return false;
    }
  }
}

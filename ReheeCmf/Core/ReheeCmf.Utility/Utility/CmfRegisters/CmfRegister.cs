using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ReheeCmf.Utility.CmfRegisters
{
  public static partial class CmfRegister
  {
    private static Action<Attribute, Type>[] Registers { get; set; } = new Action<Attribute, Type>[]
    {
      RegisterComponent
    };
    public static void SetRegisters(IEnumerable<Action<Attribute, Type>> registers)
    {
      Registers = registers.ToArray();
    }
    private static void RegisterAll(this IEnumerable<Attribute> attributes, Type decorate)
    {
      foreach (var attribute in attributes.Where(b => b is IRegistrableAttribute))
      {
        foreach (var r in Registers)
        {
          r(attribute, decorate);
        }
      }
    }
    public static void Init()
    {
      AppDomain currentDomain = AppDomain.CurrentDomain;
      Assembly[] assemblies = currentDomain.GetAssemblies();
      foreach (var assembly in assemblies)
      {
        foreach (var type in assembly.GetTypes())
        {
          if (type.CustomAttributes.Any(b => b.AttributeType.IsImplement<IRegistrableAttribute>()))
          {

            Attribute.GetCustomAttributes(type).RegisterAll(type);
          }
          CmfRegister.RegistController(type);
          foreach (var property in type.GetProperties())
          {

          }
          foreach (var field in type.GetFields())
          {

          }
        }
      }
    }
  }
}

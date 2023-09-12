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
    private static Action<Attribute>[] Registers { get; set; } = new Action<Attribute>[]
    {
      RegisterComponent
    };
    public static void SetRegisters(IEnumerable<Action<Attribute>> registers)
    {
      Registers = registers.ToArray();
    }
    private static void RegisterAll(this IEnumerable<Attribute> attributes)
    {
      foreach (var attribute in attributes.Where(b => b is IRegistrableAttribute))
      {
        foreach (var r in Registers)
        {
          r(attribute);
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
            Attribute.GetCustomAttributes(type).RegisterAll();
          }
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

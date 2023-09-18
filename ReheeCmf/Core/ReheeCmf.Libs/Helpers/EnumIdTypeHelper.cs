using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReheeCmf.Helpers
{
  public static class EnumIdTypeHelper
  {
    public static Dictionary<Type, EnumIdType> IdTypeMapper { get; set; } = new Dictionary<Type, EnumIdType>()
    {
      [typeof(Guid)] = EnumIdType.Guid,
      [typeof(Guid?)] = EnumIdType.Guid,
      [typeof(string)] = EnumIdType.String,
      [typeof(Int32?)] = EnumIdType.Number,
      [typeof(long?)] = EnumIdType.Number,
      [typeof(Int32)] = EnumIdType.Number,
      [typeof(long)] = EnumIdType.Number,
    };
    public static EnumIdType GetIdType<T>()
    {
      if (IdTypeMapper.TryGetValue(typeof(T), out var enu))
      {
        return enu;
      }
      return EnumIdType.NotSpecified;
    }
  }
}

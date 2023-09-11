using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReheeCmf.ConstValues
{
  public static class ConstCrud
  {
    public const string Create = "create";
    public const string Read = "read";
    public const string Update = "update";
    public const string Delete = "delete";
    public const string Split = ":";
    public static string GetEntityPermission(this EnumHttpMethod method, string entityName)
    {
      string option = "";
      switch (method)
      {
        case EnumHttpMethod.Get:
          option = Read;
          break;
        case EnumHttpMethod.Put:
          option = Update;
          break;
        case EnumHttpMethod.Post:
          option = Create;
          break;
        case EnumHttpMethod.Delete:
          option = Delete;
          break;
      }
      return $"{entityName}{Split}{option}";
    }
  }
}

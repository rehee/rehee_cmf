using Microsoft.AspNetCore.OData.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ReheeCmf.Helpers
{
  public static class ODataResultHelper
  {
    private static MethodInfo? _reateSingleResult { get; set; }
    private static MethodInfo CreateSingleResult
    {
      get
      {
        if (_reateSingleResult == null)
        {
          _reateSingleResult = typeof(SingleResult).GetMap().Methods.Where(b => b.Name == "Create").FirstOrDefault();
        }
        return _reateSingleResult!;
      }
    }
    public static object GetSingleResult(Type entityType, object input)
    {
      return CreateSingleResult.MakeGenericMethod(entityType).Invoke(null, new object[] { input })!;
    }
  }
}

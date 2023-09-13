using ReheeCmf.Utility.CmfRegisters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReheeCmf.ODatas.Components
{
  public static class ODataEntitySetFactory
  {
    public static IODataEntitySetHandler? GetHandler(Type entityType)
    {
      var allComponent = CmfRegister.ComponentPool.Values.ToArray();
      return CmfRegister.ComponentPool.Where(b =>
      {
        if (b.Value is IODataEntitySet s)
        {
          return s.EntityType == entityType;
        }
        return false;
      }).OrderByDescending(b => b.Value.Index).ThenByDescending(b => b.Value.SubIndex)
      .Select(b =>
      {
        if (b.Value is IODataEntitySet s)
        {
          return s.GetHandler();
        }
        return null;
      }).FirstOrDefault();
    }
  }
}

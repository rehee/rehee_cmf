using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReheeCmf.MultiTenants
{
  public class TenantConnection
  {
    public Dictionary<string, TenantConnectionItem?>? Items { get; set; }
  }

  public class TenantConnectionItem
  {
    public string? ReadAndWrite { get; set; }
    public string? ReadOnly { get; set; }
    private string[]? _ReadOnlyList { get; set; }
    public string[] ReadOnlyList
    {
      get
      {
        if (_ReadOnlyList == null)
        {
          _ReadOnlyList = ReadOnly?.Split(",").Select(b => b.Trim()).Distinct().ToArray() ?? Array.Empty<string>();
        }
        return _ReadOnlyList;
      }
    }
  }
}

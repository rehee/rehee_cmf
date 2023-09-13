using ReheeCmf.StandardInputs.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReheeCmf.StandardInputs.StandardItems
{
  public class StandardItem : IStandard
  {
    public StandardItem()
    {
      FullTypeName = "";
      Assembly = "";
    }
    public StandardItem(string fullTypeName, string assembly)
    {
      FullTypeName = fullTypeName;
      Assembly = assembly;
    }
    public string FullTypeName { get; set; }
    public string Assembly { get; set; }
    public IEnumerable<StandardProperty>? Properties { get; set; }
  }
}

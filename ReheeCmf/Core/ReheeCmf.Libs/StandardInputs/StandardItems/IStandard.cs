using ReheeCmf.StandardInputs.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReheeCmf.StandardInputs.StandardItems
{
  public interface IStandard
  {
    string FullTypeName { get; set; }
    string Assembly { get; set; }
  }
  public interface IStandardItem : IStandard
  {
    IEnumerable<StandardProperty>? Properties { get; set; }
  }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReheeCmf.ODatas.Attributes
{
  public class OdataSetAttribute : Attribute
  {
    public OdataSetAttribute(string dataSet)
    {
      DataSet = dataSet;
    }

    public string DataSet { get; }
  }
}

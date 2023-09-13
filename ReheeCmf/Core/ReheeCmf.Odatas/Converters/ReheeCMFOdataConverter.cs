using Microsoft.OData.Edm;
using Microsoft.OData;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ReheeCmf.ODatas.Converters
{
  public class ReheeCMFOdataConverter : ODataPayloadValueConverter
  {
    public override object ConvertFromPayloadValue(object value, IEdmTypeReference edmTypeReference)
    {
      return base.ConvertFromPayloadValue(value, edmTypeReference);
    }
    public override object ConvertToPayloadValue(object value, IEdmTypeReference edmTypeReference)
    {
      if (value is DateTime date)
      {
        var utc = DateTime.SpecifyKind(date, DateTimeKind.Utc);
        utc.ToString(Common.DATETIMEUTC, CultureInfo.InvariantCulture);
      }
      if (value is DateTimeOffset dateSet)
      {
        return dateSet.ToString(Common.DATETIMEUTC, CultureInfo.InvariantCulture);
      }
      return base.ConvertToPayloadValue(value, edmTypeReference);
    }
  }
}

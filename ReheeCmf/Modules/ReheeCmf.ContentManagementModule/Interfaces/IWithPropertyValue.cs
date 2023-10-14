using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReheeCmf.ContentManagementModule.Interfaces
{
  public interface IWithPropertyValue
  {
    string? Value { get; set; }
    String? ValueString { get; set; }
    Boolean? ValueBoolean { get; set; }
    Int16? ValueInt16 { get; set; }
    Int32? ValueInt32 { get; set; }
    Int64? ValueInt64 { get; set; }
    Single? ValueSingle { get; set; }
    Double? ValueDouble { get; set; }
    Decimal? ValueDecimal { get; set; }
    Guid? ValueGuid { get; set; }
    DateTime? ValueDateTime { get; set; }
    DateTimeOffset? ValueDateTimeOffset { get; set; }
  }
}

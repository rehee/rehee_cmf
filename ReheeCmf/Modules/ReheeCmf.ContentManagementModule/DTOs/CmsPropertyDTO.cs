using ReheeCmf.ContentManagementModule.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReheeCmf.ContentManagementModule.DTOs
{
  public class CmsPropertyDTO : IWithPropertyValue
  {
    public string? PropertyName { get; set; }
    public string? Value { get; set; }
    public string? ValueString { get; set; }
    public bool? ValueBoolean { get; set; }
    public short? ValueInt16 { get; set; }
    public int? ValueInt32 { get; set; }
    public long? ValueInt64 { get; set; }
    public float? ValueSingle { get; set; }
    public double? ValueDouble { get; set; }
    public decimal? ValueDecimal { get; set; }
    public Guid? ValueGuid { get; set; }
    public DateTime? ValueDateTime { get; set; }
    public DateTimeOffset? ValueDateTimeOffset { get; set; }
  }
}

using ReheeCmf.ContentManagementModule.Interfaces;
using ReheeCmf.Entities;
using ReheeCmf.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReheeCmf.ContentManagementModule.Entities
{
  public class CmsProperty : CmsBase, IWithPropertyValue
  {

    public Guid? CmsEntityId { get; set; }
    public virtual CmsEntity? Entity { get; set; }

    public Guid? CmsPropertyMetadataId { get; set; }
    public virtual CmsPropertyMetadata? Property { get; set; }

    public String? PropertyName { get; set; }
    public EnumPropertyType? PropertyType { get; set; }

    [NotMapped]
    public string? Value { get; set; }
    public String? ValueString { get; set; }
    public Boolean? ValueBoolean { get; set; }
    public Int16? ValueInt16 { get; set; }
    public Int32? ValueInt32 { get; set; }
    public Int64? ValueInt64 { get; set; }
    public Single? ValueSingle { get; set; }
    public Double? ValueDouble { get; set; }
    public Decimal? ValueDecimal { get; set; }
    public Guid? ValueGuid { get; set; }
    public DateTime? ValueDateTime { get; set; }
    public DateTimeOffset? ValueDateTimeOffset { get; set; }
  }
}

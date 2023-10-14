using ReheeCmf.ContentManagementModule.Interfaces;
using ReheeCmf.Entities;
using ReheeCmf.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReheeCmf.ContentManagementModule.Entities
{
  public class CmsPropertyMetadata : CmsBase, IWithCmsPermission, IWithCmsRule
  {

    public Guid? CmsEntityMetadataId { get; set; }
    public virtual CmsEntityMetadata? Metadata { get; set; }
    public virtual List<CmsProperty>? Properties { get; set; }
    [Required]
    public string? PropertyName { get; set; }
    public string? PropertyNameNormalization { get; set; }
    public EnumPropertyType PropertyType { get; set; }
    public EnumInputType InputType { get; set; }
    public bool? NotNull { get; set; }
    public bool? Unique { get; set; }
    public string? PermissionCreate { get; set; }
    public string? PermissionRead { get; set; }
    public string? PermissionUpdate { get; set; }
    public string? PermissionDelete { get; set; }
    public string? RuleCreate { get; set; }
    public string? RuleRead { get; set; }
    public string? RuleUpdate { get; set; }
    public string? RuleDelete { get; set; }

  }
}

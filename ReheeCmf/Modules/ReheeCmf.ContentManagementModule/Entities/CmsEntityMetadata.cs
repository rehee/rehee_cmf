using ReheeCmf.ContentManagementModule.Interfaces;
using ReheeCmf.Entities;
using ReheeCmf.Enums;
using System.ComponentModel.DataAnnotations;

namespace ReheeCmf.ContentManagementModule.Entities
{
  public class CmsEntityMetadata : CmsBase, IWithCmsPermission, IWithCmsRule, IWithCmsQuery
  {
    [Required]
    public string? EntityName { get; set; }
    public string? EntityNameNormalization { get; set; }
    public virtual List<CmsPropertyMetadata>? Properities { get; set; }
    public virtual List<CmsEntity>? Entities { get; set; }
    public string? PermissionCreate { get; set; }
    public string? PermissionRead { get; set; }
    public string? PermissionUpdate { get; set; }
    public string? PermissionDelete { get; set; }
    public string? RuleCreate { get; set; }
    public string? RuleRead { get; set; }
    public string? RuleUpdate { get; set; }
    public string? RuleDelete { get; set; }
    public bool? AsSplitQuery { get; set; }
    public string? SelectedProperties { get; set; }
    public bool? QueryBeforeFilter { get; set; }
    public bool? HideProperty { get; set; }
    
  }
}

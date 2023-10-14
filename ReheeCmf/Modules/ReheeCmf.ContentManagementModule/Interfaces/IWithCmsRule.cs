using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReheeCmf.ContentManagementModule.Interfaces
{
  public interface IWithCmsRule
  {
    string? RuleCreate { get; set; }
    string? RuleRead { get; set; }
    string? RuleUpdate { get; set; }
    string? RuleDelete { get; set; }
  }
}

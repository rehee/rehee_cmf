using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReheeCmf.Tenants
{
  public class TenantSetting
  {
    public bool EnableTenant { get; set; }
    public TenantEntity[] Tenants { get; set; }
    public bool ForceTenant { get; set; }
    public bool CustomService { get; set; }
    public bool TenentContext { get; set; }
  }
}

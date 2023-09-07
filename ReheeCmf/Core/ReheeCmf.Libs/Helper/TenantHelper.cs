using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReheeCmf.Helper
{
  public static class TenantHelper
  {
    public static Tenant GetTenant(this TenantEntity entity, FileServiceOption fileServerOptions)
    {
      return new Tenant
      {
        TenantID = entity.Id,
        TenantName = entity.TenantName,
        TenantSUbDomain = entity.TenantSubDomain,
        TenantUrl = entity.TenantSubDomain,
        TenantLevel = entity.TenantLevel,
        MainConnectionString = entity.MainConnectionString,
        ReadConnectionStrings = entity.ReadConnectionStringArray,
        FileOption = FileServiceOption.New(entity, fileServerOptions)
      };
    }
  }
}

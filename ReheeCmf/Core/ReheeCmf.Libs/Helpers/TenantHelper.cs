namespace ReheeCmf.Helpers
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

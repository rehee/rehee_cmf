using EFCoreSecondLevelCacheInterceptor;
using Microsoft.EntityFrameworkCore.Diagnostics;
using ReheeCmf.Caches;
using ReheeCmf.Commons;
using ReheeCmf.ContextModule.Readers;
using ReheeCmf.Tenants;
using System.Data.Common;
using System.Text.Json;

namespace ReheeCmf.ContextModule.Interceptors
{
  public class CmfDbCommandInterceptor : DbCommandInterceptor, ICmfDbCommandInterceptor
  {
    private readonly IContextScope<QuerySecondCache> queryCache;
    private readonly IKeyValueCaches<ICmfDbCommandInterceptor> mc;
    private readonly IContextScope<Tenant> tenant;

    public CmfDbCommandInterceptor(IContextScope<Tenant> tenant, IContextScope<QuerySecondCache> queryCache, IKeyValueCaches<ICmfDbCommandInterceptor> mc)
    {
      this.tenant = tenant;
      this.queryCache = queryCache;
      this.mc = mc;
    }

    public override DbDataReader ReaderExecuted(DbCommand command, CommandExecutedEventData eventData, DbDataReader result)
    {
      if (queryCache?.Value?.EnableCache == true)
      {
        string tenant_key = "";
        if (tenant.Value?.TenantID.HasValue == true)
        {
          tenant_key = tenant.Value?.TenantID.ToString() + (tenant.Value?.IgnoreTenant == true ? "_Ignore" : "");
        }
        var key = $"{tenant_key}_{command.CommandText}_{command.Connection?.ConnectionString ?? ""}";
        if (mc.TryGetValue<string>(key, out var value))
        {
          if (value != null && value is string json)
          {
            if (!String.IsNullOrEmpty(json))
            {
              try
              {
                return new EFTableRowsDataReader(JsonSerializer.Deserialize<EFTableRows>(json)!);
              }
              catch { }
            }

          }
        }
        var res = base.ReaderExecuted(command, eventData, result);
        EFTableRows tableRows;
        using (var dbReaderLoader = new EFDataReaderLoader(res))
        {
          tableRows = dbReaderLoader.LoadAndClose();
        }

        var jsonResponse = JsonSerializer.Serialize(tableRows);
        //var obj = JsonConvert.DeserializeObject<EFTableRows>(json);
        mc.Set(key, jsonResponse, 0.05d);
        return new EFTableRowsDataReader(tableRows);
      }
      else
      {
        return base.ReaderExecuted(command, eventData, result);
      }

    }
  }
}

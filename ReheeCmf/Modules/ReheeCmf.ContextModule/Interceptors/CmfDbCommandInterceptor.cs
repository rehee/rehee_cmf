using EFCoreSecondLevelCacheInterceptor;
using Microsoft.EntityFrameworkCore.Diagnostics;
using ReheeCmf.Caches;
using ReheeCmf.Commons;
using ReheeCmf.Commons.Jsons.Options;
using ReheeCmf.ContextModule.Readers;
using ReheeCmf.Tenants;
using System.Data.Common;
using System.Security.Cryptography;
using System.Text;
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
        var sqlQuery = command.CommandText;

        // 替换参数值为实际的值
        foreach (DbParameter parameter in command.Parameters)
        {
          var parameterName = parameter.ParameterName;
          var parameterValue = parameter.Value.ToString(); // 将参数值转换为字符串

          sqlQuery = sqlQuery.Replace(parameterName, parameterValue);
        }
        var hash = ComputeHash(sqlQuery);
        var key = $"{hash}_{command.Connection?.ConnectionString ?? ""}";
        if (mc.TryGetValue<string>(key, out var value))
        {
          if (value != null && value is string json)
          {
            if (!String.IsNullOrEmpty(json))
            {
              try
              {
                //return new EFTableRowsDataReader(JsonSerializer.Deserialize<EFTableRows>(json, JsonOption.DefaultOption)!);
                return new EFTableRowsDataReader(Newtonsoft.Json.JsonConvert.DeserializeObject<EFTableRows>(json)!);
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

        //var jsonResponse = JsonSerializer.Serialize(tableRows, JsonOption.DefaultOption);
        var jsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(tableRows);
        mc.Set(key, jsonResponse, 0.0833d);
        return new EFTableRowsDataReader(tableRows);
      }
      else
      {
        return base.ReaderExecuted(command, eventData, result);
      }

    }
    private static string ComputeHash(string input)
    {
      using (var sha256 = SHA256.Create())
      {
        var bytes = Encoding.UTF8.GetBytes(input);
        var hashBytes = sha256.ComputeHash(bytes);
        return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
      }
    }
  }
}

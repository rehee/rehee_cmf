using ReheeCmf.Commons.Jsons.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ReheeCmf.Requests
{
  public abstract class RequestBase : IRequestBase
  {
    protected string baseToken { get; set; }
    public void SetBaseToken(string token)
    {
      baseToken = token;
    }
    protected string clientUrl { get; set; }
    public void SetClientUrl(string url)
    {
      clientUrl = url;
    }
    protected Guid? TenantId { get; set; }
    public void SetTenant(Guid? tenantId)
    {
      TenantId = tenantId;
    }
    protected string overrideBaseUrl { get; set; }
    public void OverrideBaseUrl(string url)
    {
      overrideBaseUrl = url;
    }
    public virtual bool FlagOnly => true;
    public const string MultiFormJsonKey = "json";
    protected readonly IContextScope<Tenant> contextScopeTenant;
    protected Guid? tenantID => contextScopeTenant?.Value?.TenantID ?? TenantId;
    public RequestBase(IContextScope<Tenant> contextScopeTenant)
    {
      this.contextScopeTenant = contextScopeTenant;
    }
    protected abstract HttpClient GetHttpClient(string? name = default);
    protected string SerializeObject(object input)
    {
      return JsonSerializer.Serialize(input, JsonOption.DefaultOption);
    }
    public async Task<ContentResponse<T>> Request<T>
      (HttpMethod method, string url, string? json = null, Stream? content = null, Dictionary<string, string>? headValue = null,
      string? name = null, string? token = null, CancellationToken ct = default)
    {
      var result = new ContentResponse<T>();
      using var client = GetHttpClient(name);

      var response = await this.RequestString(client, method, url, json, content, headValue, token, ct);
      if (response.Success)
      {
        try
        {
          if (response.Content != null)
          {
            if (response.Content is T resultValue)
            {
              result.SetSuccess(resultValue);
            }
            else
            {
              result.SetSuccess(JsonSerializer.Deserialize<T>(response.Content, JsonOption.DefaultOption));
            }
          }
          else
          {
            result.SetSuccess(default(T));
          }
        }
        catch (Exception ex)
        {
          ex.ThrowStatusException();
        }
      }
      result.Status = response.Status;
      return result;
    }

    private async Task<ContentResponse<string>> RequestString(
      HttpClient client, HttpMethod method, string url, string? json = null, Stream? content = null,
      Dictionary<string, string>? headValue = null,
      string? token = null, CancellationToken ct = default)
    {
      var result = new ContentResponse<string>();
      var requestResponse = getRequestMessage(method, url, json, content);
      if (!requestResponse.Success)
      {
        return result;
      }
      var request = requestResponse.Content!;
      if (!String.IsNullOrEmpty(token))
      {
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
      }
      else
      {
        if (!String.IsNullOrEmpty(baseToken))
        {
          request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", baseToken);
        }
      }
      var useTenantId = true;
      if (headValue != null)
      {
        foreach (var v in headValue)
        {
          if (v.Key == Common.TenantIDHeader)
          {
            useTenantId = false;
          }
          request.Headers.Add(v.Key, v.Value);
        }
      }
      if (!string.IsNullOrEmpty(clientUrl))
      {
        request.Headers.Add(Common.ClientUrlHeader, clientUrl);
      }
      if (useTenantId && tenantID.HasValue)
      {
        request.Headers.Add(Common.TenantIDHeader, tenantID.Value.ToString());
      }
      if (!String.IsNullOrEmpty(overrideBaseUrl))
      {
        client.BaseAddress = new Uri(overrideBaseUrl);
      }
      try
      {
        var response = await client.SendAsync(request, ct);
        result.Success = response.IsSuccessStatusCode;
        result.Status = response.StatusCode;
        if (response.Content != null)
        {
          try
          {
            result.Content = await response.Content.ReadAsStringAsync();
          }
          catch (Exception ex)
          {
            ex.ThrowStatusException();
          }
        }
      }
      catch (Exception ex)
      {
        ex.ThrowStatusException();
      }
      return result;
    }
    private ContentResponse<HttpRequestMessage> getRequestMessage(
      HttpMethod method, string url, string json = null, Stream content = null)
    {
      var result = new ContentResponse<HttpRequestMessage>();
      try
      {

        HttpRequestMessage request;
        if (json != null && content != null)
        {
          var multiContent = new MultipartFormDataContent();
          multiContent.Add(new StreamContent(content), "file", "fileName");
          multiContent.Add(new StringContent(json, Encoding.UTF8, "application/json"), MultiFormJsonKey);
          request = new HttpRequestMessage(method, url)
          {
            Content = multiContent
          };
          result.SetSuccess(request);
        }
        else
        {
          request = content != null ?
      new HttpRequestMessage(method, url)
      {
        Content = new StreamContent(content as dynamic)

      } : !String.IsNullOrWhiteSpace(json) ?
      new HttpRequestMessage(method, url)
      {
        Content = new StringContent(json, Encoding.UTF8, "application/json")
      } :
      new HttpRequestMessage(method, url)
      ;
          result.SetSuccess(request);
        }
      }
      catch (Exception ex)
      {
        ex.ThrowStatusException();
      }
      return result;
    }
  }
}

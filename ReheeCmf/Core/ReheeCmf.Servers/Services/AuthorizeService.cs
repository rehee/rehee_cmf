using Microsoft.IdentityModel.Tokens;
using ReheeCmf.Authenticates;
using ReheeCmf.Commons.Consts;
using ReheeCmf.Commons.Jsons.Options;
using ReheeCmf.Enums;
using ReheeCmf.Requests;
using ReheeCmf.Responses;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ReheeCmf.Servers.Services
{
  public class AuthorizeService : IAuthorize
  {
    public bool FlagOnly => true;

    protected readonly IJWTService jwt;
    protected readonly AuthorizeOption option;
    protected readonly TokenManagement management;
    protected readonly IRequestClient<IAuthorize>? request;
    protected readonly TokenValidationParameters validationParameter;
    protected readonly ApiSetting apisetting;
    public AuthorizeService(IJWTService jwt, AuthorizeOption option, TokenManagement management, IRequestClient<IAuthorize>? request, TokenValidationParameters validationParameter, ApiSetting apisetting)
    {
      this.jwt = jwt;
      this.option = option;
      this.management = management;
      this.request = request;
      this.validationParameter = validationParameter;
      this.apisetting = apisetting;
    }

    public async Task<bool> EnableAuth()
    {
      switch (option.AuthorizeType)
      {
        case EnumAuthorizeType.RemoteManagement:
          var remoteResult = await request.Request<bool>(HttpMethod.Get, "Api/Token/EnableAuth");
          return remoteResult.Success ? remoteResult.Content : true;
        case EnumAuthorizeType.RemoteLoginSelfAuth:
          return management.EnableAuth;
        default:
          return jwt.EnableAuth();
      }
    }

    public async Task<string> FullAccessRole()
    {
      switch (option.AuthorizeType)
      {

        case EnumAuthorizeType.RemoteManagement:
          var remoteResult = await request.Request<string>(HttpMethod.Get, "Api/Token/FullAccess");
          return remoteResult.Success ? remoteResult.Content : "";
        case EnumAuthorizeType.RemoteLoginSelfAuth:
          return management.FullAccessRole;
        default:
          return jwt.FullAccessRole;
      }
    }

    public async Task<ContentResponse<TokenDTO>> ValidateAndConvert(string token)
    {
      switch (option.AuthorizeType)
      {
        case EnumAuthorizeType.RemoteManagement:
          return await request.Request<TokenDTO>(HttpMethod.Post, "Api/Token/ValidateToken",
            JsonSerializer.Serialize(new TokenValidate()
            {
              Token = token
            }, JsonOption.DefaultOption));
        case EnumAuthorizeType.RemoteLoginSelfAuth:
          return await SelfValidation(token);
        default:
          return await jwt.ValidateAndConvertToken(token, option.CheckUserEveryRequest);
      }
    }
    public Task<string> ApiSystemToken()
    {
      //var key = await request.Request<string>(HttpMethod.Get, "Api/Token/Request");
      //var token = EncryptionCheck.GetToken(key.Content.Decrypt(apisetting.EncryptionKey));
      //var jwtToken = await request.Request(HttpMethod.Post, "Api/Token/Request",
      //  JsonConvert.SerializeObject(new TokenValidate() { Token = token }));
      //return jwtToken.Content;
      //TODO: Need implenent get api system token
      return Task.FromResult(jwt.GetSystemApiToken());
    }

    private Task<ContentResponse<TokenDTO>> SelfValidation(string token)
    {
      var result = new ContentResponse<TokenDTO>();
      try
      {
        var handler = new JwtSecurityTokenHandler();
        var tokenExceptHeader = token.Split(' ').LastOrDefault();
        var claim = handler.ValidateToken(tokenExceptHeader, validationParameter, out var token2);
        var userName = claim.Identity?.Name;
        Func<string, string[]> getFromClain =
          (key) =>
          {
            var stringValue = claim.Claims.Where(b => b.Type.IndexOf(key) >= 0);
            return stringValue.Select(b => b.Value ?? "").Distinct().ToArray();
          };
        var b = claim.Claims.Where(b => b.Type == "exp")

           .Select(b =>
           {
             if (long.TryParse(b.Value, out long r))
             {
               return r;
             }
             return (long)0;
           }).ToArray().FirstOrDefault();
        var expdate = DateTimeOffset.FromUnixTimeSeconds(b).UtcDateTime;
        var expTimeSpend = (expdate - DateTime.UtcNow);
        var guidAvaliable = Guid.TryParse(getFromClain(Common.TenantIDHeader).FirstOrDefault() ?? "", out var tId);
        var dto = new TokenDTO()
        {
          UserName = claim.Identity.Name,
          UserId = getFromClain(ConstOptions.ClaimNameType).FirstOrDefault(),
          Permissions = getFromClain(ConstOptions.PermissionType).FirstOrDefault()?.Split(","),
          Avatar = getFromClain(ConstOptions.AvatarType).FirstOrDefault(),
          ExpireUCTTime = expdate,
          Roles = getFromClain(ConstOptions.RoleType),
          TokenString = token,
          ExpireSecond = (ulong)expTimeSpend.TotalSeconds,
          UserEmail = getFromClain(ConstOptions.UserEmail).FirstOrDefault(),
          IsSystemToken = claim.Claims.Any(b => b.Type == Common.SystemApiClaimType),
          TenantID = guidAvaliable ? tId : null,
        };
        result.SetSuccess(dto);
      }
      catch { }
      return Task.FromResult(result);
    }
  }

}

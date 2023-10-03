using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using ReheeCmf.Authenticates;
using ReheeCmf.Commons.Consts;
using ReheeCmf.Commons.DTOs;
using ReheeCmf.Commons;
using ReheeCmf.Contexts;
using ReheeCmf.Entities;
using ReheeCmf.Modules.Options;
using ReheeCmf.Tenants;
using System.Collections.Concurrent;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using ReheeCmf.Services;
using ReheeCmf.Responses;
using ReheeCmf.Modules;

namespace ReheeCmf.AuthenticationModule.Services
{
  public class JWTService<TUser> : IJWTService, IJWTService<TUser>
    where TUser : IdentityUser, ICmfUser, new()
  {
    private static ConcurrentDictionary<string, IEnumerable<string>> rolePermissionCache = new ConcurrentDictionary<string, IEnumerable<string>>();
    private readonly IServiceProvider sp;
    private readonly UserManager<TUser> userManager;
    private readonly SignInManager<TUser> signInManager;
    private readonly TokenManagement tokenManagement;
    private readonly TokenValidationParameters validationParameter;
    private readonly IHttpContextAccessor httpContext;
    private readonly IUserTokenStorage userTokenStorage;
    private readonly IUserLockendStorage userLockendStorage;
    private readonly IAdditionalTokenService<TUser>? additionalTokenService;
    protected readonly IContextScope<Tenant>? tenantDetail;
    private CrudOption options { get; set; }
    public JWTService(
      IServiceProvider sp, UserManager<TUser> u, SignInManager<TUser> s, TokenManagement tokenManagement, IHttpContextAccessor httpContext, CrudOption options, IUserTokenStorage userTokenStorage, IUserLockendStorage userLockendStorage)
    {
      this.sp = sp;
      this.userManager = u;
      this.signInManager = s;
      this.tokenManagement = tokenManagement;
      this.httpContext = httpContext;
      this.validationParameter = sp.GetService<TokenValidationParameters>();
      this.options = options;
      this.userTokenStorage = userTokenStorage;
      this.userLockendStorage = userLockendStorage;
      additionalTokenService = sp.GetService<IAdditionalTokenService<TUser>>();
      tenantDetail = sp.GetService<IContextScope<Tenant>>();
    }

    public bool EnableAuth()
    {
      return tokenManagement.EnableAuth;
    }

    public async Task<ContentResponse<string>> GetToken(LoginDTO request)
    {
      var result = new ContentResponse<string>();
      var dto = await GetTokenDTO(request);
      if (dto.Success)
      {
        result.SetSuccess(dto.Content.TokenString);
      }

      return result;
    }
    public async Task<ContentResponse<string>> GetToken(string userName)
    {
      var result = new ContentResponse<string>();
      var mapping = await userTokenStorage.GetUserTokenAsync(userName);
      if (mapping != null)
      {
        result.SetSuccess(mapping.TokenString);
        return result;
      }
      TUser user;
      try
      {
        user = await userManager.FindByNameAsync(userName);
        if (user == null)
        {
          return result;
        }
      }
      catch (Exception ex)
      {
        ex.ThrowStatusException();
        return result;
      }
      var dto = await getTokenDTO(user);
      if (dto.Success)
      {
        result.SetSuccess(dto.Content.TokenString);
      }
      return result;
    }
    public async Task<ContentResponse<TokenDTO>> GetTokenDTO(string userName)
    {
      var result = new ContentResponse<TokenDTO>();
      var mapping = await userTokenStorage.GetUserTokenAsync(userName);
      if (mapping != null)
      {
        result.SetSuccess(mapping);
        return result;
      }
      TUser user;
      try
      {
        user = await userManager.FindByNameAsync(userName);
        if (user == null)
        {
          return result;
        }
      }
      catch (Exception ex)
      {
        ex.ThrowStatusException();
        return result;
      }
      var dto = await getTokenDTO(user);
      if (dto.Success)
      {
        result.SetSuccess(dto.Content);
      }
      return result;
    }
    public async Task<ContentResponse<string>> GetToken()
    {
      var result = new ContentResponse<string>();
      if (httpContext.HttpContext.User.Identity.IsAuthenticated)
      {
        var user = httpContext.HttpContext.User.Identity.Name;
        var mapping = await userTokenStorage.GetUserTokenAsync(user);
        if (mapping != null)
        {
          result.SetSuccess(mapping.TokenString);
          return result;
        }
        return await GetToken(user);
      }
      return result;
    }

    public Task<ContentResponse<string>> GetRefreshToken(string userName)
    {

      return getRefreshToken(userName, false);
    }

    protected Task<ContentResponse<string>> getRefreshToken(string userName, bool impersonate)
    {

      var result = new ContentResponse<string>();
      try
      {
        var claims = new List<Claim>();
        var userClaim = new Claim(ClaimTypes.NameIdentifier, userName);

        claims.Add(userClaim);
        claims.Add(new Claim(Common.TenantIDHeader, this.tenantId?.ToString() ?? ""));
        if (impersonate)
        {
          claims.Add(new Claim(ConstJWT.ImpersonateFlag, "true"));
        }
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenManagement.Secret));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expireTime = DateTime.UtcNow.AddDays(1 + tokenManagement.RefreshTokenExtureDay);
        var jwtToken = new JwtSecurityToken(tokenManagement.Issuer, tokenManagement.Audience, claims,
            expires: expireTime,
            signingCredentials: credentials);

        var token = new JwtSecurityTokenHandler().WriteToken(jwtToken);
        result.SetSuccess(token);
      }
      catch (Exception ex)
      {
        ex.ThrowStatusException();

      }
      return Task.FromResult(result);
    }
    public async Task<ContentResponse<TokenDTO>> RefreshAccessToken(string token)
    {
      var result = new ContentResponse<TokenDTO>();
      if (String.IsNullOrEmpty(token))
      {
        result.SetError(HttpStatusCode.Forbidden);
        return result;
      }
      var validation = await this.ValidateToken(token, false);
      if (!validation.Success || validation.Content == null || validation.Content.Identity?.IsAuthenticated != true)
      {
        result.SetError(HttpStatusCode.Forbidden);
        return result;
      }
      var claims = validation.Content.Claims;
      var userName = claims.FirstOrDefault(b => b.Type == ClaimTypes.NameIdentifier).Value;

      var isImpersonate = validation.Content.Claims.Any(b => b.Type == ConstJWT.ImpersonateFlag && b.Value == "true");
      if (isImpersonate)
      {
        var tenantIdFromToken = claims.FirstOrDefault(b => b.Type == Common.TenantIDHeader)?.Value;
        var context = sp.GetService<IContext>();
        if (context.TenantID?.ToString() != tenantIdFromToken)
        {
          result.SetError(HttpStatusCode.Forbidden);
          return result;
        }
        context.SetIgnoreTenant(true);
        var tIS = sp.GetService<ITenantImpersonateService<TUser>>();
        if (tIS == null)
        {
          throw new StatusException("not implement", HttpStatusCode.NotImplemented);
        }
        var userEntity = await tIS.GetImpersonateUserAsync(userName);
        if (userEntity == null)
        {
          throw new StatusException("Unauthorized", HttpStatusCode.Forbidden);
        }
        return await GetTokenAsync(userEntity, false, true, CancellationToken.None, token);

      }
      else
      {
        var user = await userManager.FindByNameAsync(userName);

        if (user == null)
        {
          result.SetError(HttpStatusCode.Forbidden);
          return result;
        }
        if (await isLockoutAsync(user.UserName))
        {
          result.SetError(HttpStatusCode.Forbidden);
          return result;
        }
        return await getTokenDTO(user, tokenManagement.RefreshTokenExtention, token);
      }

    }

    public string GetSystemApiToken()
    {
      try
      {
        var claims = new Claim[]
        {
          new Claim(Common.SystemApiClaimType,tokenManagement.ServerApiValue??""),
          new Claim(Common.TenantIDHeader,this.tenantId?.ToString()??"")
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenManagement.Secret));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expireTime = DateTime.UtcNow.AddMinutes(tokenManagement.ServerApiExpire > 0 ? tokenManagement.ServerApiExpire : 0.16);
        var jwtToken = new JwtSecurityToken(tokenManagement.Issuer, tokenManagement.Audience, claims,
            expires: expireTime,
            signingCredentials: credentials);
        var token = new JwtSecurityTokenHandler().WriteToken(jwtToken);
        return token;
      }
      catch (Exception ex)
      {
        ex.ThrowStatusException();
      }
      return null;
    }

    public async Task<ContentResponse<ClaimsPrincipal>> ValidateToken(string token, bool checkName = true)
    {
      var result = new ContentResponse<ClaimsPrincipal>();
      if (String.IsNullOrWhiteSpace(token))
      {
        return result;
      }
      var handler = new JwtSecurityTokenHandler();
      try
      {
        var tokenExceptHeader = token.Split(' ').LastOrDefault();
        if (String.IsNullOrEmpty(tokenExceptHeader) || tokenExceptHeader.StartsWith("Bear"))
        {
          return result;
        }
        var claim = handler.ValidateToken(tokenExceptHeader, validationParameter, out var token2);
        var systemApi = claim.Claims.FirstOrDefault(b => b.Type == Common.SystemApiClaimType);
        if (systemApi != null)
        {
          if (systemApi.Value == (tokenManagement.ServerApiValue ?? ""))
          {
            result.SetSuccess(claim);
          }
          return result;

        }
        if (tenantDetail?.Value?.TenantID != null)
        {
          var tenantFromHeader = claim.Claims.FirstOrDefault(b => b.Type == Common.TenantIDHeader);
          if (tenantFromHeader == null || !Guid.TryParse(tenantFromHeader.Value, out var tGuid) || tGuid != tenantDetail?.Value?.TenantID)
          {
            return result;
          }
        }
        if (!checkName)
        {
          result.SetSuccess(claim);
          return result;
        }
        var isValidUser = await CheckUser(claim);
        if (isValidUser)
        {
          result.SetSuccess(claim);
          return result;
        }
      }
      catch (Exception ex)
      {
        ex.ThrowStatusException();
        result.SetError(ex);
      }

      return result;
    }

    private async Task<bool> CheckUser(ClaimsPrincipal? claim)
    {
      if (claim?.Identity?.IsAuthenticated == true)
      {
        if (await isLockoutAsync(claim.Identity.Name))
        {
          return false;
        }
        return true;
      }
      return false;
    }

    public ContentResponse<ClaimsPrincipal> ValidateTokenSync(string token)
    {
      return ValidateToken(token).GetAwaiter().GetResult();
    }
    public async Task<ContentResponse<TokenDTO>> ValidateAndConvertToken(string token, bool checkName = true)
    {
      var result = new ContentResponse<TokenDTO>();
      var tokenCheck = await ValidateToken(token);
      if (!tokenCheck.Success)
      {
        return result;
      }
      var identity = tokenCheck.Content;
      var userName = identity?.Identity?.Name;
      var isImpersonate = identity.Claims.Any(b => b.Type.Equals(ConstJWT.ImpersonateFlag) && b.Value == "true");
      if (checkName == true && isImpersonate != true)
      {
        if (!await CheckUser(identity))
        {
          return result;
        }
      }

      Func<string, string[]> getFromClain =
        (key) =>
        {
          var stringValue = identity.Claims.Where(b => b.Type.IndexOf(key) >= 0);
          return stringValue.Select(b => b.Value ?? "").Distinct().ToArray();
        };
      if (!tokenCheck.Success)
      {
        return result;
      }
      var b = tokenCheck.Content.Claims.Where(b => b.Type == "exp")

        .Select(b =>
        {
          if (long.TryParse(b.Value, out long r))
          {
            return r;
          }
          return (long)0;
        }).ToArray().FirstOrDefault();
      Dictionary<string, string>? additionalClaim = null;
      if (additionalTokenService != null && identity != null && identity.Claims?.Any() == true)
      {
        additionalClaim = await additionalTokenService.GetAdditionalDTOClaimAsync(identity.Claims);
      }
      var expdate = DateTimeOffset.FromUnixTimeSeconds(b).UtcDateTime;
      var expTimeSpend = (expdate - DateTime.UtcNow);
      var guidAvaliable = Guid.TryParse(getFromClain(Common.TenantIDHeader).FirstOrDefault() ?? "", out var tId);
      var dto = new TokenDTO()
      {
        UserName = identity.Identity.Name,
        UserId = getFromClain(ConstOptions.ClaimNameType).FirstOrDefault(),
        Permissions = getFromClain(ConstOptions.PermissionType).FirstOrDefault()?.Split(","),
        Avatar = getFromClain(ConstOptions.AvatarType).FirstOrDefault(),
        ExpireUCTTime = expdate,
        Roles = getFromClain(ConstOptions.RoleType),
        TokenString = token,
        ExpireSecond = (ulong)expTimeSpend.TotalSeconds,
        UserEmail = getFromClain(ConstOptions.UserEmail).FirstOrDefault(),
        IsSystemToken = identity.Claims.Any(b => b.Type == Common.SystemApiClaimType),
        TenantID = guidAvaliable ? tId : null,
        Claims = additionalClaim,
      };
      if (tokenManagement.CheckUserEveryRequest && await isLockoutAsync(dto.UserName))
      {
        return result;
      }
      result.SetSuccess(dto);
      return result;
    }
    public ContentResponse<TokenDTO> ValidateAndConvertTokenSync(string token, bool checkName = true)
    {
      return ValidateAndConvertToken(token).GetAwaiter().GetResult();
    }

    public async Task<bool> HasPermission(IEnumerable<string> permissions)
    {
      var tokenResponse = await GetTokenDTO();
      if (!tokenResponse.Success)
      {
        return false;
      }
      return permissions.Any(b => tokenResponse.Content.Permissions.Contains(b));
    }
    public async Task<bool> HasPermission(string permissions)
    {
      var tokenResponse = await GetTokenDTO();
      if (!tokenResponse.Success)
      {
        return false;
      }
      return tokenResponse.Content.Permissions.Contains(permissions);
    }
    public async Task<ContentResponse<TokenDTO>> GetTokenDTO()
    {
      var result = new ContentResponse<TokenDTO>();
      if (httpContext.HttpContext.User.Identity.IsAuthenticated)
      {
        var userName = httpContext.HttpContext.User.Identity.Name;
        if (await isLockoutAsync(userName))
        {
          return result;
        }
        var userSaved = await userTokenStorage.GetUserTokenAsync(userName);
        if (userSaved != null)
        {
          result.SetSuccess(userSaved);
          return result;
        }
        var user = await userManager.FindByNameAsync(userName);
        if (user == null || await isLockoutAsync(user.UserName))
        {
          return result;
        }

        return await getTokenDTO(user);
      }
      else
      {
        var token = httpContext.HttpContext?.Request?.Headers?.GetAccessToken();
        if (!token.Success)
        {
          return result;
        }
        return await ValidateAndConvertToken(token.Content);
      }
    }
    public async Task<ContentResponse<TokenDTO>> GetTokenDTO(LoginDTO request)
    {
      var result = new ContentResponse<TokenDTO>();
      var user = await userManager.FindByNameAsync(request.Username);
      if (user == null)
      {
        user = await userManager.FindByEmailAsync(request.Username);
      }
      if (user == null)
      {
        result.SetError(HttpStatusCode.Forbidden);
        return result;
      }
      if (await isLockoutAsync(user.UserName))
      {
        result.SetError(HttpStatusCode.Forbidden);
        return result;
      }
      var checkUser = await userManager.CheckPasswordAsync(user, request.Password);
      if (!checkUser)
      {
        result.SetError(HttpStatusCode.Forbidden);
        return result;
      }

      return await getTokenDTO(user, request.KeepLogin);
    }
    protected async Task<ContentResponse<TokenDTO>> getTokenDTO(TUser user, bool refreshToken = false, string currentRefreshToken = null, bool? impersonate = null)
    {
      var result = new ContentResponse<TokenDTO>();
      if (await isLockoutAsync(user.UserName))
      {
        result.SetError(HttpStatusCode.Forbidden);
        return result;
      }
      try
      {

        var claims = (await signInManager.ClaimsFactory.CreateAsync(user)).Claims.ToList();
        if (impersonate == true && user.ImpersonateRoles?.Any() == true)
        {
          foreach (var r in user.ImpersonateRoles)
          {
            claims.Add(new Claim(ConstOptions.RoleType, r));
          }
        }

        var moduleMap = ModuleOption.ModuleMap.ToDictionary(b => b.Key, b => sp.GetService(b.Value) as ServiceModule);
        foreach (var s in ModuleOption.ExtureModuleMap)
        {
          if (moduleMap.ContainsKey(s.Key))
          {
            continue;
          }
          moduleMap.TryAdd(s.Key, s.Value);
        }

        var roles = claims.Where(b => b.Type.IndexOf(ConstOptions.RoleType) >= 0).Select(b => b.Value).Distinct().ToList();
        var permissions = new List<string>();
        var roleKey = String.Join(",", roles.OrderBy(b => b)).ToLower() + "_" + tenantId?.ToString() ?? Guid.Empty.ToString();
        var db = sp.GetService<IContext>();
        if (tokenManagement.CachedPermission && rolePermissionCache.TryGetValue(roleKey, out var cachedValue))
        {
          permissions = cachedValue.ToList();
        }
        else
        {
          db.SetIgnoreTenant(false);
          foreach (var m in moduleMap.Values)
          {
            var p = await m!.GetRoleBasedPermissionAsync(db!, roles, "");
            if (p.Success && !string.IsNullOrEmpty(p.Content))
            {
              permissions.AddRange(p.Content.Split(','));
            }
          }
          var valueReadyToCache = permissions.Distinct().ToArray();
          rolePermissionCache.AddOrUpdate(roleKey, valueReadyToCache, (a, b) => valueReadyToCache);
        }

        claims.Add(new Claim(ConstOptions.PermissionType, String.Join(',', permissions)));
        claims.Add(new Claim(ConstOptions.UserEmail, user.Email ?? ""));
        string avatar = String.Empty;
        try
        {
          if (user.GetType().IsImplement(typeof(ICmfUser)))
          {
            var cmfUser = user as ICmfUser;
            avatar = cmfUser.Avatar ?? "";
          }
        }
        catch (Exception ex)
        {
          ex.ThrowStatusException();
          result.SetError(HttpStatusCode.Forbidden);
        }
        claims.Add(new Claim(ConstOptions.AvatarType, avatar ?? ""));
        claims.Add(new Claim(Common.TenantIDHeader, user.TenantID?.ToString() ?? ""));
        if (impersonate == true)
        {
          claims.Add(new Claim(ConstJWT.ImpersonateFlag, "true"));
        }

        Dictionary<string, string>? dtoClaims = null;
        if (additionalTokenService != null)
        {
          var additionalClaims = await additionalTokenService.GetAdditionalClaimAsync(user);
          if (additionalClaims?.Any() == true)
          {
            foreach (var ad in additionalClaims)
            {
              claims.Add(ad);
            }
          }
          dtoClaims = await additionalTokenService.GetAdditionalDTOClaimAsync(user);
        }
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenManagement.Secret));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expireTime = DateTime.UtcNow.AddMinutes(tokenManagement.AccessExpiration);
        var jwtToken = new JwtSecurityToken(tokenManagement.Issuer, tokenManagement.Audience, claims,
            expires: expireTime,
            signingCredentials: credentials);

        var token = new JwtSecurityTokenHandler().WriteToken(jwtToken);
        var lowerUserName = user.UserName.ToLower();
        var tokenDto = new TokenDTO()
        {
          ExpireUCTTime = expireTime,
          TokenString = token,
          UserId = user.Id,
          UserName = user.UserName,
          UserEmail = user.Email,
          Avatar = avatar,
          Roles = roles,
          Permissions = permissions.ToArray(),
          TenantID = user.TenantID,
          Claims = dtoClaims,
        };

        if (refreshToken)
        {
          tokenDto.RefreshTokenString = (await getRefreshToken(user.UserName, impersonate ?? false)).Content;
        }
        else
        {
          tokenDto.RefreshTokenString = currentRefreshToken;
        }
        await userTokenStorage.AddOrUpdateTokenAsync(user.UserName, tokenDto);

        result.SetSuccess(tokenDto);

        return result;
      }
      catch (Exception ex)
      {
        ex.ThrowStatusException();
        result.SetError(HttpStatusCode.Forbidden);
      }
      return result;

    }

    public string FullAccessRole => tokenManagement.FullAccessRole;

    private bool isLockout(TUser user)
    {
      SetLockedOutUser(user.UserName, user.LockoutEnabled ? user.LockoutEnd : null);
      if (user.LockoutEnabled && user.LockoutEnd.HasValue && user.LockoutEnd.Value > new DateTimeOffset(DateTime.Now))
      {
        return true;
      }
      return false;
    }
    private async Task<bool> isLockoutAsync(string userName)
    {
      var userLockCheck = await userLockendStorage.GetUserLockendAsync(userName);
      if (userLockCheck.HasRecord)
      {
        return userLockCheck.Lockend.HasValue && userLockCheck.Lockend.Value > new DateTimeOffset(DateTime.Now);
      }
      var user = await userManager.FindByNameAsync(userName);
      if (user == null)
      {
        return true;
      }
      return isLockout(user);
    }


    public void SetLockedOutUser(string userName, DateTimeOffset? lockoutEnd)
    {
      try
      {
        Task.WaitAll(
          userLockendStorage.AddOrUpdateUserLockendAsync(userName, lockoutEnd));
      }
      catch (Exception ex)
      {
        ex.ThrowStatusException();
      }

    }
    public async Task<ContentResponse<TokenDTO>> GetTokenAsync(TUser user, bool getRefresnToken, bool isImpersonate, CancellationToken ct, string? refreshToken = null)
    {
      var result = new ContentResponse<TokenDTO>();
      if (isImpersonate)
      {
        if (!user.TenantID.HasValue)
        {
          result.SetError(HttpStatusCode.Unauthorized, "user is not correct");
          return result;
        }
        var tIS = sp.GetService<ITenantImpersonateService<TUser>>();
        if (tIS == null)
        {
          result.SetError(HttpStatusCode.NotImplemented, "service not found");
          return result;
        }
        var enableImpersonate = await tIS.CheckImpersonateAsync(user);
        if (!enableImpersonate)
        {
          result.SetError(HttpStatusCode.Forbidden, "user is not correct");
          return result;
        }
      }

      var dto = await getTokenDTO(user, getRefresnToken, refreshToken, isImpersonate);
      if (!dto.Success || dto.Content == null)
      {
        result.SetError(HttpStatusCode.BadRequest, "user is not correct");
        return result;
      }
      result.SetSuccess(dto.Content);
      return result;
    }
    protected Guid? tenantId => tenantDetail?.Value?.TenantID ?? Guid.Empty;

  }

}
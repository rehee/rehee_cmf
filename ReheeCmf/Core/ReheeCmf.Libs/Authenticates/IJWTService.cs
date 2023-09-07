using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ReheeCmf.Authenticates
{
  public interface IJWTService
  {
    string FullAccessRole { get; }
    Task<ContentResponse<string>> GetToken(LoginDTO request);
    Task<ContentResponse<TokenDTO>> GetTokenDTO(LoginDTO request);
    Task<ContentResponse<string>> GetToken(string userName);
    Task<ContentResponse<TokenDTO>> GetTokenDTO(string userName);
    Task<ContentResponse<string>> GetToken();
    Task<ContentResponse<TokenDTO>> GetTokenDTO();
    Task<ContentResponse<ClaimsPrincipal>> ValidateToken(string token, bool checkName = true);
    Task<ContentResponse<TokenDTO>> ValidateAndConvertToken(string token, bool checkName = true);
    ContentResponse<TokenDTO> ValidateAndConvertTokenSync(string token, bool checkName = true);
    Task<ContentResponse<string>> GetRefreshToken(string userName);
    Task<ContentResponse<TokenDTO>> RefreshAccessToken(string token);
    string GetSystemApiToken();
    Task<bool> HasPermission(IEnumerable<string> permissions);
    Task<bool> HasPermission(string permissions);
    void SetLockedOutUser(string userName, DateTimeOffset? lockoutEnd);
    bool EnableAuth();
  }
  public interface IJWTService<TCmfUser> where TCmfUser : ICmfUser
  {
    Task<ContentResponse<TokenDTO>> GetTokenAsync(TCmfUser user, bool getRefresnToken, bool isImpersonate, CancellationToken ct, string? refreshToken = null);
  }

}

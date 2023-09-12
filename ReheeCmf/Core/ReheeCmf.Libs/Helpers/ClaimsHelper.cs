using ReheeCmf.Authenticates;
using System.Security.Claims;

namespace ReheeCmf.Helpers
{
  public static class ClaimsHelper
  {
    public static ClaimsIdentity ToClaim(this TokenDTO dto)
    {

      if (dto == null)
      {
        return null;
      }
      var identity = new ReheeCmfIdentity()
      {
        Name = dto.UserName,
        IsAuthenticated = true,
      };
      var claims = new Claim[]
      {
        new Claim(ClaimTypes.NameIdentifier, dto.UserId),
        new Claim(ClaimTypes.Name, dto.UserName),
        new Claim(ClaimTypes.Email, dto.UserEmail),
        new Claim("Avatar", dto.Avatar??""),
      };
      var claimRoles = dto.Roles.Where(b => !String.IsNullOrEmpty(b)).Select(b => new Claim(ClaimTypes.Role, b)).ToArray();
      var claimPermissions = dto.Permissions.Where(b => !String.IsNullOrEmpty(b)).Select(b => new Claim("Permissions", b)).ToArray();
      return new ClaimsIdentity(identity, claims.Concat(claimRoles).Concat(claimPermissions), identity.AuthenticationType, ClaimTypes.Name, ClaimTypes.Role);
    }
    public static TokenDTO ToUserDTO(this ClaimsPrincipal identity)
    {
      var dto = new TokenDTO();
      var claims = identity.Claims;
      if (claims?.Any() == true)
      {
        Func<string, string> getClaim = (key) => claims.Where(b => b.Type.Equals(key, StringComparison.OrdinalIgnoreCase)).Select(b => b.Value).FirstOrDefault();
        dto.UserId = getClaim(ClaimTypes.NameIdentifier);
        dto.UserName = getClaim(ClaimTypes.Name);
        dto.UserEmail = getClaim(ClaimTypes.Email);
        dto.Avatar = getClaim("Avatar");
      }

      return dto;
    }

    public static bool TryGetClaim(this ClaimsPrincipal user, string claimType, out string? value)
    {
      if (user == null || user.Claims?.Any() != true)
      {
        value = null;
        return false;
      }
      var claim = user.Claims.Where(b => b.Type.Equals(claimType, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
      if (claim == null)
      {
        value = null;
        return false;
      }
      value = claim.Value;
      return true;
    }

    public static IEnumerable<Claim> UserSigninClaim(this ICmfUser user)
    {
      yield return new Claim(Common.TenantIDHeader, user?.TenantID?.ToString() ?? "");
    }
  }
}

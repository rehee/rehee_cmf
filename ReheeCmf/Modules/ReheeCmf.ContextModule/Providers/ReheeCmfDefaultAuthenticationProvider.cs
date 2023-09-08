using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using ReheeCmf.Authenticates;
using ReheeCmf.Commons;
using ReheeCmf.Commons.Encrypts;
namespace ReheeCmf.ContextModule.Providers
{
  public class ReheeCmfDefaultAuthenticationProvider<TUser> : IUserTwoFactorTokenProvider<TUser> where TUser : IdentityUser
  {
    private TokenManagement tokenManagement { get; set; }
    private ApiSetting apiSetting { get; set; }
    public ReheeCmfDefaultAuthenticationProvider(TokenManagement tokenManagement, ApiSetting apiSetting)
    {
      this.tokenManagement = tokenManagement;
      this.apiSetting = apiSetting;
    }

    public Task<bool> CanGenerateTwoFactorTokenAsync(UserManager<TUser> manager, TUser user)
    {
      if (manager != null && user != null)
      {
        return Task.FromResult(true);
      }
      else
      {
        return Task.FromResult(false);
      }
    }
    private string GenerateToken(TUser user, string purpose)
    {
      var token = $"{tokenManagement.GetTokenGenerateSecret}__{user.Email}__{purpose}__{user.Id}";
      return token.RSAEncrypt(apiSetting.RSAOption.RSAPublicKey);
    }
    public Task<string> GenerateAsync(string purpose, UserManager<TUser> manager, TUser user)
    {
      return Task.FromResult(GenerateToken(user, purpose));
    }
    public Task<bool> ValidateAsync(string purpose, string token, UserManager<TUser> manager, TUser user)
    {
      var tokens = token.RSADecrypt(apiSetting.RSAOption.RSAPrivateKey).Split("__").ToArray();

      return Task.FromResult(
        tokens.Length == 4 &&
        tokens[0] == tokenManagement.GetTokenGenerateSecret &&
        tokens[1] == user.Email &&
        tokens[2] == purpose &&
        tokens[3] == user.Id);
    }
  }
}

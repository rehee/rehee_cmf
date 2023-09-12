using ReheeCmf.Authenticates;
using ReheeCmf.Responses;
namespace ReheeCmf.ContextModule.Managers
{
  public class LoginDTOSignInManager<TUser> : IDTOSignInManager<LoginDTO> where TUser : IdentityUser, new()
  {
    private readonly UserManager<TUser> userManager;
    private readonly SignInManager<TUser> signInManager;

    public LoginDTOSignInManager(UserManager<TUser> userManager, SignInManager<TUser> signInManager)
    {
      this.userManager = userManager;
      this.signInManager = signInManager;
    }
    public async Task<ContentResponse<bool>> LoginAsync(LoginDTO dto, CancellationToken ct, bool checkPassword = true)
    {
      var result = new ContentResponse<bool>();
      try
      {
        TUser? user = null;
        user = await userManager.FindByEmailAsync(dto.Username);
        if (user == null)
        {
          user = await userManager.FindByNameAsync(dto.Username);
        }
        if (user == null)
        {
          result.SetError(HttpStatusCode.NotFound);
          return result;
        }
        if (checkPassword)
        {
          var checkPasswordResult = await userManager.CheckPasswordAsync(user, dto.Password);
          if (!checkPassword)
          {
            result.SetError(HttpStatusCode.Forbidden);
            return result;
          }
        }
        if(user is ICmfUser cmfUser)
        {
          await signInManager.SignInWithClaimsAsync(user, dto.KeepLogin, cmfUser.UserSigninClaim());
        }
        else
        {
          await signInManager.SignInAsync(user, dto.KeepLogin);
        }
        
        result.SetSuccess(true);
        return result;
      }
      catch (Exception ex)
      {
        result.SetError(ex);
        return result;
      }

    }

    public async Task<ContentResponse<bool>> LogoutAsync(CancellationToken ct)
    {
      var result = new ContentResponse<bool>();
      try
      {
        await signInManager.SignOutAsync();
        result.SetSuccess(true);
        return result;
      }
      catch (Exception ex)
      {
        result.SetError(ex);
        return result;
      }
    }
  }
}

using ReheeCmf.Authenticates;
using ReheeCmf.Responses;
namespace ReheeCmf.ContextModule.Managers
{
  public class ClaimsPrincipalSignInManager<TUser> : IDTOSignInManager<ClaimsPrincipalDTO> where TUser : IdentityUser, new()
  {
    private readonly UserManager<TUser> userManager;
    private readonly SignInManager<TUser> signInManager;
    public ClaimsPrincipalSignInManager(UserManager<TUser> userManager, SignInManager<TUser> signInManager)
    {
      this.userManager = userManager;
      this.signInManager = signInManager;
    }
    public async Task<ContentResponse<bool>> LoginAsync(ClaimsPrincipalDTO dto, CancellationToken ct, bool checkPassword = true)
    {
      var result = new ContentResponse<bool>();
      try
      {
        await signInManager.SignOutAsync();
        if (!dto.User.Identity.IsAuthenticated)
        {
          result.SetError(HttpStatusCode.Forbidden);
          return result;
        }
        var userDto = dto.User.ToUserDTO();
        if (string.IsNullOrEmpty(userDto.UserId) || string.IsNullOrEmpty(userDto.UserName) || string.IsNullOrEmpty(userDto.UserEmail))
        {
          result.SetNotFound();
          return result;
        }
        var user = await userManager.FindByIdAsync(userDto.UserId);
        if (user == null)
        {
          result.SetNotFound();
          return result;
        }
        if (!user.UserName.Equals(userDto.UserName, StringComparison.OrdinalIgnoreCase) ||
          !user.Email.Equals(userDto.UserEmail, StringComparison.OrdinalIgnoreCase))
        {
          result.SetNotFound();
          return result;
        }
        if (user is ICmfUser cmfUser)
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

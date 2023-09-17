using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using ReheeCmf.Authenticates;
using ReheeCmf.Commons;
using ReheeCmf.Commons.DTOs;
using ReheeCmf.Contexts;
using ReheeCmf.Helpers;
using ReheeCmf.Services;
using ReheeCmf.StandardInputs.StandardItems;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace ReheeCmf.UserManagementModule.Services
{
  public class UserService<TUser, TRole, TUserRole> : IUserService
    where TUser : IdentityUser, new()
    where TRole : IdentityRole, new()
    where TUserRole : IdentityUserRole<string>, new()
  {
    private UserManagementOption? option;
    private readonly UserManager<TUser> userManager;
    private readonly RoleManager<TRole> roleManager;
    private readonly IServiceProvider sp;
    private readonly IUserRoleService userRoleService;
    public UserService(IServiceProvider sp)
    {
      option = sp.GetService<UserManagementOption>();
      this.userManager = sp.GetService<UserManager<TUser>>()!;
      this.roleManager = sp.GetService<RoleManager<TRole>>()!;
      this.sp = sp;
      this.userRoleService = sp.GetService<IUserRoleService>()!;
    }
    protected virtual async Task<string> hashPasswordAsync(TUser? entity, string? password)
    {
      var passwordCheck = await userManager.PasswordValidators.FirstOrDefault()!.ValidateAsync(userManager, entity!, password!);
      checkResult(passwordCheck);
      return userManager.PasswordHasher.HashPassword(entity!, password!);
    }
    protected virtual void checkResult(IdentityResult? result)
    {
      if (result == null || result.Succeeded)
      {
        return;
      }
      StatusException.Throw(
        result.Errors.Select(b => ValidationResultHelper.New(b.Description, b.Code)).ToArray()
      );
    }
    public async Task<bool> ChangePasswordAsync(ChangePasswordDTO? dto, TokenDTO? user, CancellationToken ct = default)
    {
      if (dto == null || user == null)
      {
        StatusException.Throw(ValidationResultHelper.New("DTO or User is required"));
      }
      var validation = dto!.Validate(new ValidationContext(dto));
      if (validation.Any())
      {
        StatusException.Throw(validation.ToArray());
      }
      var entity = await userManager.FindByIdAsync(user!.UserId!);
      if (entity == null)
      {
        StatusException.Throw(HttpStatusCode.NotFound);
      }
      var oldPasswordCheck = await userManager.CheckPasswordAsync(entity!, dto.OldPassword!);
      if (oldPasswordCheck != true)
      {
        StatusException.Throw(HttpStatusCode.Forbidden);
      }

      entity!.PasswordHash = await hashPasswordAsync(entity, dto.NewPassword);
      await userManager.UpdateAsync(entity);
      return true;
    }

    public async Task<string> CreateRoleAsync(Dictionary<string, string?> data, TokenDTO? user, CancellationToken ct = default)
    {
      var role = new TRole();
      StandardInputHelper.UpdateProperty(role, data);
      var result = await roleManager.CreateAsync(role);
      checkResult(result);
      return role.Id;
    }

    public async Task<string> CreateUserAsync(Dictionary<string, string?> properties, TokenDTO? user = null)
    {
      properties.TryGetValueStringKey(UserManagementOption.PasswordProperty, out var password);
      var entity = new TUser();
      StandardInputHelper.UpdateProperty(entity,
        properties.ToDictionWithKeys(option!.UserDetailPropertyCreate!.Concat(new string[] { option.RoleProperty! })));
      var result = String.IsNullOrEmpty(password) ? await userManager.CreateAsync(entity) : await userManager.CreateAsync(entity, password);
      checkResult(result);


      await userRoleService.UserRoleAsync(entity, properties);
      return entity.Id;
    }

    public async Task<bool> DeleteRoleAsync(string idOrName, TokenDTO? user, CancellationToken ct = default)
    {
      TRole? role = null;
      role = await roleManager.FindByIdAsync(idOrName);
      if (role == null)
      {
        role = await roleManager.FindByNameAsync(idOrName);
      }
      if (role == null)
      {
        StatusException.Throw(HttpStatusCode.NotFound);
      }
      var result = await roleManager.DeleteAsync(role!);
      checkResult(result);
      return true;
    }

    public IEnumerable GetAllRoles()
    {
      return roleManager.Roles;
    }

    public IEnumerable? GetAllUserRoles()
    {
      var context = sp.GetService<IContext>();
      if (context == null)
      {
        return default(IEnumerable?);
      }
      return context.Query<TUserRole>(true);
    }

    public async Task<StandardItem?> GetUserDetailAsync(string? userId, TokenDTO? user = null, CancellationToken ct = default)
    {
      if (userId == null)
      {
        StatusException.Throw(HttpStatusCode.NotFound);
      }
      var entity = await userManager.FindByIdAsync(userId!);
      if (entity == null)
      {
        StatusException.Throw(HttpStatusCode.NotFound);
      }
      return StandardInputHelper.GetStandardProperty(entity!, sp.GetService<IContext>());
    }

    public IEnumerable? GetUserRoles(string? userId, TokenDTO? user = null)
    {
      var context = sp.GetService<IContext>();
      if (context == null)
      {
        return default;
      }
      return
        from role in context.Query<TRole>(true)
        join userRole in context.Query<TUserRole>(true).Where(b => b.UserId.Equals(userId)) on role.Id equals userRole.RoleId
        select role.Name;
    }

    public IEnumerable ReadUsers(string? role, TokenDTO? user = null)
    {
      return userManager.Users;
    }

    public async Task<bool> ResetPassword(string userId, string? password, TokenDTO? user, bool forgot = false, string? token = null)
    {
      if (userId == null)
      {
        StatusException.Throw(HttpStatusCode.NotFound);
      }
      var entity = await userManager.FindByIdAsync(userId!);
      if (entity == null)
      {
        StatusException.Throw(HttpStatusCode.NotFound);
      }
      if (String.IsNullOrWhiteSpace(password))
      {
        password = $"{entity?.UserName ?? "ChangeMe"}_Password1";
      }
      var passwordHash = await hashPasswordAsync(entity, password);

      entity!.PasswordHash = passwordHash;
      await userManager.UpdateAsync(entity);
      return true;
    }

    public async Task<bool> UpdateUserAsync(string id, Dictionary<string, string?> properties, TokenDTO? user = null, CancellationToken ct = default)
    {
      if (id == null)
      {
        StatusException.Throw(HttpStatusCode.NotFound);
      }
      var entity = await userManager.FindByIdAsync(id!);
      if (entity == null)
      {
        StatusException.Throw(HttpStatusCode.NotFound);

      }
      StandardInputHelper.UpdateProperty(entity,
        properties.ToDictionWithKeys(option!.UserDetailPropertyEdit!.Concat(new string[] { option.RoleProperty! })));

      var result = await userManager.UpdateAsync(entity!);
      checkResult(result);
      await userRoleService.UserRoleAsync(entity!, properties);
      return true;
    }
  }
}

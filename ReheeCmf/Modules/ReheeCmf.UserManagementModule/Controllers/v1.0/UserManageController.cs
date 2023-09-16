using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.Extensions.DependencyInjection;
using ReheeCmf.Authenticates;
using ReheeCmf.Commons;
using ReheeCmf.Commons.DTOs;
using ReheeCmf.Modules.Controllers;
using ReheeCmf.Reflects.ReflectPools;
using ReheeCmf.Services;
using ReheeCmf.UserManagementModule.Consts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReheeCmf.UserManagementModule.Controllers.v1._0
{
  [ApiController]
  [Route(CrudOption.UserManagementApiEndpoint)]
  public class UserManageController : ReheeCmfController
  {
    private readonly IUserService us;


    public UserManageController(IServiceProvider sp) : base(sp)
    {
      us = sp.GetService<IUserService>();
    }

    [EnableQuery()]
    [HttpGet("json/{role?}")]
    [CmfAuthorize(EntityNameForce = ConstCmfUserManagementModule.PermissionReadUsers, EntityRoleBase = true)]
    public async Task<IActionResult> GetUser(string role)
    {
      var path = Request.Path.Value;
      if (path.Contains("/json", StringComparison.OrdinalIgnoreCase))
      {
        return NotFound();
      }
      var queryString = Request.QueryString.Value;
      var requestUrl = Request.HttpContext.RequestAborted.ToString();
      //var queryFirst = Request.QueryBeforeFilter(crudOption.UserType);

      if (Request.HttpContext.Request.RouteValues.TryGetValue("role", out var roleString))
      {
        role = roleString as string;
      }
      var result = us.ReadUsers(role, currentUser);
      return this.StatusCode(200, result);
    }
    [HttpGet("Roles/{userId?}")]
    [CmfAuthorize(EntityNameForce = ConstCmfUserManagementModule.PermissionReadUsers, EntityRoleBase = true)]
    public async Task<IActionResult> GetUserRoles(string userId)
    {
      return Ok(us.GetUserRoles(userId, currentUser));
    }
    [HttpGet("Detail/{id?}")]
    [CmfAuthorize(EntityNameForce = ConstCmfUserManagementModule.PermissionCreateUser, EntityRoleBase = true)]
    public async Task<IActionResult> GetUserItemBtyId(string id, CancellationToken ct)
    {
      return await getUserItem(id ?? "-1", ct);
    }
    private async Task<IActionResult> getUserItem(string id, CancellationToken ct)
    {
      return Ok(await us.GetUserDetailAsync(id, currentUser, ct));
    }
    [HttpPost("Detail")]
    [CmfAuthorize(EntityNameForce = ConstCmfUserManagementModule.PermissionCreateUser, EntityRoleBase = true)]
    public async Task<IActionResult> CreateUser(Dictionary<string, string> properties)
    {
      return Ok(await us.CreateUserAsync(properties, currentUser));
    }
    [HttpPut("Detail/{id?}")]
    [CmfAuthorize(EntityNameForce = ConstCmfUserManagementModule.PermissionEditUser, EntityRoleBase = true)]
    public async Task<IActionResult> UpdateUser(string id, Dictionary<string, string> properties, CancellationToken ct)
    {
      return Ok(await us.UpdateUserAsync(id, properties, currentUser, ct));
    }
    [HttpGet("Role/Json")]
    [EnableQuery()]
    [CmfAuthorize(EntityNameForce = ConstCmfUserManagementModule.PermissionReadRolePermission, EntityRoleBase = true)]
    public IActionResult GetRoles()
    {
      return Ok(us.GetAllRoles());
    }
    [HttpPost(CrudOption.RoleEndpoint)]
    [CmfAuthorize(EntityNameForce = ConstCmfUserManagementModule.PermissionCreateUser, EntityRoleBase = true)]
    public async Task<IActionResult> CreateRolesAsync(Dictionary<string, string> data, CancellationToken ct)
    {
      return Ok(await us.CreateRoleAsync(data, currentUser, ct));
    }
    [HttpDelete($"{CrudOption.RoleEndpoint}/{{idOrName}}")]
    [CmfAuthorize(EntityNameForce = ConstCmfUserManagementModule.PermissionDeleteRole, EntityRoleBase = true)]
    public async Task<IActionResult> DeleteRolesAsync(string idOrName, CancellationToken ct)
    {
      return Ok(await us.DeleteRoleAsync(idOrName, currentUser, ct));
    }
    [HttpGet("UserRoles/Json")]
    [EnableQuery()]
    [CmfAuthorize(EntityNameForce = ConstCmfUserManagementModule.PermissionReadUserRolePermission, EntityRoleBase = true)]
    public IActionResult GetUserRoles()
    {
      return Ok(us.GetAllUserRoles());
    }

    //[HttpPut(CrudOption.UserChangeAvatarEndpoint)]
    //[CmfAuthorize(AuthOnly = true)]
    //public async Task<IActionResult> ChangeAvatra(AvatarDTO dto, CancellationToken ct)
    //{

    //  return Ok(await dto.SelfConstract(sp, user).SaveChange(user));
    //}

    //[HttpPut($"{CrudOption.UserChangeAvatarEndpoint}/File")]
    //[CmfAuthorize(AuthOnly = true)]
    //public async Task<IActionResult> ChangeAvatra(CancellationToken ct)
    //{
    //  var dto = new AvatarDTO();
    //  var request = await Request.GetFileRequest();
    //  var user = await GetUserInfo();
    //  return Ok(await dto.SelfConstract(sp, user).UpdateAvatar(request, user, ct));
    //}

    [HttpPut(CrudOption.UserChangePasswordEndpoint)]
    [CmfAuthorize(AuthOnly = true)]
    public async Task<IActionResult> ChangePassword(ChangePasswordDTO dto, CancellationToken ct)
    {
      var result = await us.ChangePasswordAsync(dto, currentUser, ct);
      if (result)
      {
        return Ok();
      }
      return BadRequest();
    }
    [HttpPost($"{CrudOption.ResetUserPasswordEndpoint}/{{userId}}")]
    [CmfAuthorize(EntityNameForce = ConstCmfUserManagementModule.PermissionResetUserPassword)]
    public async Task<IActionResult> ResetUserPassword(string userId, string password, CancellationToken ct)
    {
      //var needEncrype = uo?.ResetPasswordEncrypt == true;
      //if (needEncrype)
      //{
      //  password = password.RSADecrypt(api.RSAOption.RSAPrivateKey);
      //}
      return Ok(await us.ResetPassword(userId, password, currentUser));
    }

    [HttpPost(CrudOption.ForgotPasswordEndpoint)]
    public async Task<IActionResult> ForgotPassword(ForgotPasswordDTO dto, CancellationToken ct)
    {

      return Ok();
    }
    [HttpPut(CrudOption.ForgotPasswordEndpoint)]
    public async Task<IActionResult> ForgotPasswordUpdate(ForgotPasswordDTO dto, CancellationToken ct)
    {

      return Ok();
    }
  }
}

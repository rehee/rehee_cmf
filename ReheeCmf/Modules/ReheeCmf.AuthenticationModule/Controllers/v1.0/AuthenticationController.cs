using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using ReheeCmf.Authenticates;
using ReheeCmf.Commons.DTOs;
using ReheeCmf.Modules.Controllers;
using ReheeCmf.Responses;
using ReheeCmf.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ReheeCmf.AuthenticationModule.Controllers.v1._0
{
  [ApiController]
  [Route("Api/Token")]
  public class AuthenticationController : ReheeCmfController
  {
    private readonly IJWTService jwt;
    private readonly IImpersonateManagement? im;
    public AuthenticationController(IJWTService jwt, IServiceProvider sp) : base(sp)
    {
      this.jwt = jwt;
      this.im = sp.GetService<IImpersonateManagement>();
    }

    protected async Task<IActionResult> LoginAsync(LoginDTO request, bool tokenOnly, CancellationToken ct)
    {
      ContentResponse<TokenDTO>? result = null;
      try
      {
        result = await jwt.GetTokenDTO(request);
      }
      catch
      {
        return Forbid();
      }
      if (result.Success && result.Content != null)
      {
        if (tokenOnly)
        {
          return Ok(result.Content.TokenString);
        }
        return Ok(result.Content);
      }
      if (im != null)
      {
        var imUser = await im.GetImpersonateTokenAsync(request, ct);
        if (imUser == null)
        {
          return Forbid();
        }

        if (tokenOnly)
        {
          return Ok(imUser.TokenString);
        }
        return Ok(imUser);

      }
      return Forbid();
    }

    [HttpPost("GetToken")]
    public async Task<IActionResult> GetToken(LoginDTO request, CancellationToken ct)
    {
      return await LoginAsync(request, true, ct);
    }
    [HttpPost("Login")]
    public async Task<IActionResult> GetTokenDTO(LoginDTO request, CancellationToken ct)
    {
      return await LoginAsync(request, false, ct);
    }
    [HttpPost("RefreshAccessToken")]
    public async Task<IActionResult> RefreshAccessToken(TokenValidate token)
    {
      if (String.IsNullOrEmpty(token?.Token))
      {
        return Forbid();
      }
      var result = await jwt.RefreshAccessToken(token?.Token);
      if (result.Success != true || result.Content == null)
      {
        return Forbid();
      }
      return Ok(result.Content);
    }
  }
}

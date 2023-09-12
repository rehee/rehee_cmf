using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using ReheeCmf.Authenticates;
using ReheeCmf.Commons.DTOs;
using ReheeCmf.Modules.Controllers;
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

    public AuthenticationController(IJWTService jwt, IServiceProvider sp) : base(sp)
    {
      this.jwt = jwt;
    }

    [HttpPost("GetToken")]
    public async Task<IActionResult> GetToken(LoginDTO request)
    {
      var tokenResponse = await jwt.GetToken(request);
      return StatusCode(tokenResponse.IntStatus, tokenResponse.Content);
    }
    [HttpPost("Login")]
    public async Task<IActionResult> GetTokenDTO(LoginDTO request)
    {
      var dto = await jwt.GetTokenDTO(request);
      return StatusCode(dto.IntStatus, dto.Content);
    }
    [HttpPost("RefreshAccessToken")]
    public async Task<IActionResult> RefreshAccessToken(TokenValidate token)
    {
      if (String.IsNullOrEmpty(token?.Token))
      {
        return StatusCode((int)HttpStatusCode.Forbidden);
      }
      var result = await jwt.RefreshAccessToken(token?.Token);
      return StatusCode(result.IntStatus, result.Content);
    }
  }
}

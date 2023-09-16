using ReheeCmf.DTOProcessors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReheeCmf.Commons.DTOs
{
  public class ForgotPasswordDTO : IQueryKey
  {
    public string? UserEmail { get; set; }
    public string? UserId { get; set; }
    public string? Token { get; set; }
    public string? Password { get; set; }
    public string? QueryKey { get; set; }
  }
  public class ForgotPasswordOption
  {
    public string? Title { get; set; }
    public string? Logo { get; set; }
    public string? SiteName { get; set; }
    public string? ResetUrl { get; set; }
    public string? TemplatePath { get; set; }
    public string? PathType { get; set; }
    public string? Assembly { get; set; }
  }
  public class ForgotPasswordModel : ForgotPasswordOption
  {
    public ForgotPasswordModel()
    {

    }
    public ForgotPasswordModel(ForgotPasswordOption option, string token, string userEmail, string userId)
    {
      Title = option.Title;
      Logo = option.Logo;
      SiteName = option.SiteName;
      ResetUrl = option.ResetUrl;
      TemplatePath = option.TemplatePath;
      PathType = option.PathType;
      Assembly = option.Assembly;
      Token = token;
      UserEmail = userEmail;
      UserId = userId;
    }
    public string? UserEmail { get; set; }
    public string? UserId { get; set; }
    public string? Token { get; set; }
  }
}

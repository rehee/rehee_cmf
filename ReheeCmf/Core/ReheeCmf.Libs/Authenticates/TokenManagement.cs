using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReheeCmf.Authenticates
{
  public class TokenManagement
  {
    public string? Secret { get; set; }
    public string? Issuer { get; set; }
    public string? Audience { get; set; }
    public int AccessExpiration { get; set; }
    public bool EnableAuth { get; set; }
    public string? ServerApiValue { get; set; }
    public double ServerApiExpire { get; set; }
    public int RefreshTokenExtureDay { get; set; }
    public bool RefreshTokenExtention { get; set; }
    public static string DefaultTokenGenerateSecret { get; set; } = "Password_Token_123456789!";
    public string? TokenGenerateSecret { get; set; }
    public string? FullAccessRole { get; set; }
    public string GetTokenGenerateSecret => this.TokenGenerateSecret ?? DefaultTokenGenerateSecret;
    public bool CheckUserEveryRequest { get; set; }
  }
}

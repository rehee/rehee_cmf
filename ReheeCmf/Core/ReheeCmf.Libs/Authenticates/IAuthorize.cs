using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReheeCmf.Authenticates
{
  public interface IAuthorize
  {
    Task<string> FullAccessRole();
    Task<bool> EnableAuth();
    Task<ContentResponse<TokenDTO>> ValidateAndConvert(string token);
    Task<string> ApiSystemToken();
  }
}

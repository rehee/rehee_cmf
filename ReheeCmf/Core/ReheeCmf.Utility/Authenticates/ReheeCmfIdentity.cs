using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace ReheeCmf.Authenticates
{
  public class ReheeCmfIdentity : IIdentity
  {

    public string AuthenticationType
    {
      get
      {
        return "AuthenticationTypes.Federation";
      }
      set
      {

      }
    }
    public bool IsAuthenticated { get; set; }
    public string? Name { get; set; }
  }
}

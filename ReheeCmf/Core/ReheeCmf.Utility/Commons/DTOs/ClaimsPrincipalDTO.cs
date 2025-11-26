using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ReheeCmf.Commons.DTOs
{
  public class ClaimsPrincipalDTO
  {
    public ClaimsPrincipal? User { get; set; }
    public bool KeepLogin { get; set; }
  }
}

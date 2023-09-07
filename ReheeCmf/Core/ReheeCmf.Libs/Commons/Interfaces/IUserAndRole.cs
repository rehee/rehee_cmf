using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReheeCmf.Commons
{
  public interface IUserAndRole
  {
    Type? UserType { get; set; }
    Type? RoleType { get; set; }
  }
}

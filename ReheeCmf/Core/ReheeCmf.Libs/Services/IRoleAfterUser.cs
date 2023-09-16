using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReheeCmf.Services
{
  public interface IAfterUserCreate
  {
    Task AfterUserCreateAsync(object user, Dictionary<string, string?> properties, CancellationToken ct = default);
  }
}

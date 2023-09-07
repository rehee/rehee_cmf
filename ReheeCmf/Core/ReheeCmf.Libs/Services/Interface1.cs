using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReheeCmf.Services
{
  public interface IToken<T>
  {
    Task<string?> GetTokenAsync(CancellationToken ct);
  }
}

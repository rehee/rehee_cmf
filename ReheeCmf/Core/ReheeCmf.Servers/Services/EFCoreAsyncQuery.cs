using Microsoft.EntityFrameworkCore;
using ReheeCmf.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReheeCmf.Servers.Services
{
  public class EFCoreAsyncQuery : IIAsyncQuery
  {
    public async Task<T?> FirstOrDefaultAsync<T>(IQueryable<T> query, CancellationToken ct)
    {
      
      return await query.FirstOrDefaultAsync(ct);
    }
  }
}

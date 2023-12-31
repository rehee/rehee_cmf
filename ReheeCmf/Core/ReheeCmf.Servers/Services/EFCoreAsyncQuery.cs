﻿using Microsoft.EntityFrameworkCore;
using ReheeCmf.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReheeCmf.Servers.Services
{
  public class EFCoreAsyncQuery : IAsyncQuery
  {
    public async Task<T?> FirstOrDefaultAsync<T>(IQueryable<T> query, CancellationToken ct)
    {

      return await query.FirstOrDefaultAsync(ct);
    }

    public IQueryable<T> AsNoTracking<T>(IQueryable<T> query) where T : class
    {
      return query.AsNoTracking();
    }
    public Task<T[]> ToArrayAsync<T>(IQueryable<T> query, CancellationToken ct)
    {
      return query.ToArrayAsync(ct);
    }
  }
}

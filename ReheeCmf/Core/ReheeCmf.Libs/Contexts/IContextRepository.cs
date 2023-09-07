﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReheeCmf.Contexts
{
  public interface IContextRepository
  {
    IQueryable<T> Query<T>(bool asNoTracking) where T : class;
    Task<T> GetByIdAsync<T>(object id, CancellationToken cancellationToken) where T : class;
    Task AddAsync<T>(T entity, CancellationToken cancellationToken) where T : class;
    void Delete<T>(T entity) where T : class;
    void Delete(object entity);
    Task ExecuteTransactionAsync(Func<CancellationToken, Task> action, CancellationToken cancellationToken);
  }
}

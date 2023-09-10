﻿namespace ReheeCmf.Contexts
{
  public interface IContext : ISaveChange, IRepository, IWithTenant, ITenantContext, IDisposable
  {
    object? Context { get; }
    TokenDTO? User { get; }

    object? Query(Type type, bool noTracking);
    object? Find(Type type, object key);
    void Delete(Type type, object key);
  }
}

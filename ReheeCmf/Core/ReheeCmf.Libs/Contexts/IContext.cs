using System;

namespace ReheeCmf.Contexts
{
  public interface IContext : ISaveChange, IRepository, IWithTenant, ITenantContext, IDisposable
  {
    object? Context { get; }
    TokenDTO? User { get; }

    object? Query(Type type, bool noTracking);
    object? QueryWithKey(Type type, Type keyType, bool noTracking, object key);
    object? Find(Type type, object key);
    void Add(Type type, object? value);
    void Delete(Type type, object key);

    IEnumerable<KeyValueItemDTO> GetKeyValueItemDTO(Type type);
  }
}

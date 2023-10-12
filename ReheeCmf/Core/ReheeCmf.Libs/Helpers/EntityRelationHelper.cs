using System.Reflection;

namespace ReheeCmf.Helpers
{
  public static class EntityRelationHelper
  {
    public static Type? GetType(IEnumerable<Type>? typeMapper, string typeName)
    {
      if (typeMapper?.Any() != true)
      {
        return null;
      }
      return typeMapper.Where(b => string.Equals(b.Name, typeName, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
    }
    public static Type? GetKeyType(IDictionary<Type, PropertyInfo>? mapper, Type entityType)
    {
      if (mapper?.Any() != true || !mapper.TryGetValue(entityType, out var propertyInfo))
      {
        return null;
      }
      if (propertyInfo == null)
      {
        return null;
      }
      return propertyInfo.PropertyType;
    }
    public static (Type entityType, Type keyType, string entityName)? GetEntityTypeAndKey(string entityType)
    {
      var type = GetType(ReflectPool.EntityNameMapping.Select(b => b.Value), entityType);
      if (type == null)
      {
        return null;
      }
      var key = GetKeyType(ReflectPool.EntityKeyMapping, type);
      if (key == null)
      {
        return null;
      }
      return (entityType: type, keyType: key, entityName: type.Name);
    }
  }
}

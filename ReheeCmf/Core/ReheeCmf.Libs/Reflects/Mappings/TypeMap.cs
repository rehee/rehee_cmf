using System.Reflection;
namespace ReheeCmf.Reflects.Mappings
{
  public class TypeMap
  {
    public TypeMap(Type type)
    {
      this.Properties = type.GetProperties();
      this.Fields = type.GetFields();
      this.PropertiesWithPrivate = type.GetProperties(BindingFlags.NonPublic | BindingFlags.Instance);
      this.FieldsWithPrivate = type.GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
      this.Methods = type.GetMethods();
      this.MethodsPrivate = type.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance);
      this.Interfaces = type.GetInterfaces();
      this.Name = type.Name;
      this.FullName = type.FullName!;
      this.GenericTypeDefinition = type.IsGenericType ? type.GetGenericTypeDefinition() : null;
      GenericTypeArguments = type.GenericTypeArguments;
      ThisType = type;
      Attributes = type.GetCustomAttributes().ToArray();

      Constructors = type.GetConstructors();
      this.NoAutowiredProperties = this.PropertiesWithPrivate.Where(b =>
      {
        var attrs = b.CustomAttributes;
        return attrs.Any(b => b.AttributeType == typeof(NoAutowiredAttribute));
      }).ToArray();
      this.NoAutowiredFields = this.FieldsWithPrivate
        .Where(b => !b.IsStatic)
        .Where(b =>
        {
          var attrs = b.CustomAttributes;
          return attrs.Any(b => b.AttributeType == typeof(NoAutowiredAttribute));
        }).ToArray();
      this.AutowiredProperties = this.PropertiesWithPrivate.Where(b =>
      {
        var attrs = b.CustomAttributes;
        return attrs.Any(b => b.AttributeType == typeof(AutowiredAttribute));
      }).ToArray();
      this.AutowiredFields = this.FieldsWithPrivate
        .Where(b => !b.IsStatic)
        .Where(b =>
        {
          var attrs = b.CustomAttributes;
          return attrs.Any(b => b.AttributeType == typeof(AutowiredAttribute));
        }).ToArray();
    }
    public string Name { get; }
    public string FullName { get; }
    public Type ThisType { get; }
    public string? ElementName { get; set; }
    public Type[] Interfaces { get; }
    public Type[] GenericTypeArguments { get; }
    public MethodInfo[] Methods { get; }
    public MethodInfo[] MethodsPrivate { get; }
    public PropertyInfo[] Properties { get; }
    public FieldInfo[] Fields { get; }
    public PropertyInfo[] PropertiesWithPrivate { get; }
    public FieldInfo[] FieldsWithPrivate { get; }
    public Type? GenericTypeDefinition { get; }
    public ConstructorInfo[] Constructors { get; }
    public object[] Attributes { get; }

    public PropertyInfo[] NoAutowiredProperties { get; }
    public FieldInfo[] NoAutowiredFields { get; }

    public PropertyInfo[] AutowiredProperties { get; }
    public FieldInfo[] AutowiredFields { get; }

  }



}

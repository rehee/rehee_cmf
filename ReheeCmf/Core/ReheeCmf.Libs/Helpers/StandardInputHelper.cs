using ReheeCmf.StandardInputs;
using ReheeCmf.StandardInputs.Properties;
using ReheeCmf.StandardInputs.StandardItems;
using System.Data.Common;
using System.Reflection;
using System.Reflection.Metadata;

namespace ReheeCmf.Helpers
{
  public static class StandardInputHelper
  {
    public static StandardItem? GetStandardProperty(this object input, IContext? context)
    {
      if (input == null)
      {
        return null;
      }
      var result = new StandardItem();
      var type = input.GetType().ThisType();
      var mapper = type.GetMap();
      SetType(result, type);
      result.Properties = mapper.Properties.Select(b => b.GetMap())
        .Where(b => !b.Attributes.Any(b => b is NotMappedAttribute))
        .Where(b => b.Attributes.Any(b => b is FormInputsAttribute))
        .Select(b => b.PropertyInfoMapToStandardProperty(input, context));
      return result;
    }

    public static void SetType(IStandard? input, Type type)
    {
      if (input == null)
      {
        return;
      }
      input.FullTypeName = type.FullName!;
      input.Assembly = type.Assembly.FullName!.Split(",").Select(b => b.Trim()).FirstOrDefault()!;
    }

    public static StandardProperty PropertyInfoMapToStandardProperty(this PropertyInfoMap map, object inputObj, IContext? context)
    {
      var result = new StandardProperty();
      var inputAttribute = map.Attributes.Where(b => b is FormInputsAttribute).Select(b => b as FormInputsAttribute).FirstOrDefault() ??
        new FormInputsAttribute
        {
          InputType = EnumInputType.Text
        };
      var inputType = typeof(IStandardParameter);
      foreach (var p in inputType.GetProperties())
      {
        p.SetValue(result, p.GetValue(inputAttribute));
      }
      if (map.Attributes.Any(b => b is IgnoreUpdateAttribute))
      {
        result.ReadOnly = true;
      }
      result.PropertyName = map.Name;
      var displayName = map.Attributes.Where(b => b is DisplayAttribute).Select(b => b as DisplayAttribute).FirstOrDefault();
      if (displayName != null)
      {
        result.Label = displayName.Name;
      }
      else
      {
        result.Label = result.PropertyName.SplitPascalCase();
      }
      var valueFromObject = map.Property.GetValue(inputObj);
      if (valueFromObject != null)
      {
        result.Value = valueFromObject.StringValue();
      }
      result.MultiSelect = map.Property.PropertyType.IsIEnumerable();
      result.InputType = map.Property.PropertyType.IsEnum ||
        (map.HasElementType && map.ElementType!.IsEnum) ||
        result.RelatedEntity != null
        ? EnumInputType.Select : inputAttribute?.InputType ?? EnumInputType.Hidden;
      switch (result.InputType)
      {
        case EnumInputType.Select:
          result.SetSelectItem(map, context);
          break;
      }
      return result;
    }

    private static void SetSelectItem(this StandardProperty input, PropertyInfoMap mapping, IContext? context)
    {
      if (input.RelatedEntity != null)
      {
        if (input.RelatedEntity.IsImplement<ISelect>())
        {
          input.SetEntitySelectItem(context);
        }
        return;
      }
      input.SetEnumSelectItem(mapping);
    }
    private static void SetEntitySelectItem(this StandardProperty input, IContext? context)
    {
      if (context == null || input.RelatedEntity == null)
      {
        return;
      }
      var values = (input.Value ?? "").GetObjValue<string[]>();

      var selectValue = values.Success ? values.Content ?? new string[] { } : new string[] { };
      input.SelectItem = context!.GetKeyValueItemDTO(input.RelatedEntity!).ToList()
          .Select(b =>
          {
            return new KeyValueItemDTO()
            {
              Key = b.Key,
              Value = b.Value,
              Selected = selectValue.Any(k => k == b.Key)
            };
          }).ToArray();
    }
    private static void SetEnumSelectItem(this StandardProperty input, PropertyInfoMap mapping)
    {
      var type = mapping.HasElementType ? mapping.ElementType : mapping.Property.PropertyType;
      if (type?.IsEnum != true)
      {
        return;
      }
      var selectValue = input.Value.ToIEnumerable().ToArray();
      var list = new List<KeyValueItemDTO>();
      var enumValues = Enum.GetValues(type);
      foreach (var e in enumValues)
      {
        var enumValue = e.StringValue(typeof(int));
        var item = new KeyValueItemDTO()
        {
          Key = enumValue,
          Value = (e?.ToString() ?? "").SplitPascalCase(),
          Selected = selectValue.Any(b => b == enumValue)
        };
        list.Add(item);
      }
      input.SelectItem = list.ToArray();
    }



    public static void UpdateProperty(object? input, Dictionary<string, string?> properties)
    {
      if (input == null)
      {
        return;
      }
      var type = input.GetType().ThisType();
      var mapper = type.GetMap();
      var ignoreAttr = typeof(IgnoreUpdateAttribute);
      var noMappingAttr = typeof(IgnoreMappingAttribute);

      foreach (var property in mapper.Properties
        .Where(b => !b.CustomAttributes.Any(c => c.AttributeType == ignoreAttr || c.AttributeType == noMappingAttr)))
      {
        var key = properties.Keys.FirstOrDefault(b => String.Equals(b, property.Name, StringComparison.OrdinalIgnoreCase));
        if (!properties.TryGetValue(key ?? property.Name, out var stringValue))
        {
          continue;
        }
        var objectValue = StringValueHelper.GetObjValue(stringValue, property.PropertyType);
        if (objectValue.Success != true)
        {
          continue;
        }
        property.SetValue(input, objectValue.Content);
      }
    }
  }
}

using Microsoft.EntityFrameworkCore.Diagnostics;

namespace ReheeCmf.ContentManagementModule.EntityHandlers
{
  [EntityChangeTracker<CmsProperty>]
  public class CmsPropertyHandler : EntityChangeHandler<CmsProperty>
  {
    public override async Task<IEnumerable<ValidationResult>> ValidationAsync(CancellationToken ct = default)
    {
      await Task.CompletedTask;
      var result = new List<ValidationResult>();
      if (entity?.CmsPropertyMetadataId.HasValue != true)
      {
        result.Add(ValidationResultHelper.New("CmsPropertyMetadataId is required", nameof(CmsProperty.CmsPropertyMetadataId)));
      }
      if (entity?.CmsEntityId.HasValue != true)
      {
        result.Add(ValidationResultHelper.New("CmsEntityId is required", nameof(CmsProperty.CmsEntityId)));
      }
      if (result.Any())
      {
        return result;
      }
      if (await context!.Query<CmsProperty>(true).AnyAsync(b =>
        b.Id != entity!.Id && b.CmsEntityId == entity!.CmsEntityId && b.CmsPropertyMetadataId == entity!.CmsPropertyMetadataId,
        ct))
      {
        result.Add(ValidationResultHelper.New("Property is duplicate", nameof(CmsProperty.CmsEntityId), nameof(CmsProperty.CmsPropertyMetadataId)));
        return result;
      }
      var entityRelated = entity?.Entity ??
        await context!.Query<CmsEntity>(false).Where(b => b.Id == entity!.CmsEntityId).FirstOrDefaultAsync(ct);
      var propertyMetaRelated = entity?.Property ??
        await context!.Query<CmsPropertyMetadata>(false).Where(b => b.Id == entity!.CmsPropertyMetadataId).FirstOrDefaultAsync(ct);
      if (entityRelated is null || propertyMetaRelated is null || entityRelated.CmsEntityMetadataId != propertyMetaRelated.CmsEntityMetadataId)
      {
        result.Add(ValidationResultHelper.New("Not avaliable", nameof(CmsProperty.CmsEntityId), nameof(CmsProperty.CmsPropertyMetadataId)));
        return result;
      }
      if (propertyMetaRelated.NotNull == true && String.IsNullOrEmpty(entity?.ValueString))
      {
        result.Add(ValidationResultHelper.New("Property is Not Nullable", nameof(CmsProperty.Value)));
        return result;
      }
      if (propertyMetaRelated.Unique == true &&
        await context!.Query<CmsProperty>(true).AnyAsync(b =>
        b.Id != entity!.Id && b.CmsPropertyMetadataId == entity!.CmsPropertyMetadataId && String.Equals(b.ValueString, entity!.ValueString),
        ct))
      {
        result.Add(ValidationResultHelper.New("Property is Unique", nameof(CmsProperty.Value)));
        return result;
      }
      return result;
    }
    public override async Task BeforeCreateAsync(CancellationToken ct = default)
    {
      await base.BeforeCreateAsync(ct);
      entity!.Property ??=
        await context!.Query<CmsPropertyMetadata>(false).Where(b => b.Id == entity!.CmsPropertyMetadataId).FirstOrDefaultAsync(ct);
      entity!.PropertyName = entity?.Property?.PropertyName;
      entity!.PropertyType = entity?.Property?.PropertyType;
      if (entity?.ValueSingle == null)
      {
        entity.UpdatePropertyValue(false);
      }
      else
      {
        entity.UpdatePropertyValue(true);
      }

    }
    public override async Task BeforeUpdateAsync(EntityChanges[] propertyChange, CancellationToken ct = default)
    {
      await base.BeforeUpdateAsync(propertyChange, ct);
      entity!.Property ??=
        await context!.Query<CmsPropertyMetadata>(false).Where(b => b.Id == entity!.CmsPropertyMetadataId).FirstOrDefaultAsync(ct);
      var changedNames = propertyChange.Select(b => b.PropertyName).ToArray();
      string[] valueChange = [
        nameof(CmsProperty.ValueBoolean),
        nameof(CmsProperty.ValueInt16),
        nameof(CmsProperty.ValueInt32),
        nameof(CmsProperty.ValueInt64),
        nameof(CmsProperty.ValueSingle),
        nameof(CmsProperty.ValueDouble),
        nameof(CmsProperty.ValueDecimal),
        nameof(CmsProperty.ValueGuid),
        nameof(CmsProperty.ValueDateTime),
        nameof(CmsProperty.ValueDateTimeOffset)
        ];
      string[] typeOrStringChange = [
        nameof(CmsProperty.ValueString),
        nameof(CmsProperty.PropertyType)
      ];
      if (changedNames.Intersect(valueChange).Any() == true)
      {
        entity.UpdatePropertyValue(true);
      }
      else if (changedNames.Intersect(typeOrStringChange).Any() == true)
      {
        entity.UpdatePropertyValue(false);
      }
    }
  }

  public static class CmsPropertyHandlerHelper
  {
    public static void UpdatePropertyValue(this CmsProperty? entity, bool valueFromField)
    {
      var propertyMetaRelated = entity?.Property;
      if (propertyMetaRelated == null)
      {
        return;
      }
      switch (propertyMetaRelated.PropertyType)
      {
        case Enums.EnumPropertyType.Boolean:
          if (valueFromField)
            entity!.ValueString = entity?.ValueBoolean?.StringValue();
          else
            entity!.ValueBoolean = entity?.ValueString?.GetValue<Boolean?>();
          entity!.ValueInt16 = null;
          entity!.ValueInt32 = null;
          entity!.ValueInt64 = null;
          entity!.ValueSingle = null;
          entity!.ValueDouble = null;
          entity!.ValueDecimal = null;
          entity!.ValueGuid = null;
          entity!.ValueDateTime = null;
          entity!.ValueDateTimeOffset = null;
          break;
        case Enums.EnumPropertyType.Int16:
          if (valueFromField)
            entity!.ValueString = entity?.ValueInt16?.StringValue();
          else
            entity!.ValueInt16 = entity?.ValueString?.GetValue<Int16?>();
          entity!.ValueBoolean = null;
          entity!.ValueInt32 = null;
          entity!.ValueInt64 = null;
          entity!.ValueSingle = null;
          entity!.ValueDouble = null;
          entity!.ValueDecimal = null;
          entity!.ValueGuid = null;
          entity!.ValueDateTime = null;
          entity!.ValueDateTimeOffset = null;
          break;
        case Enums.EnumPropertyType.Int32:
          if (valueFromField)
            entity!.ValueString = entity?.ValueInt32?.StringValue();
          else
            entity!.ValueInt32 = entity?.ValueString?.GetValue<Int32?>();
          entity!.ValueBoolean = null;
          entity!.ValueInt16 = null;
          entity!.ValueInt64 = null;
          entity!.ValueSingle = null;
          entity!.ValueDouble = null;
          entity!.ValueDecimal = null;
          entity!.ValueGuid = null;
          entity!.ValueDateTime = null;
          entity!.ValueDateTimeOffset = null;
          break;
        case Enums.EnumPropertyType.Int64:
          if (valueFromField)
            entity!.ValueString = entity?.ValueInt64?.StringValue();
          else
            entity!.ValueInt64 = entity?.ValueString?.GetValue<Int64?>();
          entity!.ValueBoolean = null;
          entity!.ValueInt16 = null;
          entity!.ValueInt32 = null;
          entity!.ValueSingle = null;
          entity!.ValueDouble = null;
          entity!.ValueDecimal = null;
          entity!.ValueGuid = null;
          entity!.ValueDateTime = null;
          entity!.ValueDateTimeOffset = null;
          break;
        case Enums.EnumPropertyType.Single:
          if (valueFromField)
            entity!.ValueString = entity?.ValueSingle?.StringValue();
          else
            entity!.ValueSingle = entity?.ValueString?.GetValue<Single?>();
          entity!.ValueBoolean = null;
          entity!.ValueInt16 = null;
          entity!.ValueInt32 = null;
          entity!.ValueInt64 = null;
          entity!.ValueDouble = null;
          entity!.ValueDecimal = null;
          entity!.ValueGuid = null;
          entity!.ValueDateTime = null;
          entity!.ValueDateTimeOffset = null;
          break;
        case Enums.EnumPropertyType.Double:
          if (valueFromField)
            entity!.ValueString = entity?.ValueDouble?.StringValue();
          else
            entity!.ValueDouble = entity?.ValueString?.GetValue<Double?>();
          entity!.ValueBoolean = null;
          entity!.ValueInt16 = null;
          entity!.ValueInt32 = null;
          entity!.ValueInt64 = null;
          entity!.ValueSingle = null;
          entity!.ValueDecimal = null;
          entity!.ValueGuid = null;
          entity!.ValueDateTime = null;
          entity!.ValueDateTimeOffset = null;
          break;
        case Enums.EnumPropertyType.Decimal:
          if (valueFromField)
            entity!.ValueString = entity?.ValueDecimal?.StringValue();
          else
            entity!.ValueDecimal = entity?.ValueString?.GetValue<Decimal?>();
          entity!.ValueBoolean = null;
          entity!.ValueInt16 = null;
          entity!.ValueInt32 = null;
          entity!.ValueInt64 = null;
          entity!.ValueSingle = null;
          entity!.ValueDouble = null;
          entity!.ValueGuid = null;
          entity!.ValueDateTime = null;
          entity!.ValueDateTimeOffset = null;
          break;
        case Enums.EnumPropertyType.Guid:
          if (valueFromField)
            entity!.ValueString = entity?.ValueGuid?.StringValue();
          else
            entity!.ValueGuid = entity?.ValueString?.GetValue<Guid?>();
          entity!.ValueBoolean = null;
          entity!.ValueInt16 = null;
          entity!.ValueInt32 = null;
          entity!.ValueInt64 = null;
          entity!.ValueSingle = null;
          entity!.ValueDouble = null;
          entity!.ValueDecimal = null;
          entity!.ValueDateTime = null;
          entity!.ValueDateTimeOffset = null;
          break;
        case Enums.EnumPropertyType.DateTime:
          if (valueFromField)
            entity!.ValueString = entity?.ValueDateTime?.StringValue();
          else
            entity!.ValueDateTime = entity?.ValueString?.GetValue<DateTime?>();
          entity!.ValueBoolean = null;
          entity!.ValueInt16 = null;
          entity!.ValueInt32 = null;
          entity!.ValueInt64 = null;
          entity!.ValueSingle = null;
          entity!.ValueDouble = null;
          entity!.ValueDecimal = null;
          entity!.ValueGuid = null;
          entity!.ValueDateTimeOffset = null;
          break;
        case Enums.EnumPropertyType.DateTimeOffset:
          if (valueFromField)
            entity!.ValueString = entity?.ValueDateTimeOffset?.StringValue();
          else
            entity!.ValueDateTimeOffset = entity?.ValueString?.GetValue<DateTimeOffset?>();
          entity!.ValueBoolean = null;
          entity!.ValueInt16 = null;
          entity!.ValueInt32 = null;
          entity!.ValueInt64 = null;
          entity!.ValueSingle = null;
          entity!.ValueDouble = null;
          entity!.ValueDecimal = null;
          entity!.ValueGuid = null;
          entity!.ValueDateTime = null;
          break;
        case Enums.EnumPropertyType.String:
          entity!.ValueBoolean = null;
          entity!.ValueInt16 = null;
          entity!.ValueInt32 = null;
          entity!.ValueInt64 = null;
          entity!.ValueSingle = null;
          entity!.ValueDouble = null;
          entity!.ValueDecimal = null;
          entity!.ValueGuid = null;
          entity!.ValueDateTime = null;
          entity!.ValueDateTimeOffset = null;
          break;
        default:
          break;
      }
    }
  }
}

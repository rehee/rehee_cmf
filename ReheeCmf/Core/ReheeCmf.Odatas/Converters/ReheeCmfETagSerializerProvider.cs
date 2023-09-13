using Microsoft.AspNetCore.OData.Formatter.Serialization;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OData.Edm;
using Microsoft.OData;
using ReheeCmf.Commons;
using ReheeCmf.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.OData.Formatter.Value;

namespace ReheeCmf.ODatas.Converters
{
  public class ReheeCmfETagSerializerProvider : ODataSerializerProvider
  {
    private readonly IServiceProvider serviceProvider;

    public ReheeCmfETagSerializerProvider(IServiceProvider serviceProvider) : base(serviceProvider)
    {
      this.serviceProvider = serviceProvider;
    }
    public override IODataEdmTypeSerializer GetEdmTypeSerializer(IEdmTypeReference edmType)
    {
      if (edmType == null)
      {
        throw new ArgumentNullException(nameof(edmType));
      }

      switch (edmType.TypeKind())
      {
        case EdmTypeKind.Enum:
          return serviceProvider.GetRequiredService<ODataEnumSerializer>();

        case EdmTypeKind.Primitive:
          return serviceProvider.GetRequiredService<ODataPrimitiveSerializer>();

        case EdmTypeKind.Collection:
          IEdmCollectionTypeReference collectionType = edmType.AsCollection();
          if (collectionType.Definition.IsDeltaResourceSet())
          {
            return serviceProvider.GetRequiredService<ODataDeltaResourceSetSerializer>();
          }
          else if (collectionType.ElementType().IsEntity() || collectionType.ElementType().IsComplex())
          {
            return serviceProvider.GetRequiredService<ODataResourceSetSerializer>();
          }
          else
          {
            return serviceProvider.GetRequiredService<ODataCollectionSerializer>();
          }

        case EdmTypeKind.Complex:
        case EdmTypeKind.Entity:
          return new ETagResourceSerializer(this);

        default:
          return null;
      }
    }
  }
  public class ETagResourceSerializer : ODataResourceSerializer
  {
    public ETagResourceSerializer(IODataSerializerProvider serializerProvider)
        : base(serializerProvider)
    { }

    public override ODataResource CreateResource(SelectExpandNode selectExpandNode, ResourceContext resourceContext)
    {
      ODataResource resource = base.CreateResource(selectExpandNode, resourceContext);
      if (!resourceContext.EdmObject.TryGetPropertyValue(nameof(IWithEtag.ETag), out var eTag))
      {
        return resource;
      }
      if (eTag is byte[] obj)
      {
        resource.ETag = obj.EncodeETagString();
      }
      return resource;
    }


  }
}

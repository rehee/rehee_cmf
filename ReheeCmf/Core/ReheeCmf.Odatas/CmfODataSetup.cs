using Microsoft.AspNetCore.OData;
using Microsoft.AspNetCore.OData.Formatter.Serialization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OData;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using ReheeCmf.Modules;
using ReheeCmf.ODatas.Commons;
using ReheeCmf.ODatas.Conventions;
using ReheeCmf.ODatas.Converters;
using ReheeCmf.ODatas.Helpers;

namespace ReheeCmf.ODatas
{
  public static class CmfODataSetup
  {
    public static IMvcBuilder AddCmfOData(this IMvcBuilder builder, (string, Action<ODataConventionModelBuilder>)[]? additionalOData = null)
    {
      builder.AddOData((opt, sp) =>
      {
        opt.Count().Filter().Expand().Select().OrderBy().SetMaxTop(null);
      });
      if (additionalOData?.Length == 0)
      {
        foreach (var ar in additionalOData)
        {
          builder.AddOData((opt, sp) =>
          {
            opt.AddRouteComponents(ar.Item1, ar.Item2.GetEdmModel(), action =>
            {
              action.AddSingleton<ODataPayloadValueConverter, ReheeCMFOdataConverter>();
              action.AddSingleton<IODataSerializerProvider, ReheeCmfETagSerializerProvider>();

            });
            opt.Conventions.Add(new MyConvention());
          });
          
        }
      }
      return builder;
    }

    public static IMvcBuilder AddCmfOdataEndpoint(this IMvcBuilder mvc, Func<IServiceProvider, IEdmModel> model,
      string endpoint, IEnumerable<ODataEndpointMapping> mapping)
    {
      mvc.AddOData((opt, sp) =>
      {
        opt.AddRouteComponents(endpoint, model(sp), action =>
        {
          action.AddSingleton<ODataPayloadValueConverter, ReheeCMFOdataConverter>();
          action.AddSingleton<IODataSerializerProvider, ReheeCmfETagSerializerProvider>();

        });
        foreach (var m in mapping)
        {
          opt.Conventions.Add(ODataControllerActionConventionHelper.New(endpoint, m.Controller, m.Action, m.Path, m.EntityKey, m.EntityName));
        }

      });
      return mvc;
    }
  }
}

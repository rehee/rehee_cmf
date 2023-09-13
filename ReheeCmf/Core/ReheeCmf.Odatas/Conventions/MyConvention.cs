using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Extensions;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Conventions;
using Microsoft.AspNetCore.OData.Routing.Template;
using ReheeCmf.Commons;
using ReheeCmf.ODatas.Attributes;
using ReheeCmf.ODatas.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReheeCmf.ODatas.Conventions
{
  public class MyConvention : IODataControllerActionConvention
  {
    /// <summary>
    /// Order value.
    /// </summary>
    public virtual int Order => 0;

    /// <summary>
    /// Apply to action,.
    /// </summary>
    /// <param name="context">Http context.</param>
    /// <returns>true/false</returns>
    public bool AppliesToAction(ODataControllerActionContext context)
    {
      return false;
    }

    /// <summary>
    /// Apply to controller
    /// </summary>
    /// <param name="context">Http context.</param>
    /// <returns>true/false</returns>
    public bool AppliesToController(ODataControllerActionContext context)
    {
      if (context.Prefix == CrudOption.EntityQueryEndpoint || context.Prefix == CrudOption.UserManagementApiEndpoint)
      {
        return true;
      }
      var api = context.Controller.GetAttribute<ApiControllerAttribute>();
      if (api == null)
      {
        return true;
      }
      var route = context.Controller.GetAttribute<RouteAttribute>();
      if (route == null)
      {
        return true;
      }
      if (!route.Template.Equals(context.Prefix, StringComparison.OrdinalIgnoreCase))
      {
        return true;
      }
      var prex = route.Template;
      var actions = context.Controller.Actions
        .Select(b =>
        {

          EnableQueryAttribute? enable = null;
          OdataSetAttribute? dataset = null;
          HttpGetAttribute? get = null;
          foreach (var a in b.Attributes)
          {
            if (a is EnableQueryAttribute a1)
            {
              enable = a1;
            }
            if (a is OdataSetAttribute a2)
            {
              dataset = a2;
            }
            if (a is HttpGetAttribute a3)
            {
              get = a3;
            }
          }
          return (
        action: b,
        enable: enable,
        dataset: dataset,
        get: get);
        }).Where(b => b.get != null && b.enable != null && b.dataset != null).ToArray();
      if (actions?.Any() != true)
      {
        return true;
      }
      foreach (var action in actions)
      {
        ODataPathTemplate path = new ODataPathTemplate(
              new EntityNameTemplateSegment(action.dataset!.DataSet, action.get!.Template!));
        action.action.AddSelector("Get", prex, context.Model, path);
        return true;
      }

      return true;
    }
  }
}

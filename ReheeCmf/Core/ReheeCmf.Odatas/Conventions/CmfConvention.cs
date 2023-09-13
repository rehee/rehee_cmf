using Microsoft.AspNetCore.OData.Extensions;
using Microsoft.AspNetCore.OData.Routing.Conventions;
using Microsoft.AspNetCore.OData.Routing.Template;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReheeCmf.ODatas.Conventions
{
  public class CmfConvention : IODataControllerActionConvention
  {
    public CmfConvention(string prefix, string controllerName, string actionName, ODataPathTemplate path)
    {
      Prefix = prefix;
      ControllerName = controllerName;
      ActionName = actionName;
      this.path = path;
    }

    public string Prefix { get; private set; }
    public string ControllerName { get; private set; }
    public string ActionName { get; private set; }
    public ODataPathTemplate path { get; private set; }
    public virtual int Order => 0;
    public virtual bool AppliesToAction(ODataControllerActionContext context)
    {
      return false;
    }
    public virtual bool AppliesToController(ODataControllerActionContext context)
    {
      if (!(context.Controller.ControllerType.Name == ControllerName && context.Prefix == Prefix))
      {
        return false;
      }

      if (ActionName == null)
      {
        if (context.Action != null)
        {
          context.Action.AddSelector("Get", context.Prefix, context.Model, path);
        }
        else
        {
          var action = context.Controller.Actions;
          context.Controller.Actions.FirstOrDefault().AddSelector("Get", context.Prefix, context.Model, path);
        }
        return true;
      }

      var actionFind = context.Controller.Actions.FirstOrDefault(b => b.ActionName == ActionName);
      if (actionFind == null)
      {
        return false;
      }
      actionFind.AddSelector("Get", context.Prefix, context.Model, path);
      return true;
    }
  }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using ReheeCmf.ContextModule.Entities;
using System.Linq.Expressions;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using ReheeCmf.ContentManagementModule.DTOs;
namespace ReheeCmf.ContentManagementModule.Controllers
{
  [Route("Api/Data/CmfContentDTO")]
  public class ContentController : ODataController
  {
    private readonly IContext context;
    static ScriptOptions? options { get; set; }
    public ContentController(IContext context)
    {
      this.context = context;
    }
    [EnableQuery]
    [HttpGet]
    public async Task<IEnumerable<CmfContentDTO>> Query(CancellationToken ct)
    {

      var code = "x => !x.EmailConfirmed";
      options = options ?? ScriptOptions.Default
        .WithReferences(AppDomain.CurrentDomain.GetAssemblies().Where(a => !a.IsDynamic))
        .WithImports("System", "System.Linq", "System.Linq.Expressions");

      var d = await CSharpScript.EvaluateAsync(
        $$"""
        global::System.Linq.Expressions.Expression<System.Func<ReheeCmf.ContextModule.Entities.ReheeCmfBaseUser, System.Boolean>> exp = {{code}};
        return exp;
        """
        , options, cancellationToken: ct);

      var result = d as Expression<Func<ReheeCmfBaseUser, System.Boolean>>;


      var query = context.Query<ReheeCmfBaseUser>(true).Where(result);
      //context.Query<ReheeCmfBaseUser>(true).Where();
      return
        from e in context.Query<ReheeCmfBaseUser>(true).Where(result)
        let e2g = context.Query<ReheeCmfBaseUser>(true).Where(b => b.Id == e.Id).AsEnumerable()
        select new CmfContentDTO
        {
          Id = e.Id,
          Data = new Dictionary<string, object?>(
            e2g.Select(kv => new KeyValuePair<string, object?>("UserName", kv.EmailConfirmed)).AsEnumerable())
        };
    }
  }
}

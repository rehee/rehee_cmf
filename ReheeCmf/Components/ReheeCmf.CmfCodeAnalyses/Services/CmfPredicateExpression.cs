using Microsoft.CodeAnalysis.CSharp.Scripting;
using ReheeCmf.CodeAnalyses;
using ReheeCmf.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace ReheeCmf.CmfCodeAnalyses.Services
{
  public class CmfPredicateExpression<T> : ICmfPredicateExpression<T>
  {
    private readonly CmfCodeAnalysisOption<T> options;

    public CmfPredicateExpression(CmfCodeAnalysisOption<T> options)
    {
      this.options = options;
    }
    public async Task<Func<TParam?, Expression<Func<TResult, bool>>>?> TypedEvaluateAsync<TParam, TResult>(CancellationToken cancellationToken,
      string lambdaCode, params string[] namespaceUsing)
    {
      string[] nameSpace = [typeof(TParam).Namespace, typeof(TResult).Namespace];
      if (namespaceUsing?.Any() == true)
      {
        nameSpace = nameSpace.Concat(namespaceUsing).ToArray();
      }
      var combineNameSpace = String.Join("", nameSpace.Select(b => $"using {b};"));
      var mapper = new Dictionary<string, string>()
      {
        [ConstCodeAnalysisKey.NamespaceUsing] = combineNameSpace,
        [ConstCodeAnalysisKey.PredicateSource] = typeof(TResult).Name,
        [ConstCodeAnalysisKey.PredicateParam1] = typeof(TParam).Name,
        [ConstCodeAnalysisKey.LambdaCode] = lambdaCode,
      };
      var code = options.Template.Formate(mapper);
      var result = await CSharpScript.EvaluateAsync(code, CmfCodeAnalysis.Option, cancellationToken: cancellationToken);
      if (result is Func<TParam?, Expression<Func<TResult, bool>>> typedResult)
      {
        return typedResult;
      }
      return default;
    }
  }
}

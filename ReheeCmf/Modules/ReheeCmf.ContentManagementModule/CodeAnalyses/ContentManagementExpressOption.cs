using ReheeCmf.CodeAnalyses;

namespace ReheeCmf.ContentManagementModule.CodeAnalyses
{
  public class ContentManagementExpressOption
  {
    public const string QueryPredicateLambdaTemplate =
      $$"""
      [{{ConstCodeAnalysisKey.NamespaceUsing}}]
      Func<[{{ConstCodeAnalysisKey.PredicateParam1}}]?, Expression<Func<[{{ConstCodeAnalysisKey.PredicateSource}}], Boolean>>> lambdaFunc = user => 
       [{{ConstCodeAnalysisKey.LambdaCode}}];
      return lambdaFunc;
      """;
  }
}

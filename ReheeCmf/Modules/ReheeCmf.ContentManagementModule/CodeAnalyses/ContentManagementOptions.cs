using ReheeCmf.CodeAnalyses;

namespace ReheeCmf.ContentManagementModule.CodeAnalyses
{
  public class ContentManagementOptions
  {
    public const string QueryPredicateLambdaTemplate =
      $$"""
      using ReheeCmf.Commons.DTOs;
      [{{ConstCodeAnalysisKey.NamespaceUsing}}]
      Func<TokenDTO?, Expression<Func<[{{ConstCodeAnalysisKey.PredicateSource}}], Boolean>>> lambdaFunc = user => 
       [{{ConstCodeAnalysisKey.LambdaCode}}];
      return lambdaFunc;
      """;
  }
}

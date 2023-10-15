using Microsoft.CodeAnalysis.Scripting;
using Microsoft.Extensions.DependencyInjection;
using ReheeCmf.CmfCodeAnalyses.Services;
using ReheeCmf.CodeAnalyses;

namespace ReheeCmf.CmfCodeAnalyses
{
  public static class CmfCodeAnalysis
  {
    private static object LockObj = new object();
    public static ScriptOptions? Option { get; set; }
    private static void SetOption()
    {
      if (Option == null)
      {
        lock (LockObj)
        {
          Option = ScriptOptions.Default
            .WithReferences(AppDomain.CurrentDomain.GetAssemblies().Where(a => !a.IsDynamic))
            .WithImports("System", "System.Linq", "System.Linq.Expressions");
        }
      }
    }
    public static IServiceCollection AddCmfCodeAnalysis<T>(this IServiceCollection services, Func<IServiceProvider, CmfCodeAnalysisOption<T>> getOptions)
    {
      SetOption();
      services.AddSingleton<CmfCodeAnalysisOption<T>>(getOptions);
      services.AddSingleton<ICmfCodeAnalysis<T>>(sp => new CmfCodeAnalysisService<T>(sp.GetService<CmfCodeAnalysisOption<T>>()!));
      return services;
    }
    public static IServiceCollection AddCmfPredicateExpression<T>(this IServiceCollection services, Func<IServiceProvider, CmfCodeAnalysisOption<T>> getOptions)
    {
      SetOption();
      services.AddSingleton<CmfCodeAnalysisOption<T>>(getOptions);
      services.AddSingleton<ICmfPredicateExpression<T>>(sp => new CmfPredicateExpression<T>(sp.GetService<CmfCodeAnalysisOption<T>>()!));
      return services;
    }
  }
}

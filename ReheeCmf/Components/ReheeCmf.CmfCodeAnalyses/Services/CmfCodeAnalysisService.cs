using Microsoft.CodeAnalysis.CSharp.Scripting;
using ReheeCmf.CodeAnalyses;
using ReheeCmf.Helpers;

namespace ReheeCmf.CmfCodeAnalyses.Services
{
  public class CmfCodeAnalysisService<T> : ICmfCodeAnalysis<T>
  {

    private readonly CmfCodeAnalysisOption<T> configOption;

    public CmfCodeAnalysisService(CmfCodeAnalysisOption<T> configOption)
    {
      this.configOption = configOption;
    }
    public virtual async Task<object> EvaluateAsync(CancellationToken cancellationToken, params (string key, string value)[] values)
    {
      var mapper = values.ToDictionary();
      var codes = configOption.Template.Formate(mapper);
      return await CSharpScript.EvaluateAsync(codes, CmfCodeAnalysis.Option, cancellationToken: cancellationToken);
    }

    public async Task<TResult?> TypedEvaluateAsync<TResult>(CancellationToken cancellationToken, params (string key, string value)[] values)
    {
      var result = await EvaluateAsync(cancellationToken, values);
      if (result is TResult resultT)
      {
        return resultT;
      }
      return default;
    }
  }
}

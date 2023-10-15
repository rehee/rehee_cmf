using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ReheeCmf.CodeAnalyses
{
  public interface ICmfCodeAnalysis
  {
    Task<object> EvaluateAsync(CancellationToken cancellationToken, params (string key, string value)[] values);
    Task<TResult?> TypedEvaluateAsync<TResult>(CancellationToken cancellationToken, params (string key, string value)[] values);
  }
  public interface ICmfCodeAnalysis<T> : ICmfCodeAnalysis
  {

  }

  public interface ICmfPredicateExpression
  {
    Task<Func<TParam?, Expression<Func<TResult, bool>>>?> TypedEvaluateAsync<TParam, TResult>(CancellationToken cancellationToken,
      string lambdaCode, params string[] namespaceUsing);
  }
  public interface ICmfPredicateExpression<T> : ICmfPredicateExpression
  {

  }
}

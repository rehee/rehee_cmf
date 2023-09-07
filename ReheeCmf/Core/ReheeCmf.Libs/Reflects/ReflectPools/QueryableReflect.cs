using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ReheeCmf.Reflects.ReflectPools
{
  public static class QueryableReflect
  {
    public static IEnumerable<TSource> CmfWhere<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, bool>> predicate)
    {
      return source.Where(predicate);
    }
    public static bool CmfFind<TSource>(this TSource source, Func<TSource, bool> predicate)
    {
      return predicate(source);
    }
    public static IEnumerable<T> WhereCheck<T>(this IEnumerable<T> query, TokenDTO user)
    {
      var readCheck = ReflectPool.GetReadCheck<T>();
      if (readCheck == null)
      {
        return query;
      }
      return query.Where(readCheck(user).Compile());
    }
    public static IQueryable<T> WhereCheck<T>(this IQueryable<T> query, TokenDTO user)
    {
      var readCheck = ReflectPool.GetReadCheck<T>();
      if (readCheck == null)
      {
        return query;
      }
      return query.Where(readCheck(user));
    }
    public static bool FindCheck<T>(this T obj, TokenDTO user)
    {
      var findCheck = ReflectPool.GetFindCheck<T>();
      if (findCheck == null)
      {
        return true;
      }
      return findCheck(user)(obj);
    }
  }
}

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ReheeCmf.Reflects.ReflectPools
{
  public static partial class ReflectPool
  {
    private static MethodInfo where = typeof(QueryableReflect).GetMethod(nameof(QueryableReflect.CmfWhere));
    public static MethodInfo Where(Type type)
    {
      return where.MakeGenericMethod(type);
    }

    private static MethodInfo find = typeof(QueryableReflect).GetMethod(nameof(QueryableReflect.CmfFind));
    public static MethodInfo Find(Type type)
    {
      return find.MakeGenericMethod(type);
    }

    public static ConcurrentDictionary<Type, EntityReadCheckInPool> EntityQueryReadCheck { get; set; }
      = new ConcurrentDictionary<Type, EntityReadCheckInPool>();
    public static ConcurrentDictionary<Type, EntityReadCheckInPool> EntityQueryFindCheck { get; set; }
    = new ConcurrentDictionary<Type, EntityReadCheckInPool>();

    public static ConcurrentDictionary<Type, object> EntityQueryReadCheckObj { get; set; }
      = new ConcurrentDictionary<Type, object>();
    public static ConcurrentDictionary<Type, object> EntityQueryFindCheckObj { get; set; }
    = new ConcurrentDictionary<Type, object>();

    public static void SetQueryAndFindCheck(Type type)
    {
      var readCheck = type.GetMap().Properties.FirstOrDefault(b => b.HasCustomAttribute<ReadCheckAttribute>());
      EntityReadCheckInPool funcRead = null;
      object funcReadObj = null;
      if (readCheck != null)
      {
        funcReadObj = readCheck.GetValue(null);
        funcRead = (user) => readCheck.PropertyType.GetMethod("Invoke").Invoke(readCheck.GetValue(null), new object[] { user });
      }
      EntityQueryReadCheck.AddOrUpdate(type, funcRead, (s, f) => funcRead);
      EntityQueryReadCheckObj.AddOrUpdate(type, funcReadObj, (s, f) => funcReadObj);

      var findCheck = type.GetMap().Properties.FirstOrDefault(b => b.HasCustomAttribute<FindCheckAttribute>());
      object findCheckObj = null;
      EntityReadCheckInPool funcFind = null;
      if (findCheck != null)
      {
        funcFind = (user) => findCheck.PropertyType.GetMethod("Invoke").Invoke(findCheck.GetValue(null), new object[] { user });
        findCheckObj = findCheck.GetValue(null);
      }
      EntityQueryFindCheck.AddOrUpdate(type, funcFind, (s, f) => funcFind);
      EntityQueryFindCheckObj.AddOrUpdate(type, findCheckObj, (s, f) => findCheckObj);
    }



    public static object ContentReadCheck(Type type, object entitySet, TokenDTO user)
    {
      if (!EntityQueryReadCheck.TryGetValue(type, out var check) || check == null)
      {
        return entitySet;
      }
      return Where(type).Invoke(null, new object[] { entitySet, check(user) });
    }
    public static bool ContentFindCheck(Type type, object entitySet, TokenDTO user)
    {
      if (!EntityQueryFindCheck.TryGetValue(type, out var check) || check == null)
      {
        return true;
      }
      try
      {
        if (Find(type).Invoke(null, new object[] { entitySet, check(user) }) is bool result)
        {
          return result;
        }
        return false;
      }
      catch (Exception ex)
      {
        ex.Throw();
        return false;
      }
    }

    public static ReadCheck<T>? GetReadCheck<T>()
    {
      var type = typeof(T);
      if (ReflectPool.EntityQueryReadCheckObj.TryGetValue(typeof(T), out var check) && check is ReadCheck<T> checkObj)
      {
        return checkObj;
      }
      return null;
    }
    public static FindCheck<T>? GetFindCheck<T>()
    {
      var type = typeof(T);
      if (ReflectPool.EntityQueryFindCheckObj.TryGetValue(typeof(T), out var check) && check is FindCheck<T> findObj)
      {
        return findObj;
      }
      return null;
    }
  }

  public delegate object EntityReadCheckInPool(TokenDTO user);
}

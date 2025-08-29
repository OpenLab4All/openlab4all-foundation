using System;
using System.Collections.Concurrent;
using System.Data;
using System.Linq.Expressions;
using System.Reflection;

namespace OpenLab4All.Foundation.Data.Mapping
{
  internal static class DataRowActivatorCache
  {
    private static readonly ConcurrentDictionary<Type, Delegate> _cache = new ConcurrentDictionary<Type, Delegate>();

    public static Func<DataRow, T> GetOrCreate<T>()
    {
      var type = typeof(T);
      if (_cache.TryGetValue(type, out var existing))
        return (Func<DataRow, T>)existing;

      var ctor = type.GetConstructor(BindingFlags.Public | BindingFlags.Instance, null, new[] { typeof(DataRow) }, null);
      if (ctor == null)
        throw new MissingMethodException(type.FullName, $"{type.Name}(System.Data.DataRow)");

      var param = Expression.Parameter(typeof(DataRow), "dr");
      var newExpr = Expression.New(ctor, param);
      var lambda = Expression.Lambda<Func<DataRow, T>>(newExpr, param).Compile();

      _cache[type] = lambda;
      return lambda;
    }
  }
}

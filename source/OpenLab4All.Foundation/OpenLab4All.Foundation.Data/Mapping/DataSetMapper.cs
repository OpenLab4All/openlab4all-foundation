using System;
using System.Data;
using System.Linq;
using System.Collections.Generic;

namespace OpenLab4All.Foundation.Data.Mapping
{
  public static class DataSetMapper
  {
    public static T MapSingle<T>(DataSet ds, int tableIndex = 0)
    {
      var table = GetTableOrThrow(ds, tableIndex);
      if (table.Rows.Count == 0)
        throw new InvalidOperationException($"La tabla en índice {tableIndex} no contiene filas.");

      var activator = DataRowActivatorCache.GetOrCreate<T>();
      return activator(table.Rows[0]);
    }

    public static T MapOptional<T>(DataSet ds, int tableIndex = 0)
    {
      var table = GetTableOrThrow(ds, tableIndex);
      if (table.Rows.Count == 0)
        return default(T);

      var activator = DataRowActivatorCache.GetOrCreate<T>();
      return activator(table.Rows[0]);
    }

    public static List<T> MapList<T>(DataSet ds, int tableIndex = 0)
    {
      var table = GetTableOrThrow(ds, tableIndex);
      var activator = DataRowActivatorCache.GetOrCreate<T>();

      var list = new List<T>(capacity: Math.Max(0, table.Rows.Count));
      foreach (DataRow dr in table.Rows)
        list.Add(activator(dr));

      return list;
    }

    public static T MapSingle<T>(DataSet ds, string tableName)
    {
      var table = GetTableOrThrow(ds, tableName);
      if (table.Rows.Count == 0)
        throw new InvalidOperationException($"La tabla '{tableName}' no contiene filas.");

      var activator = DataRowActivatorCache.GetOrCreate<T>();
      return activator(table.Rows[0]);
    }

    public static T MapOptional<T>(DataSet ds, string tableName)
    {
      var table = GetTableOrThrow(ds, tableName);
      if (table.Rows.Count == 0)
        return default(T);

      var activator = DataRowActivatorCache.GetOrCreate<T>();
      return activator(table.Rows[0]);
    }

    public static List<T> MapList<T>(DataSet ds, string tableName)
    {
      var table = GetTableOrThrow(ds, tableName);
      var activator = DataRowActivatorCache.GetOrCreate<T>();

      var list = new List<T>(capacity: Math.Max(0, table.Rows.Count));
      foreach (DataRow dr in table.Rows)
        list.Add(activator(dr));

      return list;
    }

    public static T MapSingle<T>(DataTable table)
    {
      if (table == null)
        throw new ArgumentNullException(nameof(table), "La DataTable es nula.");
      if (table.Rows.Count == 0)
        throw new InvalidOperationException("La DataTable no contiene filas.");

      var activator = DataRowActivatorCache.GetOrCreate<T>();
      return activator(table.Rows[0]);
    }

    public static T MapOptional<T>(DataTable table)
    {
      if (table == null)
        throw new ArgumentNullException(nameof(table), "La DataTable es nula.");
      if (table.Rows.Count == 0)
        return default(T);

      var activator = DataRowActivatorCache.GetOrCreate<T>();
      return activator(table.Rows[0]);
    }

    public static List<T> MapList<T>(DataTable table)
    {
      if (table == null)
        throw new ArgumentNullException(nameof(table), "La DataTable es nula.");

      var activator = DataRowActivatorCache.GetOrCreate<T>();
      var list = new List<T>(capacity: Math.Max(0, table.Rows.Count));
      foreach (DataRow dr in table.Rows)
        list.Add(activator(dr));

      return list;
    }

    private static DataTable GetTableOrThrow(DataSet ds, int tableIndex)
    {
      if (ds == null)
        throw new ArgumentNullException(nameof(ds), "El DataSet es nulo.");
      if (tableIndex < 0 || tableIndex >= ds.Tables.Count)
        throw new IndexOutOfRangeException($"El DataSet no contiene la tabla en índice {tableIndex}. Tablas disponibles: {ds.Tables.Count}.");

      var table = ds.Tables[tableIndex];
      if (table == null)
        throw new InvalidOperationException($"La tabla en índice {tableIndex} es nula.");

      return table;
    }

    private static DataTable GetTableOrThrow(DataSet ds, string tableName)
    {
      if (ds == null)
        throw new ArgumentNullException(nameof(ds), "El DataSet es nulo.");
      if (string.IsNullOrWhiteSpace(tableName))
        throw new ArgumentException("El nombre de tabla es nulo o vacío.", nameof(tableName));

      var table = ds.Tables.Contains(tableName)
        ? ds.Tables[tableName]
        : ds.Tables.Cast<DataTable>().FirstOrDefault(t => string.Equals(t.TableName, tableName, StringComparison.OrdinalIgnoreCase));

      if (table == null)
        throw new InvalidOperationException($"El DataSet no contiene una tabla llamada '{tableName}'. Tablas: {string.Join(", ", ds.Tables.Cast<DataTable>().Select(t => t.TableName))}");

      return table;
    }
  }
}

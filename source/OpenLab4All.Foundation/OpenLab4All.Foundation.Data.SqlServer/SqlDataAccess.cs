using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using OpenLab4All.Foundation.Core;
using OpenLab4All.Foundation.Data.Abstractions;

namespace OpenLab4All.Foundation.Data.SqlServer
{
  public sealed class SqlDataAccess : IDataAccess
  {
    private readonly string _connectionString;
    private readonly int _defaultTimeoutSeconds;

    public SqlDataAccess(string connectionString, int commandTimeoutSeconds = 1800)
    {
      _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
      _defaultTimeoutSeconds = commandTimeoutSeconds > 0 ? commandTimeoutSeconds : 30;
    }

    public Result TestConnection()
    {
      try
      {
        using (var cnn = new SqlConnection(_connectionString))
        {
          cnn.Open();
          return Result.Ok();
        }
      }
      catch (Exception ex)
      {
        return Result.Fail(ex.Message, ex);
      }
    }

    public async Task<Result> TestConnectionAsync(CancellationToken cancellationToken = default)
    {
      try
      {
        using (var cnn = new SqlConnection(_connectionString))
        {
          await cnn.OpenAsync(cancellationToken).ConfigureAwait(false);
          return Result.Ok();
        }
      }
      catch (Exception ex)
      {
        return Result.Fail(ex.Message, ex);
      }
    }

    public Result<DataSet> ExecuteProcedure(string procName, IEnumerable<DbParameter> parameters = null, int? commandTimeoutSeconds = null)
    {
      try
      {
        using (var cnn = new SqlConnection(_connectionString))
        using (var cmd = new SqlCommand(procName, cnn) { CommandType = CommandType.StoredProcedure })
        {
          cmd.CommandTimeout = commandTimeoutSeconds ?? _defaultTimeoutSeconds;
          AttachParameters(cmd, parameters);

          using (var da = new SqlDataAdapter(cmd))
          {
            var ds = new DataSet();
            da.Fill(ds);
            return Result<DataSet>.Ok(ds);
          }
        }
      }
      catch (Exception ex)
      {
        return Result<DataSet>.Fail(ex.Message, ex);
      }
    }

    public Result<T> ExecuteProcedure<T>(string procName, Func<DataSet, T> transform, IEnumerable<DbParameter> parameters = null, int? commandTimeoutSeconds = null)
    {
      try
      {
        var raw = ExecuteProcedure(procName, parameters, commandTimeoutSeconds);
        if (!raw.Success)
          return Result<T>.Fail(raw.Message, raw.Error);

        var output = transform != null ? transform(raw.Data) : default(T);
        return Result<T>.Ok(output, raw.Message);
      }
      catch (Exception ex)
      {
        return Result<T>.Fail(ex.Message, ex);
      }
    }

    public async Task<Result<DataSet>> ExecuteProcedureAsync(string procName, IEnumerable<DbParameter> parameters = null, int? commandTimeoutSeconds = null, CancellationToken cancellationToken = default)
    {
      try
      {
        using (var cnn = new SqlConnection(_connectionString))
        using (var cmd = new SqlCommand(procName, cnn) { CommandType = CommandType.StoredProcedure })
        {
          cmd.CommandTimeout = commandTimeoutSeconds ?? _defaultTimeoutSeconds;
          AttachParameters(cmd, parameters);

          await cnn.OpenAsync(cancellationToken).ConfigureAwait(false);

          using (var reader = await cmd.ExecuteReaderAsync(CommandBehavior.Default, cancellationToken).ConfigureAwait(false))
          {
            var ds = await LoadDataSetAsync(reader, cancellationToken).ConfigureAwait(false);
            return Result<DataSet>.Ok(ds);
          }
        }
      }
      catch (Exception ex)
      {
        return Result<DataSet>.Fail(ex.Message, ex);
      }
    }

    public async Task<Result<T>> ExecuteProcedureAsync<T>(string procName, Func<DataSet, T> transform, IEnumerable<DbParameter> parameters = null, int? commandTimeoutSeconds = null, CancellationToken cancellationToken = default)
    {
      try
      {
        var raw = await ExecuteProcedureAsync(procName, parameters, commandTimeoutSeconds, cancellationToken).ConfigureAwait(false);
        if (!raw.Success)
          return Result<T>.Fail(raw.Message, raw.Error);

        var output = transform != null ? transform(raw.Data) : default(T);
        return Result<T>.Ok(output, raw.Message);
      }
      catch (Exception ex)
      {
        return Result<T>.Fail(ex.Message, ex);
      }
    }

    public Result<DataSet> ExecuteQuery(string sql, IEnumerable<DbParameter> parameters = null, int? commandTimeoutSeconds = null)
    {
      try
      {
        using (var cnn = new SqlConnection(_connectionString))
        using (var cmd = new SqlCommand(sql, cnn) { CommandType = CommandType.Text })
        {
          cmd.CommandTimeout = commandTimeoutSeconds ?? _defaultTimeoutSeconds;
          AttachParameters(cmd, parameters);

          using (var da = new SqlDataAdapter(cmd))
          {
            var ds = new DataSet();
            da.Fill(ds);
            return Result<DataSet>.Ok(ds);
          }
        }
      }
      catch (Exception ex)
      {
        return Result<DataSet>.Fail(ex.Message, ex);
      }
    }

    public Result<T> ExecuteQuery<T>(string sql, Func<DataSet, T> transform, IEnumerable<DbParameter> parameters = null, int? commandTimeoutSeconds = null)
    {
      try
      {
        var raw = ExecuteQuery(sql, parameters, commandTimeoutSeconds);
        if (!raw.Success)
          return Result<T>.Fail(raw.Message, raw.Error);

        var output = transform != null ? transform(raw.Data) : default(T);
        return Result<T>.Ok(output, raw.Message);
      }
      catch (Exception ex)
      {
        return Result<T>.Fail(ex.Message, ex);
      }
    }

    public async Task<Result<DataSet>> ExecuteQueryAsync(string sql, IEnumerable<DbParameter> parameters = null, int? commandTimeoutSeconds = null, CancellationToken cancellationToken = default)
    {
      try
      {
        using (var cnn = new SqlConnection(_connectionString))
        using (var cmd = new SqlCommand(sql, cnn) { CommandType = CommandType.Text })
        {
          cmd.CommandTimeout = commandTimeoutSeconds ?? _defaultTimeoutSeconds;
          AttachParameters(cmd, parameters);

          await cnn.OpenAsync(cancellationToken).ConfigureAwait(false);

          using (var reader = await cmd.ExecuteReaderAsync(CommandBehavior.Default, cancellationToken).ConfigureAwait(false))
          {
            var ds = await LoadDataSetAsync(reader, cancellationToken).ConfigureAwait(false);
            return Result<DataSet>.Ok(ds);
          }
        }
      }
      catch (Exception ex)
      {
        return Result<DataSet>.Fail(ex.Message, ex);
      }
    }

    public async Task<Result<T>> ExecuteQueryAsync<T>(string sql, Func<DataSet, T> transform, IEnumerable<DbParameter> parameters = null, int? commandTimeoutSeconds = null, CancellationToken cancellationToken = default)
    {
      try
      {
        var raw = await ExecuteQueryAsync(sql, parameters, commandTimeoutSeconds, cancellationToken).ConfigureAwait(false);
        if (!raw.Success)
          return Result<T>.Fail(raw.Message, raw.Error);

        var output = transform != null ? transform(raw.Data) : default(T);
        return Result<T>.Ok(output, raw.Message);
      }
      catch (Exception ex)
      {
        return Result<T>.Fail(ex.Message, ex);
      }
    }

    public Result<T> ExecuteScalar<T>(string sql, IEnumerable<DbParameter> parameters = null, int? commandTimeoutSeconds = null)
    {
      try
      {
        using (var cnn = new SqlConnection(_connectionString))
        using (var cmd = new SqlCommand(sql, cnn) { CommandType = CommandType.Text })
        {
          cmd.CommandTimeout = commandTimeoutSeconds ?? _defaultTimeoutSeconds;
          AttachParameters(cmd, parameters);

          cnn.Open();
          object value = cmd.ExecuteScalar();
          T casted = ConvertValue<T>(value);
          return Result<T>.Ok(casted);
        }
      }
      catch (Exception ex)
      {
        return Result<T>.Fail(ex.Message, ex);
      }
    }

    public async Task<Result<T>> ExecuteScalarAsync<T>(string sql, IEnumerable<DbParameter> parameters = null, int? commandTimeoutSeconds = null, CancellationToken cancellationToken = default)
    {
      try
      {
        using (var cnn = new SqlConnection(_connectionString))
        using (var cmd = new SqlCommand(sql, cnn) { CommandType = CommandType.Text })
        {
          cmd.CommandTimeout = commandTimeoutSeconds ?? _defaultTimeoutSeconds;
          AttachParameters(cmd, parameters);

          await cnn.OpenAsync(cancellationToken).ConfigureAwait(false);
          object value = await cmd.ExecuteScalarAsync(cancellationToken).ConfigureAwait(false);
          T casted = ConvertValue<T>(value);
          return Result<T>.Ok(casted);
        }
      }
      catch (Exception ex)
      {
        return Result<T>.Fail(ex.Message, ex);
      }
    }

    private static void AttachParameters(SqlCommand cmd, IEnumerable<DbParameter> parameters)
    {
      if (parameters == null)
        return;

      foreach (var p in parameters)
      {
        if (p == null) continue;

        if (p is SqlParameter sp)
        {
          cmd.Parameters.Add(sp);
          continue;
        }

        var np = new SqlParameter
        {
          ParameterName = string.IsNullOrEmpty(p.ParameterName)
            ? p.ParameterName
            : (p.ParameterName.StartsWith("@") ? p.ParameterName : "@" + p.ParameterName),
          Direction = p.Direction,
          IsNullable = p.IsNullable,
          Size = p.Size
        };

        if (p.DbType != DbType.Object)
        {
          np.DbType = p.DbType;
        }

        try { np.Precision = p.Precision; } catch { }
        try { np.Scale = p.Scale; } catch { }

        np.Value = p.Value ?? DBNull.Value;

        cmd.Parameters.Add(np);
      }
    }

    private static async Task<DataSet> LoadDataSetAsync(SqlDataReader reader, CancellationToken ct)
    {
      var ds = new DataSet();

      do
      {
        var dt = new DataTable();
        dt.Load(reader);
        ds.Tables.Add(dt);
      } while (await reader.NextResultAsync(ct).ConfigureAwait(false));

      return ds;
    }

    private static T ConvertValue<T>(object value)
    {
      if (value == null || value == DBNull.Value)
        return default(T);

      var target = typeof(T);
      if (target.IsAssignableFrom(value.GetType()))
        return (T)value;

      return (T)Convert.ChangeType(value, Nullable.GetUnderlyingType(target) ?? target);
    }
  }
}

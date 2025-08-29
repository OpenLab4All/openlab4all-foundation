using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;
using OpenLab4All.Foundation.Core;

namespace OpenLab4All.Foundation.Data.Abstractions
{
  public interface IDataAccess
  {
    Result<DataSet> ExecuteProcedure(
      string procName,
      IEnumerable<DbParameter> parameters = null,
      int? commandTimeoutSeconds = null);

    Result<T> ExecuteProcedure<T>(
      string procName,
      Func<DataSet, T> transform,
      IEnumerable<DbParameter> parameters = null,
      int? commandTimeoutSeconds = null);

    Task<Result<DataSet>> ExecuteProcedureAsync(
      string procName,
      IEnumerable<DbParameter> parameters = null,
      int? commandTimeoutSeconds = null,
      CancellationToken cancellationToken = default);

    Task<Result<T>> ExecuteProcedureAsync<T>(
      string procName,
      Func<DataSet, T> transform,
      IEnumerable<DbParameter> parameters = null,
      int? commandTimeoutSeconds = null,
      CancellationToken cancellationToken = default);


    Result<DataSet> ExecuteQuery(
      string sql,
      IEnumerable<DbParameter> parameters = null,
      int? commandTimeoutSeconds = null);

    Result<T> ExecuteQuery<T>(
      string sql,
      Func<DataSet, T> transform,
      IEnumerable<DbParameter> parameters = null,
      int? commandTimeoutSeconds = null);

    Task<Result<DataSet>> ExecuteQueryAsync(
      string sql,
      IEnumerable<DbParameter> parameters = null,
      int? commandTimeoutSeconds = null,
      CancellationToken cancellationToken = default);

    Task<Result<T>> ExecuteQueryAsync<T>(
      string sql,
      Func<DataSet, T> transform,
      IEnumerable<DbParameter> parameters = null,
      int? commandTimeoutSeconds = null,
      CancellationToken cancellationToken = default);


    Result<T> ExecuteScalar<T>(
      string sql,
      IEnumerable<DbParameter> parameters = null,
      int? commandTimeoutSeconds = null);

    Task<Result<T>> ExecuteScalarAsync<T>(
      string sql,
      IEnumerable<DbParameter> parameters = null,
      int? commandTimeoutSeconds = null,
      CancellationToken cancellationToken = default);


    Result TestConnection();

    Task<Result> TestConnectionAsync(CancellationToken cancellationToken = default);
  }
}
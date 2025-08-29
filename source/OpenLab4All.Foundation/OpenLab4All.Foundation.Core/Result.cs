using System;

namespace OpenLab4All.Foundation.Core
{
  public interface IResult
  {
    bool Success { get; }
    string Message { get; }
    Exception Error { get; }
  }

  public interface IResult<out T> : IResult
  {
    T Data { get; }
  }

  public sealed class Result : IResult
  {
    public bool Success { private set; get; }
    public string Message { private set; get; }
    public Exception Error { private set; get; }

    private Result(bool success, string message, Exception error)
    {
      Success = success;
      Message = message ?? string.Empty;
      Error = error;
    }

    public static Result Ok(string message = "ok")
      => new Result(true, message, null);

    public static Result Fail(string message, Exception error = null)
      => new Result(false, message ?? string.Empty, error);

    public static Result FromException(Exception ex, string message = null)
      => new Result(false, message ?? (ex != null ? ex.Message : string.Empty), ex);
  }

  public sealed class Result<T> : IResult<T>
  {
    public bool Success { get; private set; }
    public string Message { get; private set; }
    public Exception Error { get; private set; }
    public T Data { get; private set; }

    private Result(bool success, T data, string message, Exception error)
    {
      Success = success;
      Data = data;
      Message = message ?? string.Empty;
      Error = error;
    }

    public static Result<T> Ok(T data, string message = "ok")
      => new Result<T>(true, data, message, null);

    public static Result<T> Fail(string message, Exception error = null)
      => new Result<T>(false, default(T), message ?? string.Empty, error);

    public static Result<T> FromException(Exception ex, string message = null)
      => new Result<T>(false, default(T), message ?? (ex != null ? ex.Message : string.Empty), ex);
  }
}
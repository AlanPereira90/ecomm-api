using System.Net;

public class ResponseError : Exception
{
  public ResponseError(HttpStatusCode statusCode, string message, object? value = null)
    : base(message)
  {
    StatusCode = (int)statusCode;
    Value = value;
  }

  public int StatusCode { get; }

  public object? Value { get; }
}
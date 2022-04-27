using System.Net;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

public class ResponseExceptionFilter : IActionFilter, IOrderedFilter
{
  public int Order => int.MaxValue - 10;

  public void OnActionExecuting(ActionExecutingContext context) { }

  public void OnActionExecuted(ActionExecutedContext context)
  {
    if (context.Exception != null)
    {
      var error = context.Exception;

      var errorBody = new
      {
        message = context.Exception.Message,
      };

      var value = error is ResponseError ? ((ResponseError)error).Value : errorBody;

      context.Result = new ObjectResult(errorBody)
      {
        StatusCode = error is ResponseError ? ((ResponseError)error).StatusCode : (int)HttpStatusCode.InternalServerError,
      };

      context.ExceptionHandled = true;
    }
  }
}
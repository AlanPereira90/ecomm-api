using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

public class ResponseExceptionFilter : IActionFilter, IOrderedFilter
{
  public int Order => int.MaxValue - 10;

  public void OnActionExecuting(ActionExecutingContext context) { }

  public void OnActionExecuted(ActionExecutedContext context)
  {
    if (context.Exception is ResponseError error)
    {
      var errorBody = new
      {
        message = error.Message,
      };

      context.Result = new ObjectResult(error.Value ?? errorBody)
      {
        StatusCode = error.StatusCode
      };

      context.ExceptionHandled = true;
    }
  }
}
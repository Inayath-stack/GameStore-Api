using Microsoft.AspNetCore.Mvc.Filters;

namespace GameStore.Api.Filters;

public class LoggingActionFilter : ActionFilterAttribute
{
    private readonly ILogger<LoggingActionFilter> logger;
    public LoggingActionFilter(ILogger<LoggingActionFilter> logger)
    {
        this.logger=logger;
    }
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        logger.LogInformation("Executing {Method}{Path}->{Action}",
        context.HttpContext.Request.Method,
        context.HttpContext.Request.Path,
        context.ActionDescriptor.DisplayName);

        base.OnActionExecuting(context);
    }
    public override void OnActionExecuted(ActionExecutedContext context)
    {
        logger.LogInformation("Executed {Method}{Path}->{Action}",
        context.HttpContext.Request.Method,
        context.HttpContext.Request.Path,
        context.ActionDescriptor.DisplayName);

        base.OnActionExecuted(context);
        
    }
}
using Microsoft.AspNetCore.Mvc.Filters;

namespace GameStore.Api.Filters;
public class AppVersionResultFilter : ResultFilterAttribute
{
    private readonly ILogger<AppVersionResultFilter> logger;
    public AppVersionResultFilter(ILogger<AppVersionResultFilter> logger)
    {
        this.logger=logger;
    }
    public override void OnResultExecuting(ResultExecutingContext context)
    {
        context.HttpContext.Response.Headers.Append(
            "X-App_Version",
            "1.0");
            base.OnResultExecuting(context);
    }
}
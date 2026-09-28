using Mysqlx;
using GameStore.Api.Dtos;
using GameStore.Api.Exceptions;
namespace GameStore.Api.Middleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate next;
    private readonly ILogger<ExceptionMiddleware> logger;
    public ExceptionMiddleware(RequestDelegate next,ILogger<ExceptionMiddleware> logger)
    {
        this.next=next;
        this.logger=logger;
    }
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch(Exception ex)
        {
            context.Response.ContentType="application/json";
            var errorResponse=new ErrorResponseDto();
            if(ex is ApiException apiException)
            {
                context.Response.StatusCode=apiException.StatusCode;
                errorResponse.Message=apiException.Message;
            }
            // if(ex is UserAlreadyExistsException)
            // {
            //     context.Response.StatusCode=StatusCodes.Status409Conflict;
            //     errorResponse.Message=ex.Message;
            // }
            // else if(ex is InvalidCredentialsException)
            // {
            //     context.Response.StatusCode=StatusCodes.Status401Unauthorized;
            //     errorResponse.Message=ex.Message;
            // }
            // else if(ex is GameNotFoundException)
            // {
            //     context.Response.StatusCode=StatusCodes.Status404NotFound;
            //     errorResponse.Message=ex.Message;
            // }           
            else
            {
                context.Response.StatusCode=StatusCodes.Status500InternalServerError;
                errorResponse.Message="An unexpected error occured.";
                errorResponse.Error=ex.Message;
            }            
            
            await context.Response.WriteAsJsonAsync(errorResponse);
        }
    }
}
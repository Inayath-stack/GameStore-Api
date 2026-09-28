namespace GameStore.Api.Exceptions;

public class InvalidCredentialsException : ApiException
{
    public InvalidCredentialsException():base("Invalid Credentials.",StatusCodes.Status401Unauthorized)
    {
        
    }
}
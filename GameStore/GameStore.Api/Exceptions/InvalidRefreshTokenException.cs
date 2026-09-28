namespace GameStore.Api.Exceptions;

public class InvalidRefreshTokenException : ApiException
{
    public InvalidRefreshTokenException():base("Refresh Token Invalid", StatusCodes.Status401Unauthorized)
    {
        
    }
}
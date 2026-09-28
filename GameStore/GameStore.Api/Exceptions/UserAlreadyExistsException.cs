namespace GameStore.Api.Exceptions;
public class UserAlreadyExistsException : ApiException
{
    public UserAlreadyExistsException():base("User already exists.",StatusCodes.Status409Conflict){

    }
}
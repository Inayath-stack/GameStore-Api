namespace GameStore.Api.Exceptions;

public class GameNotFoundException : ApiException
{
    public GameNotFoundException() : base("Game not found.",StatusCodes.Status404NotFound)
    {

    }
}
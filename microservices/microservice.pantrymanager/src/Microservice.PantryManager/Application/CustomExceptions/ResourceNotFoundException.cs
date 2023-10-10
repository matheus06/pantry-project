using System.Net;
using Platform.Exceptions;

namespace Microservice.PantryManager.Application.CustomExceptions;

public class ResourceNotFoundException : Exception, ICustomException
{
    public ResourceNotFoundException(string resourceName)
    {
        ErrorMessage = $"'{resourceName}' was not found";
    }

    public HttpStatusCode StatusCode => HttpStatusCode.NotFound;
    public string ErrorMessage { get; }
}
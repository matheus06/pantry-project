using System.Net;
using Platform.Exceptions;

namespace Microservice.PantryManager.Application.CustomExceptions;

public class ResourceWithNameFoundException : Exception, ICustomException
{
    public ResourceWithNameFoundException(string resourceName, string name)
    {
        ErrorMessage = $"{resourceName} with name: \"{name}\" already exists! Please try another one!";
    }

    public HttpStatusCode StatusCode => HttpStatusCode.Conflict;
    public string ErrorMessage { get; }
}
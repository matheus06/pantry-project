using System.Net;

namespace Platform.Exceptions;

public interface ICustomException
{
    public HttpStatusCode StatusCode { get; }
    public string ErrorMessage { get; }
}
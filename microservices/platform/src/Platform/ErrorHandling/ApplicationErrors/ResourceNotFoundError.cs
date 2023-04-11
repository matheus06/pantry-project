using System.Net;
using FluentResults;

namespace Platform.ErrorHandling.ApplicationErrors;

public class ResourceNotFoundError : Error
{
    public ResourceNotFoundError(string resourceName)
        : base($"The {resourceName} was not found.")
    { 
        Metadata.Add(ErrorMetadataKeys.Title, "Not Found Error");
        Metadata.Add(ErrorMetadataKeys.ErrorCode, HttpStatusCode.NotFound);
    }
}
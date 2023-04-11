using System.Net;
using FluentResults;

namespace Platform.ErrorHandling.ApplicationErrors;

public class ValidationError : Error
{
    public ValidationError(string propertyName, string errorMessage)
        : base($"The property {propertyName} is not valid: {errorMessage}")
    { 
        
        Metadata.Add(ErrorMetadataKeys.Title, "Validation Error");
        Metadata.Add(ErrorMetadataKeys.ErrorCode, HttpStatusCode.BadRequest);
    }
}
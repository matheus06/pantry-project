using System.Net;
using FluentResults;

namespace Platform.ErrorHandling.ApplicationErrors;

public class ConflictNameError : Error
{
    public ConflictNameError(string entityName, string name)
        : base($"The entity {entityName} with name {name} already exists.")
    { 
        
        Metadata.Add(ErrorMetadataKeys.Title, "Conflict Error");
        Metadata.Add(ErrorMetadataKeys.ErrorCode, HttpStatusCode.Conflict);
    }
}
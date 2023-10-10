using System.Diagnostics;
using System.Net;
using FluentResults;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Platform.ErrorHandling;

public static class ProblemHelper
{
    public static IResult Problem(List<IError> errors)
    {
        var firstError = errors[0];
        var errorCode = (HttpStatusCode)firstError.Metadata[ErrorMetadataKeys.ErrorCode];
        var problemDetails = new ProblemDetails
        {
            Status = (int)errorCode,
            Title = firstError.Metadata[ErrorMetadataKeys.Title].ToString() ?? string.Empty,
        };

        var traceId = Activity.Current?.Id;
        problemDetails.Extensions["traceId"] = traceId;
        
        problemDetails.Extensions.Add("errorMessages", errors.Select(e=>e.Message));
        

        return TypedResults.Problem(problemDetails);
    }
}
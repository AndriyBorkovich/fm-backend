using FluentValidation.Results;

namespace FootballManager.Application.Extensions;

public static class ValidationFailureExtensions
{
    public static string ToResponse(this List<ValidationFailure> errorsList)
    {
        return errorsList.First().ErrorMessage;
    }
}

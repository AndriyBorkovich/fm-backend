using System.Text;
using FluentValidation.Results;

namespace FootballManager.Application.Extensions;

public static class ValidationFailureExtensions
{
    public static string ToResponse(this List<ValidationFailure> errorsList)
    {
        var stringBuilder = new StringBuilder();

        foreach (var error in errorsList)
        {
            stringBuilder.Append($"{error.PropertyName}: {error.ErrorMessage}, ");
        }

        return stringBuilder.ToString();
    }
}

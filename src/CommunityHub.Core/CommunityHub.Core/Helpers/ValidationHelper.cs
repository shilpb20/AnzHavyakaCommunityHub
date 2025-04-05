using CommunityHub.Core.Messages;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace CommunityHub.Core.Helpers
{
    public static class ValidationHelper
    {
        public static void ValidateNullOrEmptyString(string value, string paramName)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException(String.Format(ErrorMessage.ValueNullOrEmpty, paramName));
            }
        }

        public static ErrorResponse? ValidateModelState(ModelStateDictionary modelState, string errorCode)
        {
            if (!modelState.IsValid)
            {
                var errors = modelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                var errorMessage = errors.Any() ? string.Join("; ", errors) : "Invalid or missing data.";

                return new ErrorResponse
                {
                    ErrorCode = errorCode,
                    ErrorMessage = errorMessage
                };
            }

            return null;
        }
    }
}

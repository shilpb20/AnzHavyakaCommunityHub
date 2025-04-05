using CommunityHub.Core.Models;

namespace CommunityHub.Core.Factory
{
    public class ResponseFactory : IResponseFactory
    {
        public ApiResponse<T> Success<T>(T data)
        {
            return new ApiResponse<T>(data);
        }

        public ApiResponse<T> Failure<T>(string errorCode, string errorMessage)
        {
            return new ApiResponse<T>(errorCode, errorMessage);
        }

        public ApiResponse<T> Failure<T>(ErrorResponse errorResponse)
        {
            return new ApiResponse<T>(errorResponse.ErrorCode, errorResponse.ErrorMessage);
        }
    }
}


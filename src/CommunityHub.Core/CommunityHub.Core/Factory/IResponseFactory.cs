using CommunityHub.Core.Models;

namespace CommunityHub.Core.Factory
{
    public interface IResponseFactory
    {
        ApiResponse<T> Success<T>(T data);
        ApiResponse<T> Failure<T>(string errorCode, string errorMessage);
        ApiResponse<T> Failure<T>(ErrorResponse errorResponse);
    }

}
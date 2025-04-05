namespace CommunityHub.Core.Models
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public T? Data { get; set; } = default;
        public string ErrorMessage { get; set; }
        public string ErrorCode { get; set; }

        public ApiResponse(T? data)
        {
            Success = true;
            Data = data;
        }

        public ApiResponse()
        {
            Success = false;
            Data = default;
            ErrorMessage = "Unknown Error";
            ErrorCode = "Unknown Error";
        }

        public ApiResponse(ErrorResponse errorResponse)
        {
            Success = false;
            Data = default;
            ErrorCode = errorResponse.ErrorCode;
            ErrorMessage = errorResponse.ErrorMessage;
        }

        public ApiResponse(string errorCode, string errorMessage)
        {
            Success = false;
            Data = default;
            ErrorCode = errorCode;
            ErrorMessage = errorMessage;
        }
    }
}

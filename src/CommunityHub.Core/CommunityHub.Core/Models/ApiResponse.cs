namespace CommunityHub.Core.Models
{
    public class ApiResponse<T>
    {
        public bool IsSuccess { get; set; }
        public T? Data { get; set; } = default;
        public string ErrorMessage { get; set; }
        public string ErrorCode { get; set; }

        public ApiResponse(T? data)
        {
            IsSuccess = true;
            Data = data;
        }

        public ApiResponse()
        {
            IsSuccess = false;
            Data = default;
            ErrorMessage = "Unknown Error";
            ErrorCode = "Unknown Error";
        }

        public ApiResponse(ErrorResponse errorResponse)
        {
            IsSuccess = false;
            Data = default;
            ErrorCode = errorResponse.ErrorCode;
            ErrorMessage = errorResponse.ErrorMessage;
        }

        public ApiResponse(string errorCode, string errorMessage)
        {
            IsSuccess = false;
            Data = default;
            ErrorCode = errorCode;
            ErrorMessage = errorMessage;
        }
    }
}

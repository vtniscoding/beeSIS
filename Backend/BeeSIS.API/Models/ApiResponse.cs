namespace BeeSIS.API.Models
{
    /// <summary>
    /// Generic API response wrapper for consistent response formatting.
    /// </summary>
    public class ApiResponse<T>
    {
        public bool IsSuccess { get; set; }
        public T? Data { get; set; }
        public string Message { get; set; } = string.Empty;
        public List<string> Errors { get; set; } = new List<string>();

        /// <summary>
        /// Creates a successful response with data.
        /// </summary>
        public static ApiResponse<T> Success(T data, string message = "")
        {
            return new ApiResponse<T>
            {
                IsSuccess = true,
                Data = data,
                Message = message
            };
        }

        /// <summary>
        /// Creates a failure response with error messages.
        /// </summary>
        public static ApiResponse<T> Failure(string message, List<string>? errors = null)
        {
            return new ApiResponse<T>
            {
                IsSuccess = false,
                Message = message,
                Errors = errors ?? new List<string>()
            };
        }

        /// <summary>
        /// Creates a failure response with a single error message.
        /// </summary>
        public static ApiResponse<T> Failure(string message, string error)
        {
            return new ApiResponse<T>
            {
                IsSuccess = false,
                Message = message,
                Errors = new List<string> { error }
            };
        }
    }
}

namespace Application.Common.Results
{
    // Result classes for service layer
    public class ServiceResult<T>
    {
        public bool IsSuccess { get; }
        public string Message { get; }
        public T? Data { get; }
        public IEnumerable<string> Errors { get; }

        private ServiceResult(bool isSuccess, string message, T? data = default, IEnumerable<string>? errors = null)
        {
            IsSuccess = isSuccess;
            Message = message;
            Data = data;
            Errors = errors ?? Enumerable.Empty<string>();
        }

        public static ServiceResult<T> Success(T data, string message = "Operation successful")
        {
            return new ServiceResult<T>(true, message, data);
        }

        public static ServiceResult<T> Success(string message)
        {
            return new ServiceResult<T>(true, message);
        }

        public static ServiceResult<T> Failure(string message, IEnumerable<string>? errors = null)
        {
            return new ServiceResult<T>(false, message, default, errors);
        }

        public static ServiceResult<T> Failure(string message, string error)
        {
            return new ServiceResult<T>(false, message, default, new[] { error });
        }
    }

    // Specific result data models
    public class AuthTokenData
    {
        public required int UserId { get; set; }
        public required string Token { get; set; }
        public required DateTime ExpiresIn { get; set; }
        public string? UserRole { get; set; }
        public string? RefreshToken { get; set; }
    }
}

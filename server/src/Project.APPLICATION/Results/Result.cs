using Project.CORE.Interfaces;

namespace Project.APPLICATION.Results
{
    public class Result<T> : IResult<T>
    {
        public bool Success { get; set; }

        public string Message { get; set; } = "";
        public T? Data { get; set; }

        public static Result<T> SuccessResult(T? data = default, string message = "Operation completed successfully.")
        {
            return new Result<T>
            {
                Success = true,
                Message = message,
                Data = data
            };
        }

        public static Result<T> FailureResult(string message = "Operation failed.", T? data = default)
        {
            return new Result<T>
            {
                Success = false,
                Message = message,
                Data = data
            };
        }
    }
};


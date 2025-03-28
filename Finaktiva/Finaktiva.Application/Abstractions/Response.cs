using Microsoft.AspNetCore.Http;
using System.Net;

namespace Finaktiva.Application.Abstractions
{
    public class Response<T>
    {
        public T? Data { get; set; }
        public string Message { get; set; } = string.Empty;
        public int StatusCode { get; set; }
        public bool Success { get; set; }
        public Dictionary<string, string[]>? Errors { get; set; }

        public static Response<T> SuccessResponse(T data, int statusCode = StatusCodes.Status200OK, string message = "Operación exitosa")
        {
            return new Response<T>
            {
                Data = data,
                Success = true,
                StatusCode = statusCode,
                Message = message
            };
        }

        public static Response<T> NoFoundResponse(string message, int statusCode = StatusCodes.Status404NotFound)
        {
            return new Response<T>
            {
                Success = false,
                StatusCode = statusCode,
                Message = message
            };
        }

        public static Response<T> ErrorResponse(string message, int statusCode = StatusCodes.Status500InternalServerError)
        {
            return new Response<T>
            {
                Success = false,
                StatusCode = statusCode,
                Message = message
            };
        }

        public static Response<T> ValidationError(string message, Dictionary<string, string[]> errors)
        {
            return new Response<T>
            {
                Success = false,
                StatusCode = StatusCodes.Status400BadRequest,
                Message = message,
                Errors = errors
            };
        }
    }
}

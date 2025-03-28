using Finaktiva.Application.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace Finaktiva.Api.Helper
{
    public static class ResponseExtensions
    {
        public static IActionResult ToActionResult<T>(this Response<T> response)
        {
            return new ObjectResult(response)
            {
                StatusCode = response.StatusCode
            };
        }
    }
}

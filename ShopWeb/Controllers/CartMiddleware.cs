using DataAccess.DTO;
using Microsoft.AspNetCore.Http;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace ShopWeb.Middleware
{
    public class NotFoundMiddleware
    {
        private readonly RequestDelegate _next;

        public NotFoundMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            await _next(context);

            if (context.Response.StatusCode == StatusCodes.Status404NotFound)
            {
                context.Response.Redirect("Product"); // Điều hướng đến trang NotFount
            }
        }
    }
}

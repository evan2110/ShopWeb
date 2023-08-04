using DataAccess.DTO;
using Microsoft.AspNetCore.Http;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace ShopWeb.Middleware
{
    public class CheckCartMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IHttpClientFactory _httpClientFactory;
        private static string cartUrl = "https://localhost:7010/api/Cart"; // Thay đổi URL API kiểm tra giỏ hàng tùy theo ứng dụng của bạn

        public CheckCartMiddleware(RequestDelegate next, IHttpClientFactory httpClientFactory)
        {
            _next = next;
            _httpClientFactory = httpClientFactory;
        }

        public async Task Invoke(HttpContext context)
        {
            await _next(context);
            if (context.Session.GetString("UserId") == null)
            {
                context.Session.SetString("UserId", "0");
            }
            if (context.Session.GetString("UserId") != null && context.Session.GetString("UserId") != "0")
            {
                var httpClient = _httpClientFactory.CreateClient();
                string urlCart = $"{cartUrl}/{context.Session.GetString("UserId")}";
                HttpResponseMessage responseCart = await httpClient.GetAsync(urlCart);
                string strDataCart = await responseCart.Content.ReadAsStringAsync();

                // Kiểm tra và xử lý dữ liệu giỏ hàng ở đây (tương tự như trong ví dụ của bạn)
                if(!string.IsNullOrEmpty(strDataCart) && strDataCart.Trim().StartsWith("{"))
                {
                    var optionsCart = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };

                    CartDTO cartDTO = System.Text.Json.JsonSerializer.Deserialize<CartDTO>(strDataCart, optionsCart);

                    if (cartDTO != null)
                    {
                        string cartJson = System.Text.Json.JsonSerializer.Serialize(cartDTO);
                        context.Session.SetString("Cart", cartJson);
                    }
                    else
                    {
                        context.Session.SetString("Cart", null);
                    }
                }
                await _next(context);
            }
        }
    }
}

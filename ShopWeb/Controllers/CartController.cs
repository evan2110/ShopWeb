using DataAccess.DTO;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace ShopWeb.Controllers
{
    public class CartController : Controller
    {
        private static string cartUrl = "https://localhost:7010/api/Cart";
        private static HttpClient httpClient;

        public CartController(ILogger<HomeController> logger)
        {
            httpClient = new HttpClient();
        }

        public async Task<IActionResult> Index()
        {
            //GetCart
            string urlCart = $"{cartUrl}/{HttpContext.Session.GetString("UserId")}";
            HttpResponseMessage responseCart = await httpClient.GetAsync(urlCart);
            string strDataCart = await responseCart.Content.ReadAsStringAsync();

            // Kiểm tra xem strDataCart có phải là chuỗi JSON hợp lệ hay không
            if (!string.IsNullOrEmpty(strDataCart) && strDataCart.Trim().StartsWith("{"))
            {
                var optionsCart = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                CartDTO cartDTO = System.Text.Json.JsonSerializer.Deserialize<CartDTO>(strDataCart, optionsCart);
                ViewBag.CartItems = cartDTO.CartItemDTOs;
            }
            return View();
        }
    }
}

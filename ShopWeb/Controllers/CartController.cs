using DataAccess.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShoppingWebAPI.Request;
using ShoppingWebAPI.Response;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace ShopWeb.Controllers
{
    public class CartController : Controller
    {
        private static string cartUrl = "https://localhost:7010/api/Cart";
        private static string orderUrl = "https://localhost:7010/api/Order";
        private static string orderDetailUrl = "https://localhost:7010/api/OrderDetail";
        private static string deleteCartItemByCartIdUrl = "https://localhost:7010/api/CartItem/DeleteCartItemByCartId";



        private static HttpClient httpClient;

        public CartController(ILogger<HomeController> logger)
        {
            httpClient = new HttpClient();
        }

        public async Task<IActionResult> Index()
        {
			if (HttpContext.Session.GetString("UserId") != null)
			{
				CartDTO cartDTO = await GetCart();
				if (cartDTO != null)
				{
					ViewBag.CartItems = cartDTO.CartItemDTOs;
				}
				return View();
			}
			else
			{
				return View("Error");
			}
        }

        [HttpPost]
        public async Task<IActionResult> AddOrder(AddOrderRequest addOrderRequest)
        {
            if(HttpContext.Session.GetString("UserId") != null)
            {
                // Serialize addOrderRequest thành chuỗi JSON
                string addOrderRequestJson = JsonSerializer.Serialize(addOrderRequest);

                // Lưu vào session
                HttpContext.Session.SetString("AddOrderRequest", addOrderRequestJson);

                //Tao Order
                OrderDTO orderDTO = new OrderDTO();


                var checkoutUrl = "https://localhost:7010/api/Cart/create-payment-intent";
				HttpResponseMessage responseCreatePayment = await httpClient.PostAsJsonAsync(checkoutUrl, orderDTO);
				var result = await responseCreatePayment.Content.ReadFromJsonAsync<PaymentStripeResponse>();
                
				return Redirect(result.Url);
			}
			else
			{
				return View("Error");
			}
        }

        private async Task<CartDTO> GetCart()
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
                return cartDTO;

            }
            return null;
        }

    }
}

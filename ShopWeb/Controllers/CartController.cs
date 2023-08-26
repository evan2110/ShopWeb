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
                orderDTO.UserId = addOrderRequest.UserId;
                orderDTO.OrderDate = DateTime.Now;
                orderDTO.RequiredDate = DateTime.Now.AddDays(7);
                orderDTO.ShippedDate = DateTime.Now.AddDays(7);
                orderDTO.ShipAddress = addOrderRequest.ShipAddress;
                orderDTO.Status = "Active";
                orderDTO.CreatedTime = DateTime.Now;
                orderDTO.ShipPhone = addOrderRequest.ShipPhone;
                orderDTO.TotalPrice = addOrderRequest.ToTalPrice;

                //Lay tat ca cartItem da order
                CartDTO cartDTO = await GetCart();
                List<CartItemDTO> cartItemDTOs = cartDTO.CartItemDTOs;
                List<OrderDetailDTO> lstOrderDetailDTOs = new List<OrderDetailDTO>();
                foreach (var item in cartItemDTOs)
                {
                    OrderDetailDTO orderDetailDTO = new OrderDetailDTO();
                    orderDetailDTO.ProductId = item.ProductId;
                    orderDetailDTO.ProductName = item.ProductName;
                    orderDetailDTO.Image = item.ImageFront;
                    orderDetailDTO.OrderId = orderDTO.OrderId;
                    //set Price la ToTalPrice tong de sang kia hien thi
                    orderDetailDTO.Price = item.TotalPrice;
                    orderDetailDTO.Quantity = item.Quantity;
                    orderDetailDTO.Status = "Active";
                    orderDetailDTO.CreatedTime = DateTime.Now;
                    lstOrderDetailDTOs.Add(orderDetailDTO);
                }

                var checkoutUrl = "https://localhost:7010/api/Cart/create-payment-intent";
				HttpResponseMessage responseCreatePayment = await httpClient.PostAsJsonAsync(checkoutUrl, lstOrderDetailDTOs);
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

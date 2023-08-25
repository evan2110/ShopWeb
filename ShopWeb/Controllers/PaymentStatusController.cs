using BusinessObject.Models;
using DataAccess.DTO;
using Microsoft.AspNetCore.Mvc;
using ShoppingWebAPI.Request;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace ShopWeb.Controllers
{
    public class PaymentStatusController : Controller
    {
        private static string cartUrl = "https://localhost:7010/api/Cart";
        private static string orderUrl = "https://localhost:7010/api/Order";
        private static string orderDetailUrl = "https://localhost:7010/api/OrderDetail";
        private static string deleteCartItemByCartIdUrl = "https://localhost:7010/api/CartItem/DeleteCartItemByCartId";
        private static HttpClient httpClient;
        public PaymentStatusController()
        {
            httpClient = new HttpClient();
        }

        public async Task<IActionResult> Index(string? status)
        {
            if(status == "suss")
            {
                // Lấy chuỗi JSON từ session
                string addOrderRequestJson = HttpContext.Session.GetString("AddOrderRequest");

                if (!string.IsNullOrEmpty(addOrderRequestJson))
                {
                    // Deserialization thành đối tượng AddOrderRequest
                    AddOrderRequest savedAddOrderRequest = JsonSerializer.Deserialize<AddOrderRequest>(addOrderRequestJson);

                    OrderDTO orderDTO = new OrderDTO();
                    orderDTO.UserId = savedAddOrderRequest.UserId;
                    orderDTO.OrderDate = DateTime.Now;
                    orderDTO.RequiredDate = DateTime.Now.AddDays(7);
                    orderDTO.ShippedDate = DateTime.Now.AddDays(7);
                    orderDTO.ShipAddress = savedAddOrderRequest.ShipAddress;
                    orderDTO.Status = "Active";
                    orderDTO.CreatedTime = DateTime.Now;
                    orderDTO.ShipPhone = savedAddOrderRequest.ShipPhone;
                    orderDTO.TotalPrice = savedAddOrderRequest.ToTalPrice;

                    string urlCreateOrder = $"{orderUrl}";
                    //Lay token tu session
                    string token = HttpContext.Session.GetString("Token");
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    HttpResponseMessage response = await httpClient.PostAsJsonAsync(urlCreateOrder, orderDTO);
                    orderDTO = await response.Content.ReadFromJsonAsync<OrderDTO>();

                    //Lay tat ca cartItem da order
                    CartDTO cartDTO = await GetCart();
                    List<CartItemDTO> cartItemDTOs = cartDTO.CartItemDTOs;
                    List<OrderDetailDTO> lstOrderDetailDTOs = new List<OrderDetailDTO>();
                    foreach (var item in cartItemDTOs)
                    {
                        OrderDetailDTO orderDetailDTO = new OrderDetailDTO();
                        orderDetailDTO.ProductId = item.ProductId;
                        orderDetailDTO.OrderId = orderDTO.OrderId;
                        orderDetailDTO.Price = item.TotalPrice;
                        orderDetailDTO.Quantity = item.Quantity;
                        orderDetailDTO.Status = "Active";
                        orderDetailDTO.CreatedTime = DateTime.Now;
                        lstOrderDetailDTOs.Add(orderDetailDTO);
                    }
                    //tao OrderDetail
                    string urlCreateOrderDetail = $"{orderDetailUrl}";
                    foreach (var item in lstOrderDetailDTOs)
                    {
                        HttpResponseMessage responseOrderDetail = await httpClient.PostAsJsonAsync(urlCreateOrderDetail, item);
                    }

                    //xoa cartItem sau khi thanh toan xong
                    string urlDeleteCartItem = $"{deleteCartItemByCartIdUrl}/{cartDTO.CartId}";
                    HttpResponseMessage responseDeleteCartItem = await httpClient.DeleteAsync(urlDeleteCartItem);

                    // Lấy thông tin giỏ hàng sau cập nhật
                    string urlCart = $"{cartUrl}/{HttpContext.Session.GetString("UserId")}";
                    HttpResponseMessage responseCart = await httpClient.GetAsync(urlCart);
                    cartDTO = await responseCart.Content.ReadFromJsonAsync<CartDTO>();

                    if (cartDTO != null)
                    {
                        var cart = System.Text.Json.JsonSerializer.Serialize(cartDTO);
                        HttpContext.Session.SetString("Cart", cart);
                    }
                    else
                    {
                        HttpContext.Session.SetString("Cart", null);
                    }
                }
                return View("SussPayment");
            }else if(status == "fail")
            {
                return View("FailPayment");
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

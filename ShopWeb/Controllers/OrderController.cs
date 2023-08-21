using BusinessObject.Models;
using DataAccess.DTO;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using System;
using System.Text.Json;

namespace ShopWeb.Controllers
{
    public class OrderController : Controller
    {
        private static string orderDetailUrl = "https://localhost:7010/api/OrderDetail";

        private static HttpClient httpClient;

        public OrderController()
        {
            httpClient = new HttpClient();
        }

        public async Task<IActionResult> Index(int userId, int pageNumber = 1, int pageSize = 5)
        {
            string urlGetOrderDetail;
            urlGetOrderDetail = $"{orderDetailUrl}/{userId}?pageSize={pageSize}&pageNumber={pageNumber}";
            HttpResponseMessage responseOrderDetail = await httpClient.GetAsync(urlGetOrderDetail);
            string strDataOrderDetail = await responseOrderDetail.Content.ReadAsStringAsync();
            var optionsOrderDetail = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            List<OrderDetailDTO> OrderDetails = System.Text.Json.JsonSerializer.Deserialize<List<OrderDetailDTO>>(strDataOrderDetail, optionsOrderDetail);
            ViewBag.OrderDetails = OrderDetails;


            // Lay totalPage
            string urlGetTotalOrderDetail = $"{orderDetailUrl}/{userId}?pageSize=0&pageNumber=1";
            HttpResponseMessage responseTotalOrderDetail = await httpClient.GetAsync(urlGetTotalOrderDetail);
            string strTotalOrderDetail = await responseTotalOrderDetail.Content.ReadAsStringAsync();
            var optionsTotalOrderDetail = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            List<OrderDetailDTO> totalOrderDetail = System.Text.Json.JsonSerializer.Deserialize<List<OrderDetailDTO>>(strTotalOrderDetail, optionsTotalOrderDetail);

            ViewBag.userId = userId;
            ViewBag.pageNumber = pageNumber;
            ViewBag.pageSize = pageSize;
            if (OrderDetails != null && OrderDetails.Count > 0)
            {
                ViewBag.ToTalPage = (int)Math.Ceiling((double)totalOrderDetail.Count / pageSize);
            }
            else
            {

                ViewBag.TotalPage = 1;
            }
            return View();
        }
    }
}

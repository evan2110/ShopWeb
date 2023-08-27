using DataAccess.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Text.Json;
using Twilio.TwiML.Voice;

namespace ShopWeb.Controllers
{
    public class DashboardController : Controller
    {
        private static HttpClient httpClient;
        private static string categoryUrl = "https://localhost:7010/api/Category";
        private static string orderDetailUrl = "https://localhost:7010/api/OrderDetail";

        public DashboardController()
        {
            httpClient = new HttpClient();
        }

        public async Task<IActionResult> Index(string? mode)
        {
            if (HttpContext.Session.GetString("Role") == "3")
            {
                if(mode == "Dashboard" ||  mode == null)
                {
                    //Thống kê % sản phẩm thuộc mỗi category chiếm trên tổng số product
                    Dictionary<string,double> numberProductsEachCategory = new Dictionary<string,double>();
                    int totalProduct = 0;
                    string urlGetCategory = $"{categoryUrl}/getAll";
                    HttpResponseMessage responseGetCategory = await httpClient.GetAsync(urlGetCategory);
                    string strDataGetCategory = await responseGetCategory.Content.ReadAsStringAsync();
                    var optionsGetCategory = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };
                    var categories = System.Text.Json.JsonSerializer.Deserialize<List<CategoryDTO>>(strDataGetCategory, optionsGetCategory);
                    foreach(var item in categories)
                    {
                        numberProductsEachCategory.Add(item.CategoryName, item.Products.Count());
                        totalProduct += item.Products.Count();
                    }

                    foreach (var item in numberProductsEachCategory.ToList())
                    {
                        double percentage = (item.Value / totalProduct) * 100;
                        percentage = Math.Round(percentage, 2);
                        numberProductsEachCategory[item.Key] = percentage;
                    }

                    //top 10 sản phẩm được mua nhiều nhất
                    Dictionary<string, double> topProductSelled = new Dictionary<string, double>();
                    string urlTopBuyProduct = $"{orderDetailUrl}/topBuyProduct";
                    HttpResponseMessage responseTopBuyProduct = await httpClient.GetAsync(urlTopBuyProduct);
                    string strDataTopBuyProduct = await responseTopBuyProduct.Content.ReadAsStringAsync();
                    var optionsTopBuyProduct = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };

                    List<TopBuyProductDTO> listTopBuyProduct = System.Text.Json.JsonSerializer.Deserialize<List<TopBuyProductDTO>>(strDataTopBuyProduct, optionsTopBuyProduct);
                    foreach(var item in listTopBuyProduct)
                    {
                        topProductSelled.Add(item.ProductName, Math.Round((double)(item.Quantity * item.Price), 2));
                    }

                    ViewBag.NumberProductsEachCategory = numberProductsEachCategory;
                    ViewBag.topProductSelled = topProductSelled;
                    ViewBag.Mode = "Dashboard";
                }else if(mode == "User")
                {
                    ViewBag.Mode = "User";
                }
                return View();

            }
            else
            {
                return View("unAuthen");
            }
        }
    }
}

using DataAccess.DTO;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Text.Json;

namespace ShopWeb.Controllers
{
    public class ShopController : Controller
    {
        private static HttpClient httpClient;
        private static string productUrl = "https://localhost:7010/api/Product";
        private static string categoryUrl = "https://localhost:7010/api/Category";
        private static string colorUrl = "https://localhost:7010/api/Color";

        public ShopController()
        {
            httpClient = new HttpClient();
        }
        public async Task<IActionResult> Index(decimal? maxPrice, decimal? minPrice, int? categoryId, int? sort, int pageNumber = 1, int pageSize = 9)
        {
            string urlGetAllProduct;
            urlGetAllProduct = $"{productUrl}/getAll?pageSize={pageSize}&pageNumber={pageNumber}&minPrice={minPrice}&maxPrice={maxPrice}&categoryId={categoryId}&sort={sort}";
            HttpResponseMessage responseProduct = await httpClient.GetAsync(urlGetAllProduct);
            string strDataProduct = await responseProduct.Content.ReadAsStringAsync();
            var optionsProduct = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            List<ProductDTO> product = System.Text.Json.JsonSerializer.Deserialize<List<ProductDTO>>(strDataProduct, optionsProduct);

            //Lay totalPage
            string urlGetTotalProduct = $"{productUrl}/getAll?pageSize=0&pageNumber=1&minPrice={minPrice}&maxPrice={maxPrice}&categoryId={categoryId}&sort={sort}";
            HttpResponseMessage responseTotalProduct = await httpClient.GetAsync(urlGetTotalProduct);
            string strTotalProduct = await responseTotalProduct.Content.ReadAsStringAsync();
            var optionsTotalProduct = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            List<ProductDTO> totalProduct = System.Text.Json.JsonSerializer.Deserialize<List<ProductDTO>>(strTotalProduct, optionsTotalProduct);

            ViewBag.products = product;
            ViewBag.pageNumber = pageNumber;
            ViewBag.pageSize = pageSize;
            if (product != null && product.Count > 0)
            {
                ViewBag.ToTalPage = (int)Math.Ceiling((double)totalProduct.Count / pageSize);
            }
            else
            {

                ViewBag.TotalPage = 1;
            }

            //Lay Category
            string urlGetCategory = $"{categoryUrl}/getAll";
            HttpResponseMessage responseCategory = await httpClient.GetAsync(urlGetCategory);
            string strCategory = await responseCategory.Content.ReadAsStringAsync();
            var optionsCategory = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            List<CategoryDTO> categories = System.Text.Json.JsonSerializer.Deserialize<List<CategoryDTO>>(strCategory, optionsCategory);
            ViewBag.categories = categories;

            //Lay Color
            string urlGetColor = $"{colorUrl}/getAll";
            HttpResponseMessage responseColor = await httpClient.GetAsync(urlGetColor);
            string strColor = await responseColor.Content.ReadAsStringAsync();
            var optionsColor = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            List<ColorDTO> colors = System.Text.Json.JsonSerializer.Deserialize<List<ColorDTO>>(strColor, optionsColor);
            ViewBag.colors = colors;

            ViewBag.minPrice = minPrice;
            ViewBag.maxPrice = maxPrice;
            ViewBag.categoryId = categoryId;
            ViewBag.sort = sort;

            return View();
        }
    }
}

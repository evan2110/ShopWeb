using DataAccess.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text.Json;

namespace ShopWeb.Controllers
{
    public class ProductController : Controller
    {
        private static HttpClient httpClient;
        private static string productdetailUrl = "https://localhost:7010/api/Product";

        public ProductController()
        {
            httpClient = new HttpClient();
        }
        public async Task<IActionResult> Index(int product_id)
        {
            string urlProductDetail = $"{productdetailUrl}/{product_id}";
            HttpResponseMessage responseProduct = await httpClient.GetAsync(urlProductDetail);
            string strDataProduct = await responseProduct.Content.ReadAsStringAsync();
            var optionsProduct = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            ProductDTO product = System.Text.Json.JsonSerializer.Deserialize<ProductDTO>(strDataProduct, optionsProduct);

            ViewBag.product = product;
            ViewBag.listColor = new SelectList(product.ProductColorDTOs.ToList(), "ColorId", "ColorName");
            ViewBag.listSize = new SelectList(product.ProductSizeDTOs.ToList(), "SizeId", "SizeName");

            return View();
        }
    }
}

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

            //GetRelatedProduct
            string urlRelatedProduct = $"{productdetailUrl}/list?categoryId={product.CategoryId}";
            HttpResponseMessage responseRelatedProduct = await httpClient.GetAsync(urlRelatedProduct);
            string strDataRelatedProduct = await responseRelatedProduct.Content.ReadAsStringAsync();
            var optionsRelatedProduct = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            List<ProductDTO> productRelated = System.Text.Json.JsonSerializer.Deserialize<List<ProductDTO>>(strDataRelatedProduct, optionsRelatedProduct);

            //GetUpSellProduct
            string urlUpSellProduct = $"{productdetailUrl}/list";
            HttpResponseMessage responseUpSellProduct = await httpClient.GetAsync(urlUpSellProduct);
            string strDataUpSellProduct = await responseUpSellProduct.Content.ReadAsStringAsync();
            var optionsUpSellProduct = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            List<ProductDTO> upSellProduct = System.Text.Json.JsonSerializer.Deserialize<List<ProductDTO>>(strDataUpSellProduct, optionsUpSellProduct);

            ViewBag.product = product;
            ViewBag.listColor = new SelectList(product.ProductColorDTOs.ToList(), "ColorId", "ColorName");
            ViewBag.listSize = new SelectList(product.ProductSizeDTOs.ToList(), "SizeId", "SizeName");
            ViewBag.relatedProduct = productRelated;
            ViewBag.upSellProduct = upSellProduct;

            return View();
        }
    }
}

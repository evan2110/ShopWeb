using DataAccess.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ShoppingWebAPI.Request;
using System.Text.Json;

namespace ShopWeb.Controllers
{
    public class ProductController : Controller
    {
        private static HttpClient httpClient;
        private static string productdetailUrl = "https://localhost:7010/api/Product";
        private static string cartUrl = "https://localhost:7010/api/Cart";
        private static string cartItemUrl = "https://localhost:7010/api/CartItem";

        public ProductController()
        {
            httpClient = new HttpClient();
        }
        public async Task<IActionResult> Index(int product_id)
        {
            await LoadProductDetails(product_id);
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(AddToCartRequest addToCartRequest)
        {
            var userId = Int32.Parse(HttpContext.Session.GetString("UserId"));
            var cart = HttpContext.Session.GetString("Cart");
            CartDTO cartDTO;

            if (cart == null)
            {
                // Tạo một giỏ hàng mới nếu chưa có
                cartDTO = new CartDTO
                {
                    UserId = userId,
                    Status = "Active",
                    CreatedTime = DateTime.Now
                };

                string urlCreateCart = $"{cartUrl}";
                HttpResponseMessage response = await httpClient.PostAsJsonAsync(urlCreateCart, cartDTO);

                // Lấy thông tin giỏ hàng sau khi đã tạo
                cartDTO = await response.Content.ReadFromJsonAsync<CartDTO>();
            }
            else
            {
                cartDTO = System.Text.Json.JsonSerializer.Deserialize<CartDTO>(cart);
            }

            // Kiểm tra xem sản phẩm đã có trong giỏ hàng chưa
            string urlCartItem = $"{cartItemUrl}?cartId={cartDTO.CartId}&productId={addToCartRequest.ProductId}&colorId={addToCartRequest.ColorId}&sizeId={addToCartRequest.SizeId}";
            HttpResponseMessage responseCartItem = await httpClient.GetAsync(urlCartItem);
            string strDataCartItem = await responseCartItem.Content.ReadAsStringAsync();

            if (!string.IsNullOrEmpty(strDataCartItem) && strDataCartItem.Trim().StartsWith("{"))
            {
                // Sản phẩm đã có trong giỏ hàng, cập nhật số lượng
                var optionsCartItem = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                CartItemDTO cartItemDTO = System.Text.Json.JsonSerializer.Deserialize<CartItemDTO>(strDataCartItem, optionsCartItem);
                cartItemDTO.Quantity += addToCartRequest.Quantity;

                string urlUpdate = $"{cartItemUrl}/{cartItemDTO.CartItemId}";
                HttpResponseMessage response = await httpClient.PutAsJsonAsync(urlUpdate, cartItemDTO);
            }
            else
            {
                // Sản phẩm chưa có trong giỏ hàng, tạo mới
                var productDTO = await GetProductDetails(addToCartRequest.ProductId);

                decimal totalPrice = (productDTO.Price - (productDTO.Discount * productDTO.Price)) * addToCartRequest.Quantity;

                CartItemDTO newCartItemDTO = new CartItemDTO
                {
                    CartId = cartDTO.CartId,
                    ProductId = addToCartRequest.ProductId,
                    Quantity = addToCartRequest.Quantity,
                    TotalPrice = totalPrice,
                    Status = "Active",
                    CreatedTime = DateTime.Now,
                    ColorId = addToCartRequest.ColorId,
                    SizeId = addToCartRequest.SizeId
                };

                string urlCreate = $"{cartItemUrl}";
                HttpResponseMessage response = await httpClient.PostAsJsonAsync(urlCreate, newCartItemDTO);
            }

            // Lấy thông tin giỏ hàng sau cập nhật
            string urlCart = $"{cartUrl}/{userId}";
            HttpResponseMessage responseCart = await httpClient.GetAsync(urlCart);
            cartDTO = await responseCart.Content.ReadFromJsonAsync<CartDTO>();

            if (cartDTO != null)
            {
                cart = System.Text.Json.JsonSerializer.Serialize(cartDTO);
                HttpContext.Session.SetString("Cart", cart);
            }
            else
            {
                HttpContext.Session.SetString("Cart", null);
            }

            await LoadProductDetails(addToCartRequest.ProductId);
            return View("Index");
        }

        private async Task<ProductDTO> GetProductDetails(int productId)
        {
            string urlProduct = $"{productdetailUrl}/{productId}";
            HttpResponseMessage responseProduct = await httpClient.GetAsync(urlProduct);
            string strDataProduct = await responseProduct.Content.ReadAsStringAsync();
            var optionsProduct = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            return System.Text.Json.JsonSerializer.Deserialize<ProductDTO>(strDataProduct, optionsProduct);
        }

        private async Task LoadProductDetails(int productId)
        {
            string urlProductDetail = $"{productdetailUrl}/{productId}";
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
        }

    }
}

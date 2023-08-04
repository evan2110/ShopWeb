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
            var cart = HttpContext.Session.GetString("Cart");
            CartDTO cartDTO = null;
            //Neu Cart da co
            if (cart != null)
            {
                //Get Product vua add to cart
                string urlProduct = $"{productdetailUrl}/{addToCartRequest.ProductId}";
                HttpResponseMessage responseProduct = await httpClient.GetAsync(urlProduct);
                string strDataProduct = await responseProduct.Content.ReadAsStringAsync();
                var optionsProduct = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                var productDTO = System.Text.Json.JsonSerializer.Deserialize<ProductDTO>(strDataProduct, optionsProduct);

                cartDTO = System.Text.Json.JsonSerializer.Deserialize<CartDTO>(cart);
                string url = $"{cartItemUrl}?cartId={cartDTO.CartId}&productId={addToCartRequest.ProductId}&colorId={addToCartRequest.ColorId}&sizeId={addToCartRequest.SizeId}";
                HttpResponseMessage responseCartItem = await httpClient.GetAsync(url);
                string strDataCartItem = await responseCartItem.Content.ReadAsStringAsync();
                //Neu CartItem da ton tai
                if (!string.IsNullOrEmpty(strDataCartItem) && strDataCartItem.Trim().StartsWith("{"))
                {
                    var optionsCartItem = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };

                    CartItemDTO cartItemDTO = System.Text.Json.JsonSerializer.Deserialize<CartItemDTO>(strDataCartItem, optionsCartItem);
                    cartItemDTO.Quantity = cartItemDTO.Quantity + addToCartRequest.Quantity;

                    cartItemDTO.TotalPrice = ((productDTO.Price - (productDTO.Discount * productDTO.Price)) * cartItemDTO.Quantity);
                    string urlUpdate = $"{cartItemUrl}/{cartItemDTO.CartItemId}";
                    HttpResponseMessage response = await httpClient.PutAsJsonAsync(urlUpdate, cartItemDTO);
                }
                else
                {
                    decimal totalPrice = 0;
                    totalPrice = ((productDTO.Price - (productDTO.Discount * productDTO.Price)) * addToCartRequest.Quantity);

                    CartItemDTO newCartItemDTO = new CartItemDTO();
                    newCartItemDTO.CartId = cartDTO.CartId;
                    newCartItemDTO.ProductId = addToCartRequest.ProductId;
                    newCartItemDTO.Quantity = addToCartRequest.Quantity;
                    newCartItemDTO.TotalPrice = totalPrice;
                    newCartItemDTO.Status = "Active";
                    newCartItemDTO.CreatedTime = DateTime.Now;
                    newCartItemDTO.ColorId = addToCartRequest.ColorId;
                    newCartItemDTO.SizeId = addToCartRequest.SizeId;

                    string urlCreate = $"{cartItemUrl}";
                    HttpResponseMessage response = await httpClient.PostAsJsonAsync(urlCreate, newCartItemDTO);
                }

            }
            //Neu chua co cart
            else
            {
                CartDTO newCartDTO = new CartDTO();
                newCartDTO.UserId = Int32.Parse(HttpContext.Session.GetString("UserId"));
                newCartDTO.Status = "Active";
                newCartDTO.CreatedTime = DateTime.Now;
                string urlCreateCart = $"{cartUrl}";
                HttpResponseMessage response = await httpClient.PostAsJsonAsync(urlCreateCart, newCartDTO);
            }

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

                CartDTO cartFindDTO = System.Text.Json.JsonSerializer.Deserialize<CartDTO>(strDataCart, optionsCart);

                if (cartFindDTO != null)
                {
                    string cartJson = System.Text.Json.JsonSerializer.Serialize(cartFindDTO);
                    HttpContext.Session.SetString("Cart", cartJson);
                }
                else
                {
                    HttpContext.Session.SetString("Cart", null);

                }
            }
            await LoadProductDetails(addToCartRequest.ProductId);
            return View("Index");

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

using System.Diagnostics;
using System.Text;
using DataAccess.DTO;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using ShopWeb.Models;
using System.Text.Json;
using Microsoft.AspNetCore.Http;

namespace ShopWeb.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private static HttpClient httpClient;
    private static string productUrl = "https://localhost:7010/api/Product";
    private static string blogUrl = "https://localhost:7010/api/Blog";
    private static string cartUrl = "https://localhost:7010/api/Cart";



    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
        httpClient = new HttpClient();
    }

    public async Task<IActionResult> Index()
    {
        if (HttpContext.Session.GetString("UserId") == null)
        {
            HttpContext.Session.SetString("UserId", "0");
        }
        else
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

                if (cartDTO != null)
                {
                    string cartJson = System.Text.Json.JsonSerializer.Serialize(cartDTO);
                    HttpContext.Session.SetString("Cart", cartJson);
                }
                else
                {
                    HttpContext.Session.SetString("Cart", null);

                }
            }


        }
        try
        {
            string urlProduct = $"{productUrl}/list";
            string urlBlog = $"{blogUrl}/list";

            //GetTopProduct
            HttpResponseMessage responseProduct = await httpClient.GetAsync(urlProduct);
            string strDataProduct = await responseProduct.Content.ReadAsStringAsync();
            var optionsProduct = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            List<ProductDTO> listProduct = System.Text.Json.JsonSerializer.Deserialize<List<ProductDTO>>(strDataProduct, optionsProduct);
            ViewBag.listProductDTO = listProduct;

            //GetTopBlog
            HttpResponseMessage responseBlog = await httpClient.GetAsync(urlBlog);
            string strDataBlog = await responseBlog.Content.ReadAsStringAsync();
            var optionsBlog = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            List<BlogDTO> listBlog = System.Text.Json.JsonSerializer.Deserialize<List<BlogDTO>>(strDataBlog, optionsBlog);

            
            ViewBag.listBlogDTO = listBlog;
            return View("Index");
        }
        catch (Exception ex)
        {
            ViewBag.ErrorMessageLogin = "Đã có lỗi xảy ra khi đăng nhập";
            return View("Index");
        }
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
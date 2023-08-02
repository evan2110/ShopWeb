using System.Diagnostics;
using System.Text;
using DataAccess.DTO;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using ShopWeb.Models;
using System.Text.Json;

namespace ShopWeb.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private static HttpClient httpClient;
    private static string productUrl = "https://localhost:7010/api/Product";
    private static string blogUrl = "https://localhost:7010/api/Blog";


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
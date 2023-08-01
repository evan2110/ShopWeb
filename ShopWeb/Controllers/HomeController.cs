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
    private static string baseApiUrl = "https://localhost:7010/api/Product";

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
            string url = $"{baseApiUrl}/list";

            HttpResponseMessage response = await httpClient.GetAsync(url);
            string strData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            List<ProductDTO> list = System.Text.Json.JsonSerializer.Deserialize<List<ProductDTO>>(strData, options);
            ViewBag.listProductDTO = list;
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
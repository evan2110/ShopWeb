using Microsoft.AspNetCore.Mvc;

namespace ShopWeb.Controllers
{
    public class LoginController : Controller
    {
        private static HttpClient httpClient;
        private static string baseApiUrl = "https://localhost:7010/api/User";

        public LoginController()
        {
            httpClient = new HttpClient();
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}

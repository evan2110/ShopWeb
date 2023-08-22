using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ShoppingWebAPI.Request;
using System.Runtime.InteropServices;
using System.Text;

namespace ShopWeb.Controllers
{
    public class RegisterController : Controller
    {
        private static HttpClient httpClient;
        private static string baseApiUrl = "https://localhost:7010/api/User";
        public RegisterController()
        {
            httpClient = new HttpClient();
        }

        public IActionResult Index()
        {
            return RedirectToAction("Index", "Login");
		}

        [HttpPost]
        public IActionResult Register(UserRegisterRequest registerRequest)
        {
            if (registerRequest != null && ModelState.IsValid)
            {
                try
                {
                    var request = new HttpRequestMessage
                    {
                        Method = HttpMethod.Post,
                        RequestUri = new Uri($"{baseApiUrl}/register"),
                        Content = new StringContent(JsonConvert.SerializeObject(registerRequest), Encoding.UTF8, "application/json")
                    };
                    var resp = httpClient.Send(request);
                    dynamic json = JObject.Parse(resp.Content.ReadAsStringAsync().GetAwaiter().GetResult());
                    if (resp.IsSuccessStatusCode)
                    {
                        TempData["SuccessMessage"] = "Register susscess!";
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Wrong something!";
                    }
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = "Wrong something !";
                }
                return RedirectToAction("Index", "Login");
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }
    }
}

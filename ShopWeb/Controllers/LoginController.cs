using DataAccess.DTO;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Text;
using ShoppingWebAPI.Request;

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
            if (TempData.ContainsKey("SuccessMessage"))
            {
                // Lấy thông báo thành công từ TempData và gán vào ViewBag
                ViewBag.SuccessMessage = TempData["SuccessMessage"];
            }

            // Kiểm tra TempData có chứa thông báo lỗi không
            if (TempData.ContainsKey("ErrorMessage"))
            {
                // Lấy thông báo lỗi từ TempData và gán vào ViewBag
                ViewBag.ErrorMessage = TempData["ErrorMessage"];
            }
            return View();
        }

        [HttpPost]
        public IActionResult Login(UserRegisterRequest loginRequest)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var request = new HttpRequestMessage
                    {
                        Method = HttpMethod.Post,
                        RequestUri = new Uri($"{baseApiUrl}/login"),
                        Content = new StringContent(JsonConvert.SerializeObject(loginRequest), Encoding.UTF8, "application/json")
                    };
                    var resp = httpClient.Send(request);
                    dynamic json = JObject.Parse(resp.Content.ReadAsStringAsync().GetAwaiter().GetResult());
                    if (resp.IsSuccessStatusCode)
                    {
                        UserDTO userDTO = JsonConvert.DeserializeObject<UserDTO>(Convert.ToString(json.result));
                        HttpContext.Session.SetString("UserId", userDTO.UserId.ToString());
                        HttpContext.Session.SetString("FullName", userDTO.FirstName + userDTO.LastName);
                        HttpContext.Session.SetString("Role", userDTO.RoleId.ToString());
                        HttpContext.Session.SetString("Token", (string)json.token);
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ViewBag.ErrorMessageLogin = (string)json.errorMessages;
                    }
                    return View("Index");
                }
                catch (Exception ex)
                {
                    ViewBag.ErrorMessageLogin = "Đã có lỗi xảy ra khi đăng nhập";
                    return View("Index");
                }
            }
            ViewBag.ErrorMessageLogin = "Dữ liệu không hợp lệ";
            return View("Index");
        }

        [HttpPost]
        public IActionResult Logout()
        {
            if (HttpContext.Session.IsAvailable)
            {
                HttpContext.Session.Clear();
            }
            return View("Index");
        }
    }
}

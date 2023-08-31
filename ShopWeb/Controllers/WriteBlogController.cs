using DataAccess.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ShoppingWebAPI.Request;
using System.Net.Http.Headers;
using System.Text.Json;

namespace ShopWeb.Controllers
{
    public class WriteBlogController : Controller
    {
        private static HttpClient httpClient;
        private static string categoryUrl = "https://localhost:7010/api/Category";
        private static string blogUrl = "https://localhost:7010/api/Blog";

        public WriteBlogController()
        {
            httpClient = new HttpClient();
        }
        public async Task<IActionResult> Index(string? status)
        {
			if (HttpContext.Session.GetString("UserId") != null)
			{
				//Lay Category
				string urlGetCategory = $"{categoryUrl}/getAll";
                HttpResponseMessage responseCategory = await httpClient.GetAsync(urlGetCategory);
                string strCategory = await responseCategory.Content.ReadAsStringAsync();
                var optionsCategory = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                List<CategoryDTO> categories = System.Text.Json.JsonSerializer.Deserialize<List<CategoryDTO>>(strCategory, optionsCategory);
                ViewBag.categories = new SelectList(categories, "CategoryId", "CategoryName");
                if (status != null)
                {
                    ViewBag.status = "Your blog was updated success !";
                }
                return View();
            }
            else
            {
                return View("Error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateBlog(BlogDTO blogDTO)
        {
            if (HttpContext.Session.GetString("UserId") != null)
            {
                blogDTO.Status = "Active";
                blogDTO.CreatedTime = DateTime.Now;

                string urlCreateBlog = $"{blogUrl}";

                //Lay token tu session
                string token = HttpContext.Session.GetString("Token");
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage response = await httpClient.PostAsJsonAsync(urlCreateBlog, blogDTO);
                Console.WriteLine(response);
                return RedirectToAction("Index", new { status = "success" });
            }
            else
            {
				return View("Error");
			}
        }
    }
}

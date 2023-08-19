using DataAccess.DTO;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace ShopWeb.Controllers
{
    public class BlogController : Controller
    {
        private static HttpClient httpClient;
        private static string blogUrl = "https://localhost:7010/api/Blog";
        private static string categoryUrl = "https://localhost:7010/api/Category";

        public BlogController()
        {
            httpClient = new HttpClient();
        }
        public async Task<IActionResult> Index(int? categoryId, string? search, int pageNumber = 1, int pageSize = 4)
        {
            string urlGetAllBlog;
            urlGetAllBlog = $"{blogUrl}/getAll?pageSize={pageSize}&pageNumber={pageNumber}&categoryId={categoryId}&search={search}";
            HttpResponseMessage responseBlog = await httpClient.GetAsync(urlGetAllBlog);
            string strDataBlog = await responseBlog.Content.ReadAsStringAsync();
            var optionsBlog = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            List<BlogDTO> blog = System.Text.Json.JsonSerializer.Deserialize<List<BlogDTO>>(strDataBlog, optionsBlog);

            //Lay totalPage
            string urlGetTotalBlog = $"{blogUrl}/getAll?pageSize=0&pageNumber=1&categoryId={categoryId}&search={search}";
            HttpResponseMessage responseTotalBlog = await httpClient.GetAsync(urlGetTotalBlog);
            string strTotalBlog = await responseTotalBlog.Content.ReadAsStringAsync();
            var optionsTotalBlog = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            List<BlogDTO> totalBlog = System.Text.Json.JsonSerializer.Deserialize<List<BlogDTO>>(strTotalBlog, optionsTotalBlog);
            ViewBag.blogs = blog;
            ViewBag.pageNumber = pageNumber;
            ViewBag.pageSize = pageSize;
            if (blog != null && blog.Count > 0)
            {
                ViewBag.ToTalPage = (int)Math.Ceiling((double)totalBlog.Count / pageSize);
            }
            else
            {

                ViewBag.TotalPage = 1;
            }

            //Lay Category
            string urlGetCategory = $"{categoryUrl}/getAll";
            HttpResponseMessage responseCategory = await httpClient.GetAsync(urlGetCategory);
            string strCategory = await responseCategory.Content.ReadAsStringAsync();
            var optionsCategory = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            List<CategoryDTO> categories = System.Text.Json.JsonSerializer.Deserialize<List<CategoryDTO>>(strCategory, optionsCategory);

            //GetTopBlog
            string urlGetTopBlog = $"{blogUrl}/list";
            HttpResponseMessage responseTopBlog = await httpClient.GetAsync(urlGetTopBlog);
            string strDataTopBlog = await responseTopBlog.Content.ReadAsStringAsync();
            var optionsTopBlog = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            List<BlogDTO> listTopBlog = System.Text.Json.JsonSerializer.Deserialize<List<BlogDTO>>(strDataTopBlog, optionsTopBlog);


            ViewBag.listTopBlog = listTopBlog;
            ViewBag.categories = categories;
            ViewBag.categoryId = categoryId;
            ViewBag.search = search;
            return View();
        }
    }
}

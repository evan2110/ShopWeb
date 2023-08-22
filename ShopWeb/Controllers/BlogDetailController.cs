using DataAccess.DTO;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text.Json;

namespace ShopWeb.Controllers
{
    public class BlogDetailController : Controller
    {
        private static string blogUrl = "https://localhost:7010/api/Blog";
        private static string categoryUrl = "https://localhost:7010/api/Category";

        private static HttpClient httpClient;
        public BlogDetailController()
        {
            httpClient = new HttpClient();
        }
        public async Task<IActionResult> Index(int blogId)
        {
			try
			{
				//GetBlog
				string urlBlog = $"{blogUrl}/{blogId}";
				HttpResponseMessage responseBlog = await httpClient.GetAsync(urlBlog);
				string strDataBlog = await responseBlog.Content.ReadAsStringAsync();
				var optionsBlog = new JsonSerializerOptions
				{
					PropertyNameCaseInsensitive = true
				};
				BlogDTO blog = System.Text.Json.JsonSerializer.Deserialize<BlogDTO>(strDataBlog, optionsBlog);
				ViewBag.blog = blog;

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

				//GetRelatedBlog
				string urlGetRelatedBlog = $"{blogUrl}/lstRelate?categoryId={blog.CategoryId}";
				HttpResponseMessage responseRelatedBlog = await httpClient.GetAsync(urlGetRelatedBlog);
				string strDataRelatedBlog = await responseRelatedBlog.Content.ReadAsStringAsync();
				var optionsRelatedBlog = new JsonSerializerOptions
				{
					PropertyNameCaseInsensitive = true
				};

				List<BlogDTO> listRelatedBlog = System.Text.Json.JsonSerializer.Deserialize<List<BlogDTO>>(strDataRelatedBlog, optionsRelatedBlog);

				ViewBag.listRelatedBlog = listRelatedBlog;

				return View();
			}catch(Exception ex)
			{
				return View("Error");
			}
				
        }
    }
}

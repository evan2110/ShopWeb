using Microsoft.AspNetCore.Mvc;

namespace ShopWeb.Controllers
{
    public class AboutController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

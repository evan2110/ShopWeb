using Microsoft.AspNetCore.Mvc;

namespace ShopWeb.Controllers
{
    public class OrderController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

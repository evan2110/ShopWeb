using Microsoft.AspNetCore.Mvc;

namespace ShopWeb.Controllers
{
    public class ContactController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

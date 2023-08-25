using BusinessObject.Models;
using DataAccess.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ShoppingWebAPI.Request;
using System.Reflection.Metadata;
using System.Text.Json;
using Twilio;
using Twilio.TwiML.Voice;

namespace ShopWeb.Controllers
{
    public class SupportController : Controller
    {
        private static HttpClient httpClient;
        private static string supportlUrl = "https://localhost:7010/api/Support";

        public SupportController()
        {
            httpClient = new HttpClient();
        }

        public async Task<IActionResult> Index(int? roomId, int pageNumber = 1, int pageSize = 12)
        {
            if(HttpContext.Session.GetString("Role") != null)
            {
                string urlGetSupport = $"{supportlUrl}/getAll";
                HttpResponseMessage responseGetSupport = await httpClient.GetAsync(urlGetSupport);
                string strDataGetSupport = await responseGetSupport.Content.ReadAsStringAsync();
                var optionsGetSupport = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                var supports = System.Text.Json.JsonSerializer.Deserialize<List<SupportDTO>>(strDataGetSupport, optionsGetSupport);

                ViewBag.supports = supports;
            }
            else
            {
                return View("unAuthen");
            }
            return View();

        }

        [HttpPost]
        public async Task<IActionResult> SaveMessageInSession([FromBody] MessageDTO messageData)
        {
            
            var supportDTO = new SupportDTO();

            supportDTO.UserId = Int32.Parse(HttpContext.Session.GetString("UserId"));
            supportDTO.Message = $"{messageData.TimeSend} - {messageData.UserSupport}: {messageData.Message}";
            supportDTO.Status = "Active";
            supportDTO.CreatedTime = DateTime.Now;

            HttpResponseMessage response = await httpClient.PostAsJsonAsync(supportlUrl, supportDTO);
            return Json(new { success = true });
        }

    }
}

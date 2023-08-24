using BusinessObject.Models;
using DataAccess.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ShoppingWebAPI.Request;
using System.Text.Json;
using Twilio;
using Twilio.TwiML.Voice;

namespace ShopWeb.Controllers
{
    public class SupportController : Controller
    {
        private static HttpClient httpClient;
        private static string roomlUrl = "https://localhost:7010/api/Room";
        private static string supportlUrl = "https://localhost:7010/api/Support";

        public SupportController()
        {
            httpClient = new HttpClient();
        }

        public async Task<IActionResult> Index(int? roomId)
        {
            if(HttpContext.Session.GetString("Role") == "6")
            {
                int userId = Int32.Parse(HttpContext.Session.GetString("UserId"));
                //User
                string urlGetRoomByUserId = $"{roomlUrl}?user_id={userId}";
                HttpResponseMessage responseGetRoomByUserId = await httpClient.GetAsync(urlGetRoomByUserId);
                string strDataGetRoomByUserId = await responseGetRoomByUserId.Content.ReadAsStringAsync();
                var optionsGetRoomByUserId = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                var roomFind = System.Text.Json.JsonSerializer.Deserialize<List<RoomDTO>>(strDataGetRoomByUserId, optionsGetRoomByUserId);

                string urlGetSupport = "";

                if (roomFind.Count == 0)
                {
                    RoomDTO roomDTO = new RoomDTO();
                    roomDTO.UserId = userId;
                    roomDTO.Status = "Active";
                    roomDTO.CreatedTime = DateTime.Now;
                    HttpResponseMessage response = await httpClient.PostAsJsonAsync(roomlUrl, roomDTO);
                    RoomDTO roomCreated = JsonConvert.DeserializeObject<RoomDTO>(response.Content.ReadAsStringAsync().Result);
                    urlGetSupport = $"{supportlUrl}?room_id={roomCreated.RoomId}";
                }
                else
                {
                    urlGetSupport = $"{supportlUrl}?room_id={roomFind[0].RoomId}";
                }
                HttpResponseMessage responseGetSupport = await httpClient.GetAsync(urlGetSupport);
                string strDataGetSupport = await responseGetSupport.Content.ReadAsStringAsync();
                var optionsGetSupport = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                var supports = System.Text.Json.JsonSerializer.Deserialize<List<SupportDTO>>(strDataGetSupport, optionsGetSupport);

                ViewBag.supports = supports;
                ViewBag.roomFind = roomFind;
            }
            else if(HttpContext.Session.GetString("Role") == "3")
            {
                //Admin
                string urlGetRooms = $"{roomlUrl}/getAll?pageSize=0&pageNumber=1";
                HttpResponseMessage responseRooms = await httpClient.GetAsync(urlGetRooms);
                string strDataRooms = await responseRooms.Content.ReadAsStringAsync();
                var optionsRooms = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                var lstRooms = System.Text.Json.JsonSerializer.Deserialize<List<RoomDTO>>(strDataRooms, optionsRooms);

                if(roomId != null)
                {
                    string urlGetSupport = $"{supportlUrl}?room_id={roomId}";
                    HttpResponseMessage responseGetSupport = await httpClient.GetAsync(urlGetSupport);
                    string strDataGetSupport = await responseGetSupport.Content.ReadAsStringAsync();
                    var optionsGetSupport = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };
                    var supports = System.Text.Json.JsonSerializer.Deserialize<List<SupportDTO>>(strDataGetSupport, optionsGetSupport);
                    ViewBag.supports = supports;
                }
                ViewBag.lstRooms = lstRooms;

            }
            else
            {
                return View("error");
            }
            return View();

        }

        [HttpPost]
        public async Task<IActionResult> SaveMessageInSession([FromBody] MessageDTO messageData)
        {
            
            //Lay room cua user day
            int userId = Int32.Parse(HttpContext.Session.GetString("UserId"));
            string urlGetRoomByUserId = $"{roomlUrl}?user_id={userId}";
            HttpResponseMessage responseGetRoomByUserId = await httpClient.GetAsync(urlGetRoomByUserId);
            string strDataGetRoomByUserId = await responseGetRoomByUserId.Content.ReadAsStringAsync();
            var optionsGetRoomByUserId = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var roomFind = System.Text.Json.JsonSerializer.Deserialize<List<RoomDTO>>(strDataGetRoomByUserId, optionsGetRoomByUserId);
            var supportDTO = new SupportDTO();
            if (HttpContext.Session.GetString("Role") == "3")
            {
                supportDTO.RoomId = Int32.Parse(messageData.RoomId);
            }
            else
            {
                supportDTO.RoomId = roomFind[0].RoomId;
            }
            supportDTO.Message = $"{messageData.TimeSend} - {messageData.UserSupport}: {messageData.Message}";
            supportDTO.Status = "Active";
            supportDTO.CreatedTime = DateTime.Now;

            HttpResponseMessage response = await httpClient.PostAsJsonAsync(supportlUrl, supportDTO);
            return Json(new { success = true });
        }

    }
}

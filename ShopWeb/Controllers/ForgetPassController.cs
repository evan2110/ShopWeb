using DataAccess.DTO;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text.Json;
using System.Text;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace ShopWeb.Controllers
{
    public class ForgetPassController : Controller
    {
        private static string userUrl = "https://localhost:7010/api/User";
        private static HttpClient httpClient;
        public ForgetPassController()
        {
            httpClient = new HttpClient();
        }

        public IActionResult Index(string? status)
        {
            if (status == "fail")
            {
                ViewBag.status = "Number phone can't found";
            }
            else if(status == "success")
            {
                ViewBag.status = "Your new password was sent";
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgetPass(string Phone)
        {
            //Lay all user de check phone
            string urlGetUser = $"{userUrl}/getAll?pageNumber=1&&pageSize=0";

            HttpResponseMessage responseUser = await httpClient.GetAsync(urlGetUser);
            string strUser = await responseUser.Content.ReadAsStringAsync();
            var optionsUser = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            UserDTO userDTO = new UserDTO();

            List<UserDTO> users = System.Text.Json.JsonSerializer.Deserialize<List<UserDTO>>(strUser, optionsUser);
            bool check = false;
            foreach(var item in users)
            {
                if(item.Phone == Phone)
                {
                    check = true;
                    userDTO = item;
                }
            }

            if(check == false)
            {
                return RedirectToAction("Index", new { status = "fail" });
            }

            int index = Phone.IndexOf('0');
            if (index >= 0)
            {
                Phone = Phone.Substring(0, index) + "+84" + Phone.Substring(index + 1);
            }
            string newpass = GenerateVerificationCode();
            var accountSid = "ACef875dab95bd8f3737e10358fe6311af";
            var authToken = "c41c241aa0fafa287a4767a2beebfaa5";
            TwilioClient.Init(accountSid, authToken);
            Console.WriteLine(newpass);

            var message = MessageResource.Create(to: new Twilio.Types.PhoneNumber(Phone),
            from: new Twilio.Types.PhoneNumber("+14179003335"),
                body: "Hello " + DateTime.Now.ToString("MM/dd/yyyy hh:mm tt") + $"/nNew password is: {newpass}");
            userDTO.Password = BCrypt.Net.BCrypt.HashPassword(newpass);

            string urlUpdateUser = $"{userUrl}/{userDTO.UserId}";
            HttpResponseMessage response = await httpClient.PutAsJsonAsync(urlUpdateUser, userDTO);
            Console.WriteLine(response);

            return RedirectToAction("Index", new { status = "success" });
        }

        public static string GenerateVerificationCode()
        {
            int codeLength = 4;
            string characters = "0123456789";
            StringBuilder codeBuilder = new StringBuilder();

            Random random = new Random();
            for (int i = 0; i < codeLength; i++)
            {
                int index = random.Next(characters.Length);
                codeBuilder.Append(characters[index]);
            }

            return codeBuilder.ToString();
        }
    }
}

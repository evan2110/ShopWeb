using Azure.Core;
using BusinessObject.Models;
using DataAccess.DTO;
using DataAccess.Repository;
using Microsoft.AspNetCore.Mvc;
using ShoppingWebAPI.Config;
using ShoppingWebAPI.Request;
using System.Drawing.Printing;
using System.Security.Policy;
using System.Text.Json;

namespace ShopWeb.Controllers
{
    public class UserDetailController : Controller
    {
        private static HttpClient httpClient;
        private static string userUrl = "https://localhost:7010/api/User";

        public UserDetailController()
        {
            httpClient = new HttpClient();
        }

        public async Task<IActionResult> Index(int user_id)
        {
            UserDTO user = await GetUser(user_id);
            ViewBag.user = user;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UpdateUser(UserDTO user)
        {
            UserDTO userResult = await GetUser(user.UserId);

            if (user.Password != userResult.Password)
            {
                user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
            }
            user.Gender = user.Gender == "True" ? "Male" : "Female";
            user.Status = "Active";
            user.UpdatedTime = DateTime.Now;

            string url = $"{userUrl}/{user.UserId}";
            HttpResponseMessage response = await httpClient.PutAsJsonAsync(url, user);
            Console.WriteLine(response);

            return RedirectToAction("Index", new { user_id = user.UserId });
        }

        private async Task<UserDTO> GetUser(int userId)
        {
            string urlGetUser;
            urlGetUser = $"{userUrl}/{userId}";
            HttpResponseMessage responseUser = await httpClient.GetAsync(urlGetUser);
            string strDataUser = await responseUser.Content.ReadAsStringAsync();
            var optionsUser = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            UserDTO user = System.Text.Json.JsonSerializer.Deserialize<UserDTO>(strDataUser, optionsUser);
            return user;
        }

    }
}

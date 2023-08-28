﻿using BusinessObject.Models;
using DataAccess.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Net.Http.Headers;
using System.Reflection.Metadata;
using System.Text.Json;
using Twilio.TwiML.Voice;

namespace ShopWeb.Controllers
{
    public class DashboardController : Controller
    {
        private static HttpClient httpClient;
        private static string categoryUrl = "https://localhost:7010/api/Category";
        private static string orderDetailUrl = "https://localhost:7010/api/OrderDetail";
        private static string userUrl = "https://localhost:7010/api/User";
        private static string rateUrl = "https://localhost:7010/api/Rate";
        private static string supportUrl = "https://localhost:7010/api/Support";
        private static string orderUrl = "https://localhost:7010/api/Order";
        private static string blogUrl = "https://localhost:7010/api/Blog";




        public DashboardController()
        {
            httpClient = new HttpClient();
        }

        public async Task<IActionResult> Index(string? Mode, int id, int pageNumber = 1, int pageSize = 10)
        {
            if (HttpContext.Session.GetString("Role") == "3")
            {
                int userId = Int32.Parse(HttpContext.Session.GetString("UserId"));
                

                UserDTO user = await GetUser(userId);

                if (Mode == "Dashboard" || Mode == null)
                {
                    //Thống kê % sản phẩm thuộc mỗi category chiếm trên tổng số product
                    Dictionary<string,double> numberProductsEachCategory = new Dictionary<string,double>();
                    int totalProduct = 0;
                    string urlGetCategory = $"{categoryUrl}/getAll";
                    HttpResponseMessage responseGetCategory = await httpClient.GetAsync(urlGetCategory);
                    string strDataGetCategory = await responseGetCategory.Content.ReadAsStringAsync();
                    var optionsGetCategory = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };
                    var categories = System.Text.Json.JsonSerializer.Deserialize<List<CategoryDTO>>(strDataGetCategory, optionsGetCategory);
                    foreach(var item in categories)
                    {
                        numberProductsEachCategory.Add(item.CategoryName, item.Products.Count());
                        totalProduct += item.Products.Count();
                    }

                    foreach (var item in numberProductsEachCategory.ToList())
                    {
                        double percentage = (item.Value / totalProduct) * 100;
                        percentage = Math.Round(percentage, 2);
                        numberProductsEachCategory[item.Key] = percentage;
                    }

                    //top 10 sản phẩm được mua nhiều nhất
                    Dictionary<string, double> topProductSelled = new Dictionary<string, double>();
                    string urlTopBuyProduct = $"{orderDetailUrl}/topBuyProduct";
                    HttpResponseMessage responseTopBuyProduct = await httpClient.GetAsync(urlTopBuyProduct);
                    string strDataTopBuyProduct = await responseTopBuyProduct.Content.ReadAsStringAsync();
                    var optionsTopBuyProduct = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };

                    List<TopBuyProductDTO> listTopBuyProduct = System.Text.Json.JsonSerializer.Deserialize<List<TopBuyProductDTO>>(strDataTopBuyProduct, optionsTopBuyProduct);
                    foreach(var item in listTopBuyProduct)
                    {
                        topProductSelled.Add(item.ProductName, Math.Round((double)(item.Quantity * item.Price), 2));
                    }

                    ViewBag.NumberProductsEachCategory = numberProductsEachCategory;
                    ViewBag.topProductSelled = topProductSelled;
                    ViewBag.Mode = "Dashboard";
                }else if(Mode == "User")
                {
                    string urlGetAllUser;
                    urlGetAllUser = $"{userUrl}/getAllForAdmin?pageSize={pageSize}&pageNumber={pageNumber}";
                    HttpResponseMessage responseUser = await httpClient.GetAsync(urlGetAllUser);
                    string strDataUser = await responseUser.Content.ReadAsStringAsync();
                    var optionsUser = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };

                    List<UserDTO> users = System.Text.Json.JsonSerializer.Deserialize<List<UserDTO>>(strDataUser, optionsUser);

                    //Lay totalPage
                    string urlGetTotalUser = $"{userUrl}/getAllForAdmin?pageSize=0&pageNumber=1";
                    HttpResponseMessage responseTotalUser = await httpClient.GetAsync(urlGetTotalUser);
                    string strTotalUser = await responseTotalUser.Content.ReadAsStringAsync();
                    var optionsTotalUser = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };

                    List<UserDTO> totalUser = System.Text.Json.JsonSerializer.Deserialize<List<UserDTO>>(strTotalUser, optionsTotalUser);

                    ViewBag.users = users;
                    ViewBag.pageNumber = pageNumber;
                    ViewBag.pageSize = pageSize;
                    if (totalUser != null && totalUser.Count > 0)
                    {
                        ViewBag.ToTalPage = (int)Math.Ceiling((double)totalUser.Count / pageSize);
                    }
                    else
                    {
                        ViewBag.TotalPage = 1;
                    }
                    ViewBag.Mode = "User";
                }else if(Mode == "CreateUser")
                {
                    ViewBag.Mode = "CreateUser";
                    if(id != 0)
                    {
                        UserDTO userDetail = await GetUser(id);

                        ViewBag.userDetail = userDetail;
                    }
                }else if(Mode == "DeleteUser")
                {
                    UserDTO userResult = await GetUser(id);

                    userResult.Status = "Deactive";
                    userResult.UpdatedTime = DateTime.Now;
                    string url = $"{userUrl}/{id}";

                    HttpResponseMessage response = await httpClient.PutAsJsonAsync(url, userResult);
                    ChangeSkateholdersWhenChangeUser(id, "Deactive");

                    Response.Redirect("Dashboard?Mode=User");
                }

                ViewBag.user = user;
                return View();

            }
            else
            {
                return View("unAuthen");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateAndUpdate(UserDTO? userDTO)
        {
            if(userDTO.Email != null)
            {
                if(userDTO.UserId != 0)
                {
                    UserDTO userResult = await GetUser(userDTO.UserId);
                    Console.WriteLine(userDTO.UserId);
                    if (userDTO.Password != userResult.Password)
                    {
                        userDTO.Password = BCrypt.Net.BCrypt.HashPassword(userDTO.Password);
                    }
                    if(userDTO.Status == "Deactive")
                    {
                        Console.WriteLine("Deactive");
                        ChangeSkateholdersWhenChangeUser(userDTO.UserId, userDTO.Status);
                    }
                    else if(userDTO.Status == "Active")
                    {
                        Console.WriteLine("Active");
                        ChangeSkateholdersWhenChangeUser(userDTO.UserId, userDTO.Status);
                    }
                    userDTO.RoleId = userResult.RoleId;
                    userDTO.RoleName = userResult.RoleName;
                    userDTO.CreatedTime = userResult.CreatedTime;
                    userDTO.UpdatedTime = DateTime.Now;
                    string url = $"{userUrl}/{userDTO.UserId}";
              
                    HttpResponseMessage response = await httpClient.PutAsJsonAsync(url, userDTO);
                    return RedirectToAction("Index", new { Mode = "CreateUser" , id = userDTO .UserId});

                }
                else
                {
                    userDTO.RoleId = 6;
                    userDTO.RoleName = "NORMAL";
                    userDTO.Password = BCrypt.Net.BCrypt.HashPassword(userDTO.Password);
                    userDTO.CreatedTime = DateTime.Now;
                    string url = $"{userUrl}";

                    //Lay token tu session
                    string token = HttpContext.Session.GetString("Token");
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                    HttpResponseMessage response = await httpClient.PostAsJsonAsync(url, userDTO);
                    return RedirectToAction("Index", new { Mode = "CreateUser" });
                }
            }
            else
            {
                return View("Error");
            }
        }

        private async Task<UserDTO> GetUser(int userId)
        {
            string urlGetUser;
            urlGetUser = $"{userUrl}/{userId}";

            //Lay token tu session
            string token = HttpContext.Session.GetString("Token");
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            HttpResponseMessage responseUser = await httpClient.GetAsync(urlGetUser);
            string strDataUser = await responseUser.Content.ReadAsStringAsync();
            var optionsUser = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            UserDTO user = System.Text.Json.JsonSerializer.Deserialize<UserDTO>(strDataUser, optionsUser);
            return user;
        }

        private async Task<IActionResult> ChangeSkateholdersWhenChangeUser(int id, string status)
        {
            //Deactive Rate
            string urlGetRate;
            urlGetRate = $"{rateUrl}/getAll";

            HttpResponseMessage responseRate = await httpClient.GetAsync(urlGetRate);
            string strDataRate = await responseRate.Content.ReadAsStringAsync();
            var optionsRate = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var rates = System.Text.Json.JsonSerializer.Deserialize<List<RateDTO>>(strDataRate, optionsRate);
            foreach (var item in rates)
            {
                if (item.UserId == id)
                {
                    item.Status = status;
                    item.UpdatedTime = DateTime.Now;
                    string urlRate = $"{rateUrl}/{item.RateId}";
                    await httpClient.PutAsJsonAsync(urlRate, item);
                }
            }

            //Deactive Support
            string urlGetSupport;
            urlGetSupport = $"{supportUrl}/getAllForAdmin";

            HttpResponseMessage responseSupport = await httpClient.GetAsync(urlGetSupport);
            string strDataSupport = await responseSupport.Content.ReadAsStringAsync();
            var optionsSupport = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var supports = System.Text.Json.JsonSerializer.Deserialize<List<SupportDTO>>(strDataSupport, optionsSupport);
            foreach (var item in supports)
            {
                if (item.UserId == id)
                {
                    item.Status = status;
                    item.UpdatedTime = DateTime.Now;
                    string urlSupport = $"{supportUrl}/{item.SupportId}";
                    await httpClient.PutAsJsonAsync(urlSupport, item);
                }
            }

            //GetAllOrderDetail
            string urlGetOrderDetail;
            urlGetOrderDetail = $"{orderDetailUrl}/getAllForAdmin";
            HttpResponseMessage responseOrderDetail = await httpClient.GetAsync(urlGetOrderDetail);
            string strDataOrderDetail = await responseOrderDetail.Content.ReadAsStringAsync();
            var optionsOrderDetail = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var orderDetails = System.Text.Json.JsonSerializer.Deserialize<List<OrderDetailDTO>>(strDataOrderDetail, optionsOrderDetail);

            //Deactive Order
            string urlGetOrder;
            urlGetOrder = $"{orderUrl}/getAllForAdmin";

            HttpResponseMessage responseOrder = await httpClient.GetAsync(urlGetOrder);
            string strDataOrder = await responseOrder.Content.ReadAsStringAsync();
            var optionsOrder = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var orders = System.Text.Json.JsonSerializer.Deserialize<List<OrderDTO>>(strDataOrder, optionsOrder);
            foreach (var item in orders)
            {
                if (item.UserId == id)
                {
                    item.Status = status;
                    item.UpdatedTime = DateTime.Now;
                    string urlOrder = $"{orderUrl}/{item.OrderId}";
                    await httpClient.PutAsJsonAsync(urlOrder, item);

                    //Deactive OrderDetail
                    foreach (var items in orderDetails)
                    {
                        if(items.OrderId == item.OrderId)
                        {
                            items.Status = status;
                            items.UpdatedTime = DateTime.Now;
                            string urlOrderDetail = $"{orderDetailUrl}/{items.OrderDetailId}";
                            await httpClient.PutAsJsonAsync(urlOrderDetail, items);
                        }
                    }
                }
            }

            //Deactive Blog
            string urlGetBlog;
            urlGetBlog = $"{blogUrl}/getAllForAdmin";

            HttpResponseMessage responseBlog = await httpClient.GetAsync(urlGetBlog);
            string strDataBlog = await responseBlog.Content.ReadAsStringAsync();
            var optionsBlog = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var blogs = System.Text.Json.JsonSerializer.Deserialize<List<BlogDTO>>(strDataBlog, optionsBlog);
            foreach (var item in blogs)
            {
                if (item.UserId == id)
                {
                    item.Status = status;
                    item.UpdatedTime = DateTime.Now;
                    string urlBlog = $"{blogUrl}/{item.BlogId}";
                    HttpResponseMessage responseTopBuyProduct = await httpClient.PutAsJsonAsync(urlBlog, item);
                    Console.WriteLine(responseTopBuyProduct);
                }
            }
            return null;
        }

    }
}

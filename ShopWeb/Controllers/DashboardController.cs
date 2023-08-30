using BusinessObject.Models;
using DataAccess.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Stripe;
using System;
using System.Drawing;
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
        private static string productUrl = "https://localhost:7010/api/Product";
        private static string colorUrl = "https://localhost:7010/api/Color";
        private static string sizeUrl = "https://localhost:7010/api/Size";
        private static string productColorUrl = "https://localhost:7010/api/ProductColor";
        private static string productSizeUrl = "https://localhost:7010/api/ProductSize";

        public DashboardController()
        {
            httpClient = new HttpClient();
        }

        public async Task<IActionResult> Index(string? Mode, string? Mess, string? ColorMess, int id, int pageNumber = 1, int pageSize = 10)
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

                    //Lay tong so User(duoc active)
                    string urlGetTotalUser = $"{userUrl}/getAll?pageSize=0&pageNumber=1";
                    HttpResponseMessage responseTotalUser = await httpClient.GetAsync(urlGetTotalUser);
                    string strTotalUser = await responseTotalUser.Content.ReadAsStringAsync();
                    var optionsTotalUser = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };

                    List<UserDTO> totalUsers = System.Text.Json.JsonSerializer.Deserialize<List<UserDTO>>(strTotalUser, optionsTotalUser);

                    //Lay tong so Produc(duoc active)
                    string urlGetTotalProduct = $"{productUrl}/getAll?pageSize=0&pageNumber=1";
                    HttpResponseMessage responseTotalProduct = await httpClient.GetAsync(urlGetTotalProduct);
                    string strTotalProduct = await responseTotalProduct.Content.ReadAsStringAsync();
                    var optionsTotalProduct = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };

                    List<ProductDTO> totalProducts = System.Text.Json.JsonSerializer.Deserialize<List<ProductDTO>>(strTotalProduct, optionsTotalProduct);

                    //Lay tong so Blog(duoc active)
                    string urlGetTotalBlog = $"{blogUrl}/getAll?pageSize=0&pageNumber=1";
                    HttpResponseMessage responseTotalBlog = await httpClient.GetAsync(urlGetTotalBlog);
                    string strTotalBlog = await responseTotalBlog.Content.ReadAsStringAsync();
                    var optionsTotalBlog = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };

                    List<BlogDTO> totalBlogs = System.Text.Json.JsonSerializer.Deserialize<List<BlogDTO>>(strTotalBlog, optionsTotalBlog);

                    //Lay tong so Order(duoc active)
                    string urlGetTotalOrder = $"{orderUrl}/getAll?pageSize=0&pageNumber=1";
                    HttpResponseMessage responseTotalOrder = await httpClient.GetAsync(urlGetTotalOrder);
                    string strTotalOrder = await responseTotalOrder.Content.ReadAsStringAsync();
                    var optionsTotalOrder = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };

                    List<OrderDTO> totalOrders = System.Text.Json.JsonSerializer.Deserialize<List<OrderDTO>>(strTotalOrder, optionsTotalOrder);

                    ViewBag.totalOrders = totalOrders.Count;
                    ViewBag.totalBlogs = totalBlogs.Count;
                    ViewBag.totalProducts = totalProducts.Count;
                    ViewBag.totalUsers = totalUsers.Count;
                    ViewBag.NumberProductsEachCategory = numberProductsEachCategory;
                    ViewBag.topProductSelled = topProductSelled;
                    ViewBag.Mode = "Dashboard";
                }
                else if(Mode == "User")
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
                }
                else if(Mode == "CreateUser")
                {
                    ViewBag.Mode = "CreateUser";
                    if(id != 0)
                    {
                        UserDTO userDetail = await GetUser(id);

                        ViewBag.userDetail = userDetail;
                    }
                }
                else if(Mode == "DeleteUser")
                {
                    UserDTO userResult = await GetUser(id);

                    userResult.Status = "Deactive";
                    userResult.UpdatedTime = DateTime.Now;
                    string url = $"{userUrl}/{id}";

                    HttpResponseMessage response = await httpClient.PutAsJsonAsync(url, userResult);
                    ChangeSkateholdersWhenChangeUser(id, "Deactive");

                    Response.Redirect("Dashboard?Mode=User");
                }
                else if(Mode == "Category")
                {
                    string urlGetAllCategory;
                    urlGetAllCategory = $"{categoryUrl}/getAllForAdmin?pageSize={pageSize}&pageNumber={pageNumber}";
                    HttpResponseMessage responseCategory = await httpClient.GetAsync(urlGetAllCategory);
                    string strDataCategory = await responseCategory.Content.ReadAsStringAsync();
                    var optionsCategory = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };

                    List<CategoryDTO> categories = System.Text.Json.JsonSerializer.Deserialize<List<CategoryDTO>>(strDataCategory, optionsCategory);

                    //Lay totalPage
                    string urlGetTotalCategory = $"{categoryUrl}/getAllForAdmin?pageSize=0&pageNumber=1";
                    HttpResponseMessage responseTotalCategory = await httpClient.GetAsync(urlGetTotalCategory);
                    string strTotalCategory = await responseTotalCategory.Content.ReadAsStringAsync();
                    var optionsTotalCategory = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };

                    List<CategoryDTO> totalCategory = System.Text.Json.JsonSerializer.Deserialize<List<CategoryDTO>>(strTotalCategory, optionsTotalCategory);

                    ViewBag.categories = categories;
                    ViewBag.pageNumber = pageNumber;
                    ViewBag.pageSize = pageSize;
                    if (totalCategory != null && totalCategory.Count > 0)
                    {
                        ViewBag.ToTalPage = (int)Math.Ceiling((double)totalCategory.Count / pageSize);
                    }
                    else
                    {
                        ViewBag.TotalPage = 1;
                    }
                    ViewBag.Mode = "Category";
                }
                else if (Mode == "CreateCategory")
                {
                    ViewBag.Mode = "CreateCategory";
                    if (id != 0)
                    {
                        CategoryDTO categoryDetail = await GetCategory(id);

                        ViewBag.categoryDetail = categoryDetail;
                    }
                }
                else if (Mode == "DeleteCategory")
                {
                    CategoryDTO categoryDetail = await GetCategory(id);

                    categoryDetail.Status = "Deactive";
                    categoryDetail.UpdatedTime = DateTime.Now;
                    string url = $"{categoryUrl}/{id}";

                    HttpResponseMessage response = await httpClient.PutAsJsonAsync(url, categoryDetail);
                    ChangeSkateholdersWhenChangeCategory(id, "Deactive");

                    Response.Redirect("Dashboard?Mode=Category");
                }
                else if(Mode == "Product")
                {
                    string urlGetAllProduct;
                    urlGetAllProduct = $"{productUrl}/getAllForAdmin?pageSize={pageSize}&pageNumber={pageNumber}";
                    HttpResponseMessage responseProduct = await httpClient.GetAsync(urlGetAllProduct);
                    string strDataProduct = await responseProduct.Content.ReadAsStringAsync();
                    var optionsProduct = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };

                    List<ProductDTO> products = System.Text.Json.JsonSerializer.Deserialize<List<ProductDTO>>(strDataProduct, optionsProduct);

                    //Lay totalPage
                    string urlGetTotalProduct = $"{productUrl}/getAllForAdmin?pageSize=0&pageNumber=1";
                    HttpResponseMessage responseTotalProduct = await httpClient.GetAsync(urlGetTotalProduct);
                    string strTotalProduct = await responseTotalProduct.Content.ReadAsStringAsync();
                    var optionsTotalProduct = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };

                    List<ProductDTO> totalProduct = System.Text.Json.JsonSerializer.Deserialize<List<ProductDTO>>(strTotalProduct, optionsTotalProduct);

                    ViewBag.products = products;
                    ViewBag.pageNumber = pageNumber;
                    ViewBag.pageSize = pageSize;
                    if (totalProduct != null && totalProduct.Count > 0)
                    {
                        ViewBag.ToTalPage = (int)Math.Ceiling((double)totalProduct.Count / pageSize);
                    }
                    else
                    {
                        ViewBag.TotalPage = 1;
                    }
                    ViewBag.Mode = "Product";
                }
                else if (Mode == "CreateProduct")
                {
                    //Lay Category
                    string urlGetCategory = $"{categoryUrl}/getAll";
                    HttpResponseMessage responseCategory = await httpClient.GetAsync(urlGetCategory);
                    string strCategory = await responseCategory.Content.ReadAsStringAsync();
                    var optionsCategory = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };

                    List<CategoryDTO> categories = System.Text.Json.JsonSerializer.Deserialize<List<CategoryDTO>>(strCategory, optionsCategory);
                    ViewBag.categories = new SelectList(categories, "CategoryId", "CategoryName");

                    //Lay Color
                    string urlGetColor = $"{colorUrl}/getAll";
                    HttpResponseMessage responseColor = await httpClient.GetAsync(urlGetColor);
                    string strColor = await responseColor.Content.ReadAsStringAsync();
                    var optionsColor = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };

                    List<ColorDTO> colors = System.Text.Json.JsonSerializer.Deserialize<List<ColorDTO>>(strColor, optionsColor);
                    ViewBag.colors = new SelectList(colors, "ColorId", "ColorName");

                    //Lay Size
                    string urlGetSize = $"{sizeUrl}/getAll";
                    HttpResponseMessage responseSize = await httpClient.GetAsync(urlGetSize);
                    string strSize = await responseSize.Content.ReadAsStringAsync();
                    var optionsSize = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };

                    List<SizeDTO> sizes = System.Text.Json.JsonSerializer.Deserialize<List<SizeDTO>>(strSize, optionsSize);
                    ViewBag.sizes = new SelectList(sizes, "SizeId", "SizeName");

                    ViewBag.Mode = "CreateProduct";
                    if (id != 0)
                    {
                        ProductDTO productDetail = await GetProduct(id);
                        ViewBag.listColor = new SelectList(productDetail.ProductColorDTOs.ToList(), "ColorId", "ColorName");
                        ViewBag.listSize = new SelectList(productDetail.ProductSizeDTOs.ToList(), "SizeId", "SizeName");
                        ViewBag.productDetail = productDetail;
                    }
                }
                else if(Mode == "DeleteProduct")
                {
                    ProductDTO productDetail = await GetProduct(id);

                    productDetail.Status = "Deactive";
                    productDetail.UpdatedTime = DateTime.Now;
                    string url = $"{productUrl}/{id}";

                    HttpResponseMessage response = await httpClient.PutAsJsonAsync(url, productDetail);
                    ChangeSkateholdersWhenChangeProduct(id, "Deactive");

                    Response.Redirect("Dashboard?Mode=Product");
                }
                else if(Mode == "Blog")
                {
                    string urlGetAllBlog;
                    urlGetAllBlog = $"{blogUrl}/getAllForAdmin?pageSize={pageSize}&pageNumber={pageNumber}";
                    HttpResponseMessage responseBlog = await httpClient.GetAsync(urlGetAllBlog);
                    string strDataBlog = await responseBlog.Content.ReadAsStringAsync();
                    var optionsBlog = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };

                    List<BlogDTO> blogs = System.Text.Json.JsonSerializer.Deserialize<List<BlogDTO>>(strDataBlog, optionsBlog);

                    //Lay totalPage
                    string urlGetTotalBlog = $"{blogUrl}/getAllForAdmin?pageSize=0&pageNumber=1";
                    HttpResponseMessage responseTotalBlog = await httpClient.GetAsync(urlGetTotalBlog);
                    string strTotalBlog = await responseTotalBlog.Content.ReadAsStringAsync();
                    var optionsTotalBlog = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };

                    List<BlogDTO> totalBlog = System.Text.Json.JsonSerializer.Deserialize<List<BlogDTO>>(strTotalBlog, optionsTotalBlog);

                    ViewBag.blogs = blogs;
                    ViewBag.pageNumber = pageNumber;
                    ViewBag.pageSize = pageSize;
                    if (totalBlog != null && totalBlog.Count > 0)
                    {
                        ViewBag.ToTalPage = (int)Math.Ceiling((double)totalBlog.Count / pageSize);
                    }
                    else
                    {
                        ViewBag.TotalPage = 1;
                    }
                    ViewBag.Mode = "Blog";
                }
                else if(Mode == "CreateBlog")
                {
                    //Lay Category
                    string urlGetCategory = $"{categoryUrl}/getAll";
                    HttpResponseMessage responseCategory = await httpClient.GetAsync(urlGetCategory);
                    string strCategory = await responseCategory.Content.ReadAsStringAsync();
                    var optionsCategory = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };

                    List<CategoryDTO> categories = System.Text.Json.JsonSerializer.Deserialize<List<CategoryDTO>>(strCategory, optionsCategory);
                    ViewBag.categories = new SelectList(categories, "CategoryId", "CategoryName");

                    //Lay User
                    string urlGetUser = $"{userUrl}/getAll";
                    HttpResponseMessage responseUser = await httpClient.GetAsync(urlGetUser);
                    string strUser = await responseUser.Content.ReadAsStringAsync();
                    var optionsUser = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };

                    List<UserDTO> users = System.Text.Json.JsonSerializer.Deserialize<List<UserDTO>>(strUser, optionsUser);
                    ViewBag.users = new SelectList(users, "UserId", "Email");

                    ViewBag.Mode = "CreateBlog";
                    if (id != 0)
                    {
                        BlogDTO blogDetail = await GetBlog(id);
                        ViewBag.blogDetail = blogDetail;
                    }
                }
                else if(Mode == "DeleteBlog")
                {
                    BlogDTO blogDetail = await GetBlog(id);

                    blogDetail.Status = "Deactive";
                    blogDetail.UpdatedTime = DateTime.Now;
                    string url = $"{blogUrl}/{id}";

                    HttpResponseMessage response = await httpClient.PutAsJsonAsync(url, blogDetail);

                    Response.Redirect("Dashboard?Mode=Blog");
                }
                else if(Mode == "Color")
                {
                    string urlGetAllColor;
                    urlGetAllColor = $"{colorUrl}/getAllForAdmin?pageSize={pageSize}&pageNumber={pageNumber}";
                    HttpResponseMessage responseColor = await httpClient.GetAsync(urlGetAllColor);
                    string strDataColor = await responseColor.Content.ReadAsStringAsync();
                    var optionsColor = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };

                    List<ColorDTO> colors = System.Text.Json.JsonSerializer.Deserialize<List<ColorDTO>>(strDataColor, optionsColor);

                    //Lay totalPage
                    string urlGetTotalColor = $"{colorUrl}/getAllForAdmin?pageSize=0&pageNumber=1";
                    HttpResponseMessage responseTotalColor = await httpClient.GetAsync(urlGetTotalColor);
                    string strTotalColor = await responseTotalColor.Content.ReadAsStringAsync();
                    var optionsTotalColor = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };

                    List<ColorDTO> totalColor = System.Text.Json.JsonSerializer.Deserialize<List<ColorDTO>>(strTotalColor, optionsTotalColor);

                    ViewBag.colors = colors;
                    ViewBag.pageNumber = pageNumber;
                    ViewBag.pageSize = pageSize;
                    if (totalColor != null && totalColor.Count > 0)
                    {
                        ViewBag.ToTalPage = (int)Math.Ceiling((double)totalColor.Count / pageSize);
                    }
                    else
                    {
                        ViewBag.TotalPage = 1;
                    }
                    ViewBag.Mode = "Color";
                }
                else if(Mode == "CreateColor")
                {
                    ViewBag.Mode = "CreateColor";
                    if (id != 0)
                    {
                        ColorDTO colorDetail = await GetColor(id);
                        ViewBag.colorDetail = colorDetail;
                    }
                }
                else if(Mode == "DeleteColor")
                {
                    ColorDTO colorDetail = await GetColor(id);

                    colorDetail.Status = "Deactive";
                    colorDetail.UpdatedTime = DateTime.Now;
                    string url = $"{colorUrl}/{id}";

                    HttpResponseMessage response = await httpClient.PutAsJsonAsync(url, colorDetail);
                    ChangeSkateholdersWhenChangeColor(id, "Deactive");

                    Response.Redirect("Dashboard?Mode=Color");
                }
                else if (Mode == "Size")
                {
                    string urlGetAllSize;
                    urlGetAllSize = $"{sizeUrl}/getAllForAdmin?pageSize={pageSize}&pageNumber={pageNumber}";
                    HttpResponseMessage responseSize = await httpClient.GetAsync(urlGetAllSize);
                    string strDataSize = await responseSize.Content.ReadAsStringAsync();
                    var optionsSize = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };

                    List<SizeDTO> sizes = System.Text.Json.JsonSerializer.Deserialize<List<SizeDTO>>(strDataSize, optionsSize);

                    //Lay totalPage
                    string urlGetTotalSize = $"{sizeUrl}/getAllForAdmin?pageSize=0&pageNumber=1";
                    HttpResponseMessage responseTotalSize = await httpClient.GetAsync(urlGetTotalSize);
                    string strTotalSize = await responseTotalSize.Content.ReadAsStringAsync();
                    var optionsTotalSize = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };

                    List<SizeDTO> totalSize = System.Text.Json.JsonSerializer.Deserialize<List<SizeDTO>>(strTotalSize, optionsTotalSize);

                    ViewBag.sizes = sizes;
                    ViewBag.pageNumber = pageNumber;
                    ViewBag.pageSize = pageSize;
                    if (totalSize != null && totalSize.Count > 0)
                    {
                        ViewBag.ToTalPage = (int)Math.Ceiling((double)totalSize.Count / pageSize);
                    }
                    else
                    {
                        ViewBag.TotalPage = 1;
                    }
                    ViewBag.Mode = "Size";
                }
                else if (Mode == "CreateSize")
                {
                    ViewBag.Mode = "CreateSize";
                    if (id != 0)
                    {
                        SizeDTO sizeDetail = await GetSize(id);
                        ViewBag.sizeDetail = sizeDetail;
                    }
                }
                else if (Mode == "DeleteSize")
                {
                    SizeDTO sizeDetail = await GetSize(id);

                    sizeDetail.Status = "Deactive";
                    sizeDetail.UpdatedTime = DateTime.Now;
                    string url = $"{sizeUrl}/{id}";

                    HttpResponseMessage response = await httpClient.PutAsJsonAsync(url, sizeDetail);
                    ChangeSkateholdersWhenChangeSize(id, "Deactive");

                    Response.Redirect("Dashboard?Mode=Size");
                }
                else if(Mode == "Order")
                {
                    string urlGetAllOrder;
                    urlGetAllOrder = $"{orderUrl}/getAllForAdmin?pageSize={pageSize}&pageNumber={pageNumber}";
                    HttpResponseMessage responseOrder = await httpClient.GetAsync(urlGetAllOrder);
                    string strDataOrder = await responseOrder.Content.ReadAsStringAsync();
                    var optionsOrder = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };

                    List<OrderDTO> orders = System.Text.Json.JsonSerializer.Deserialize<List<OrderDTO>>(strDataOrder, optionsOrder);

                    //Lay totalPage
                    string urlGetTotalOrder = $"{orderUrl}/getAllForAdmin?pageSize=0&pageNumber=1";
                    HttpResponseMessage responseTotalOrder = await httpClient.GetAsync(urlGetTotalOrder);
                    string strTotalOrder = await responseTotalOrder.Content.ReadAsStringAsync();
                    var optionsTotalOrder = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };

                    List<OrderDTO> totalOrder = System.Text.Json.JsonSerializer.Deserialize<List<OrderDTO>>(strTotalOrder, optionsTotalOrder);

                    ViewBag.orders = orders;
                    ViewBag.pageNumber = pageNumber;
                    ViewBag.pageSize = pageSize;
                    if (totalOrder != null && totalOrder.Count > 0)
                    {
                        ViewBag.ToTalPage = (int)Math.Ceiling((double)totalOrder.Count / pageSize);
                    }
                    else
                    {
                        ViewBag.TotalPage = 1;
                    }
                    ViewBag.Mode = "Order";
                }
                else if(Mode == "CreateOrder")
                {
                    //Lay User
                    string urlGetUser = $"{userUrl}/getAllForAdmin";
                    HttpResponseMessage responseUser = await httpClient.GetAsync(urlGetUser);
                    string strUser = await responseUser.Content.ReadAsStringAsync();
                    var optionsUser = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };

                    List<UserDTO> users = System.Text.Json.JsonSerializer.Deserialize<List<UserDTO>>(strUser, optionsUser);
                    ViewBag.users = new SelectList(users, "UserId", "Email");
                    ViewBag.Mode = "CreateOrder";
                    if (id != 0)
                    {
                        OrderDTO orderDetail = await GetOrder(id);
                        ViewBag.orderDetail = orderDetail;
                    }
                }
                else if(Mode == "DeleteOrder")
                {
                    OrderDTO orderDetail = await GetOrder(id);

                    orderDetail.Status = "Deactive";
                    orderDetail.UpdatedTime = DateTime.Now;
                    string url = $"{orderUrl}/{id}";

                    HttpResponseMessage response = await httpClient.PutAsJsonAsync(url, orderDetail);
                    ChangeSkateholdersWhenChangeOrder(id, "Deactive");

                    Response.Redirect("Dashboard?Mode=Order");
                }

                ViewBag.user = user;
                ViewBag.Mess = Mess;
                ViewBag.ColorMess = ColorMess;
                return View();

            }
            else
            {
                return View("unAuthen");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateAndUpdate(UserDTO? userDTO, CategoryDTO? categoryDTO, ProductDTO? productDTO, BlogDTO? blogDTO,
            ColorDTO? colorDTO, SizeDTO? sizeDTO, OrderDTO? orderDTO)
        {
            if (userDTO.Email != null)
            {
                //Lay User
                string urlGetUser = $"{userUrl}/getAll";
                HttpResponseMessage responseUser = await httpClient.GetAsync(urlGetUser);
                string strUser = await responseUser.Content.ReadAsStringAsync();
                var optionsUser = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                List<UserDTO> users = System.Text.Json.JsonSerializer.Deserialize<List<UserDTO>>(strUser, optionsUser);

                if (userDTO.UserId != 0)
                {
                    bool isEmailDuplicate = users.Any(user => user.UserId != userDTO.UserId && user.Email == userDTO.Email);
                    if (isEmailDuplicate)
                    {
                        return RedirectToAction("Index", new { Mode = "CreateUser", id = userDTO.UserId, Mess = "Email is exist", ColorMess = "Red" });
                    }

                    bool isPhoneDuplicate = users.Any(user => user.UserId != userDTO.UserId && user.Phone == userDTO.Phone);
                    if (isPhoneDuplicate)
                    {
                        return RedirectToAction("Index", new { Mode = "CreateUser", id = userDTO.UserId, Mess = "Phone is exist", ColorMess = "Red" });
                    }

                    UserDTO userResult = await GetUser(userDTO.UserId);
                    if (userDTO.Password != userResult.Password)
                    {
                        userDTO.Password = BCrypt.Net.BCrypt.HashPassword(userDTO.Password);
                    }
                    if (userDTO.Status == "Deactive")
                    {
                        Console.WriteLine("Deactive");
                        ChangeSkateholdersWhenChangeUser(userDTO.UserId, userDTO.Status);
                    }
                    else if (userDTO.Status == "Active")
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
                    return RedirectToAction("Index", new { Mode = "CreateUser", id = userDTO.UserId, Mess = "Update user successful", ColorMess = "Green" });

                }
                else
                {
                    bool isEmailDuplicate = users.Any(user => user.Email == userDTO.Email);
                    if (isEmailDuplicate)
                    {
                        return RedirectToAction("Index", new { Mode = "CreateUser", Mess = "Email is exist", ColorMess = "Red" });
                    }
                    bool isPhoneDuplicate = users.Any(user => user.Phone == userDTO.Phone);
                    if (isPhoneDuplicate)
                    {
                        return RedirectToAction("Index", new { Mode = "CreateUser", Mess = "Phone is exist", ColorMess = "Red" });
                    }

                    userDTO.RoleId = 6;
                    userDTO.RoleName = "NORMAL";
                    userDTO.Password = BCrypt.Net.BCrypt.HashPassword(userDTO.Password);
                    userDTO.CreatedTime = DateTime.Now;
                    string url = $"{userUrl}";

                    //Lay token tu session
                    string token = HttpContext.Session.GetString("Token");
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                    HttpResponseMessage response = await httpClient.PostAsJsonAsync(url, userDTO);
                    return RedirectToAction("Index", new { Mode = "CreateUser", Mess = "Create user successful", ColorMess = "Green" });
                }
            }
            else if (categoryDTO.CategoryName != null)
            {
                // Lay Category
                string urlGetCategory = $"{categoryUrl}/getAllForAdmin";
                HttpResponseMessage responseCategory = await httpClient.GetAsync(urlGetCategory);
                string strCategory = await responseCategory.Content.ReadAsStringAsync();
                var optionsCategory = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                List<CategoryDTO> categories = System.Text.Json.JsonSerializer.Deserialize<List<CategoryDTO>>(strCategory, optionsCategory);
                if (categoryDTO.CategoryId != 0)
                {
                    bool isCategoryNameDuplicate = categories.Any(cate => cate.CategoryId != categoryDTO.CategoryId && cate.CategoryName == categoryDTO.CategoryName);
                    if (isCategoryNameDuplicate)
                    {
                        return RedirectToAction("Index", new { Mode = "CreateCategory", id = categoryDTO.CategoryId, Mess = "Category name is exist", ColorMess = "Red" });
                    }
                    CategoryDTO categoryResult = await GetCategory(categoryDTO.CategoryId);
                    ChangeSkateholdersWhenChangeCategory(categoryDTO.CategoryId, categoryDTO.Status);
                    categoryDTO.CreatedTime = categoryResult.CreatedTime;
                    categoryDTO.UpdatedTime = DateTime.Now;
                    string url = $"{categoryUrl}/{categoryDTO.CategoryId}";

                    HttpResponseMessage response = await httpClient.PutAsJsonAsync(url, categoryDTO);
                    return RedirectToAction("Index", new { Mode = "CreateCategory", id = categoryDTO.CategoryId, Mess = "Update category successful", ColorMess = "Green" });
                }
                else
                {
                    bool isCategoryNameDuplicate = categories.Any(cate => cate.CategoryName == categoryDTO.CategoryName);
                    if (isCategoryNameDuplicate)
                    {
                        return RedirectToAction("Index", new { Mode = "CreateCategory", Mess = "Category name is exist", ColorMess = "Red" });
                    }
                    categoryDTO.CreatedTime = DateTime.Now;
                    string url = $"{categoryUrl}";

                    HttpResponseMessage response = await httpClient.PostAsJsonAsync(url, categoryDTO);
                    return RedirectToAction("Index", new { Mode = "CreateCategory", Mess = "Create category successful", ColorMess = "Green" });
                }
            }
            else if (productDTO.ProductName != null)
            {
                // Lay Product
                string urlGetProduct = $"{productUrl}/getAllForAdmin";
                HttpResponseMessage responseProduct = await httpClient.GetAsync(urlGetProduct);
                string strProduct = await responseProduct.Content.ReadAsStringAsync();
                var optionsProduct = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                List<ProductDTO> products = System.Text.Json.JsonSerializer.Deserialize<List<ProductDTO>>(strProduct, optionsProduct);
                if (productDTO.ProductId != 0)
                {
                    bool isProductNameDuplicate = products.Any(pro => pro.ProductId != productDTO.ProductId && pro.ProductName == productDTO.ProductName);
                    if (isProductNameDuplicate)
                    {
                        return RedirectToAction("Index", new { Mode = "CreateProduct", id = productDTO.ProductId, Mess = "Product name is exist", ColorMess = "Red" });
                    }
                    ProductDTO productResult = await GetProduct(productDTO.ProductId);
                    ChangeSkateholdersWhenChangeProduct(productDTO.ProductId, productDTO.Status);
                    productDTO.CreatedTime = productResult.CreatedTime;
                    productDTO.UpdatedTime = DateTime.Now;
                    string url = $"{productUrl}/{productDTO.ProductId}";

                    HttpResponseMessage response = await httpClient.PutAsJsonAsync(url, productDTO);

                    //Xoa cac Color hien dang co
                    foreach (var item in productResult.ProductColorDTOs)
                    {
                        string urlDelete = $"{productColorUrl}?id={item.ProductColorId}";

                        await httpClient.DeleteAsync(urlDelete);
                    }

                    //Xoa cac Size hien dang co
                    foreach (var item in productResult.ProductSizeDTOs)
                    {
                        string urlDelete = $"{productSizeUrl}?id={item.ProductSizeId}";

                        await httpClient.DeleteAsync(urlDelete);
                    }

                    //Them moi cac color sau khi update
                    if (productDTO.ColorId.Count > 0)
                    {
                        foreach (var item in productDTO.ColorId)
                        {
                            var productColor = new ProductColorDTO();
                            productColor.ProductId = productDTO.ProductId;
                            productColor.ColorId = item;
                            productColor.Status = "Active";
                            productColor.CreatedTime = DateTime.Now;
                            string urlColor = $"{productColorUrl}";
                            await httpClient.PostAsJsonAsync(urlColor, productColor);
                        }

                    }
                    if (productDTO.SizeId.Count > 0)
                    {
                        foreach (var item in productDTO.SizeId)
                        {
                            var productSize = new ProductSizeDTO();
                            productSize.ProductId = productDTO.ProductId;
                            productSize.SizeId = item;
                            productSize.Status = "Active";
                            productSize.CreatedTime = DateTime.Now;
                            string urlSize = $"{productSizeUrl}";
                            await httpClient.PostAsJsonAsync(urlSize, productSize);
                        }

                    }
                    return RedirectToAction("Index", new { Mode = "CreateProduct", id = productDTO.ProductId, Mess = "Update product successful", ColorMess = "Green" });
                }
                else
                {
                    bool isProductNameDuplicate = products.Any(pro => pro.ProductName == productDTO.ProductName);
                    if (isProductNameDuplicate)
                    {
                        return RedirectToAction("Index", new { Mode = "CreateProduct",  Mess = "Product name is exist", ColorMess = "Red" });
                    }
                    productDTO.CreatedTime = DateTime.Now;
                    string url = $"{productUrl}";

                    HttpResponseMessage response = await httpClient.PostAsJsonAsync(url, productDTO);
                    //Doc lai Product 
                    var responseData = await response.Content.ReadFromJsonAsync<ProductDTO>();

                    if (productDTO.ColorId.Count > 0)
                    {
                        foreach (var item in productDTO.ColorId)
                        {
                            var productColor = new ProductColorDTO();
                            productColor.ProductId = responseData.ProductId;
                            productColor.ColorId = item;
                            productColor.Status = "Active";
                            productColor.CreatedTime = DateTime.Now;
                            string urlColor = $"{productColorUrl}";
                            await httpClient.PostAsJsonAsync(urlColor, productColor);
                        }

                    }
                    if (productDTO.SizeId.Count > 0)
                    {
                        foreach (var item in productDTO.SizeId)
                        {
                            var productSize = new ProductSizeDTO();
                            productSize.ProductId = responseData.ProductId;
                            productSize.SizeId = item;
                            productSize.Status = "Active";
                            productSize.CreatedTime = DateTime.Now;
                            string urlSize = $"{productSizeUrl}";
                            await httpClient.PostAsJsonAsync(urlSize, productSize);
                        }

                    }
                    return RedirectToAction("Index", new { Mode = "CreateProduct", Mess = "Create product successful", ColorMess = "Green" });
                }
            }
            else if (blogDTO.BlogName != null)
            {
                if (blogDTO.BlogId != 0)
                {
                    BlogDTO blogResult = await GetBlog(blogDTO.BlogId);
                    blogDTO.CreatedTime = blogResult.CreatedTime;
                    blogDTO.UpdatedTime = DateTime.Now;
                    string url = $"{blogUrl}/{blogDTO.BlogId}";

                    HttpResponseMessage response = await httpClient.PutAsJsonAsync(url, blogDTO);
                    return RedirectToAction("Index", new { Mode = "CreateBlog", id = blogDTO.BlogId });
                }
                else
                {
                    blogDTO.CreatedTime = DateTime.Now;
                    string url = $"{blogUrl}";
                    //Lay token tu session
                    string token = HttpContext.Session.GetString("Token");
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    HttpResponseMessage response = await httpClient.PostAsJsonAsync(url, blogDTO);
                    return RedirectToAction("Index", new { Mode = "CreateBlog" });
                }
            }
            else if(colorDTO.ColorName != null)
            {
                // Lay Color
                string urlGetColor = $"{colorUrl}/getAllForAdmin";
                HttpResponseMessage responseColor = await httpClient.GetAsync(urlGetColor);
                string strColor = await responseColor.Content.ReadAsStringAsync();
                var optionsColor = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                List<ColorDTO> colors = System.Text.Json.JsonSerializer.Deserialize<List<ColorDTO>>(strColor, optionsColor);
                if (colorDTO.ColorId != 0)
                {
                    bool isColorNameDuplicate = colors.Any(col => col.ColorId != colorDTO.ColorId && col.ColorName == colorDTO.ColorName);
                    if (isColorNameDuplicate)
                    {
                        return RedirectToAction("Index", new { Mode = "CreateColor", id = colorDTO.ColorId, Mess = "Color name is exist", ColorMess = "Red" });
                    }
                    ColorDTO colorResult = await GetColor(colorDTO.ColorId);
                    ChangeSkateholdersWhenChangeColor(colorDTO.ColorId, colorDTO.Status);
                    colorDTO.CreatedTime = colorResult.CreatedTime;
                    colorDTO.UpdatedTime = DateTime.Now;

                    string url = $"{colorUrl}/{colorDTO.ColorId}";

                    HttpResponseMessage response = await httpClient.PutAsJsonAsync(url, colorDTO);
                    return RedirectToAction("Index", new { Mode = "CreateColor", id = colorDTO.ColorId, Mess = "Update color successful", ColorMess = "Green" });
                }
                else
                {
                    bool isColorNameDuplicate = colors.Any(col => col.ColorName == colorDTO.ColorName);
                    if (isColorNameDuplicate)
                    {
                        return RedirectToAction("Index", new { Mode = "CreateColor", Mess = "Color name is exist", ColorMess = "Red" });
                    }
                    colorDTO.CreatedTime = DateTime.Now;
                    string url = $"{colorUrl}";
                    HttpResponseMessage response = await httpClient.PostAsJsonAsync(url, colorDTO);
                    return RedirectToAction("Index", new { Mode = "CreateColor", Mess = "Create color successful", ColorMess = "Green" });
                }
            }
            else if(sizeDTO.SizeName != null)
            {
                // Lay Size
                string urlGetSize = $"{sizeUrl}/getAllForAdmin";
                HttpResponseMessage responseSize = await httpClient.GetAsync(urlGetSize);
                string strSize = await responseSize.Content.ReadAsStringAsync();
                var optionsSize = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                List<SizeDTO> sizes = System.Text.Json.JsonSerializer.Deserialize<List<SizeDTO>>(strSize, optionsSize);
                if (sizeDTO.SizeId != 0)
                {
                    bool isSizeNameDuplicate = sizes.Any(siz => siz.SizeId != sizeDTO.SizeId && siz.SizeName == sizeDTO.SizeName);
                    if (isSizeNameDuplicate)
                    {
                        return RedirectToAction("Index", new { Mode = "CreateSize", id = sizeDTO.SizeId, Mess = "Size name is exist", ColorMess = "Red" });
                    }
                    SizeDTO sizeResult = await GetSize(sizeDTO.SizeId);
                    ChangeSkateholdersWhenChangeSize(sizeDTO.SizeId, sizeDTO.Status);
                    sizeDTO.CreatedTime = sizeResult.CreatedTime;
                    sizeDTO.UpdatedTime = DateTime.Now;

                    string url = $"{sizeUrl}/{sizeDTO.SizeId}";

                    HttpResponseMessage response = await httpClient.PutAsJsonAsync(url, sizeDTO);
                    return RedirectToAction("Index", new { Mode = "CreateSize", id = sizeDTO.SizeId, Mess = "Update size successful", ColorMess = "Green" });
                }
                else
                {
                    bool isSizeNameDuplicate = sizes.Any(siz => siz.SizeName == sizeDTO.SizeName);
                    if (isSizeNameDuplicate)
                    {
                        return RedirectToAction("Index", new { Mode = "CreateSize", Mess = "Size name is exist", ColorMess = "Red" });
                    }
                    sizeDTO.CreatedTime = DateTime.Now;
                    string url = $"{sizeUrl}";
                    HttpResponseMessage response = await httpClient.PostAsJsonAsync(url, sizeDTO);
                    return RedirectToAction("Index", new { Mode = "CreateSize", Mess = "Create size successful", ColorMess = "Green" });
                }
            }
            else if(orderDTO.OrderDate != null)
            {
                if (orderDTO.OrderId != 0)
                {
                    OrderDTO orderResult = await GetOrder(orderDTO.OrderId);
                    ChangeSkateholdersWhenChangeOrder(orderDTO.OrderId, orderDTO.Status);
                    orderDTO.CreatedTime = orderResult.CreatedTime;
                    orderDTO.UpdatedTime = DateTime.Now;

                    string url = $"{orderUrl}/{orderDTO.OrderId}";

                    HttpResponseMessage response = await httpClient.PutAsJsonAsync(url, orderDTO);
                    return RedirectToAction("Index", new { Mode = "CreateOrder", id = orderDTO.OrderId });
                }
                else
                {
                    orderDTO.CreatedTime = DateTime.Now;
                    string url = $"{orderUrl}";
                    //Lay token tu session
                    string token = HttpContext.Session.GetString("Token");
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    HttpResponseMessage response = await httpClient.PostAsJsonAsync(url, orderDTO);
                    return RedirectToAction("Index", new { Mode = "CreateOrder" });
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

        private async Task<CategoryDTO> GetCategory(int categoryId)
        {
            string urlGetCategory;
            urlGetCategory = $"{categoryUrl}/{categoryId}";

            HttpResponseMessage responseCategory = await httpClient.GetAsync(urlGetCategory);
            string strDataCategory = await responseCategory.Content.ReadAsStringAsync();
            var optionsCategory = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            CategoryDTO category = System.Text.Json.JsonSerializer.Deserialize<CategoryDTO>(strDataCategory, optionsCategory);
            return category;
        }

        private async Task<ProductDTO> GetProduct(int productId)
        {
            string urlGetProduct;
            urlGetProduct = $"{productUrl}/{productId}";

            HttpResponseMessage responseProduct = await httpClient.GetAsync(urlGetProduct);
            string strDataProduct = await responseProduct.Content.ReadAsStringAsync();
            var optionsProduct = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            ProductDTO product = System.Text.Json.JsonSerializer.Deserialize<ProductDTO>(strDataProduct, optionsProduct);
            return product;
        }

        private async Task<BlogDTO> GetBlog(int blogId)
        {
            string urlGetBlog;
            urlGetBlog = $"{blogUrl}/{blogId}";

            HttpResponseMessage responseBlog = await httpClient.GetAsync(urlGetBlog);
            string strDataBlog = await responseBlog.Content.ReadAsStringAsync();
            var optionsBlog = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            BlogDTO blog = System.Text.Json.JsonSerializer.Deserialize<BlogDTO>(strDataBlog, optionsBlog);
            return blog;
        }
        private async Task<ColorDTO> GetColor(int colorId)
        {
            string urlGetColor;
            urlGetColor = $"{colorUrl}/{colorId}";

            HttpResponseMessage responseColor = await httpClient.GetAsync(urlGetColor);
            string strDataColor = await responseColor.Content.ReadAsStringAsync();
            var optionsColor = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            ColorDTO color = System.Text.Json.JsonSerializer.Deserialize<ColorDTO>(strDataColor, optionsColor);
            return color;
        }

        private async Task<SizeDTO> GetSize(int sizeId)
        {
            string urlGetSize;
            urlGetSize = $"{sizeUrl}/{sizeId}";

            HttpResponseMessage responseSize = await httpClient.GetAsync(urlGetSize);
            string strDataSize = await responseSize.Content.ReadAsStringAsync();
            var optionsSize = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            SizeDTO size = System.Text.Json.JsonSerializer.Deserialize<SizeDTO>(strDataSize, optionsSize);
            return size;
        }

        private async Task<OrderDTO> GetOrder(int orderId)
        {
            string urlGetOrder;
            urlGetOrder = $"{orderUrl}/{orderId}";

            HttpResponseMessage responseOrder = await httpClient.GetAsync(urlGetOrder);
            string strDataOrder = await responseOrder.Content.ReadAsStringAsync();
            var optionsOrder = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            OrderDTO order = System.Text.Json.JsonSerializer.Deserialize<OrderDTO>(strDataOrder, optionsOrder);
            return order;
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

        private async Task<IActionResult> ChangeSkateholdersWhenChangeCategory(int id, string status)
        {
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

            //GetAllRate
            string urlGetRate;
            urlGetRate = $"{rateUrl}/getAll";

            HttpResponseMessage responseRate = await httpClient.GetAsync(urlGetRate);
            string strDataRate = await responseRate.Content.ReadAsStringAsync();
            var optionsRate = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var rates = System.Text.Json.JsonSerializer.Deserialize<List<RateDTO>>(strDataRate, optionsRate);

            //Deactive Product
            string urlGetProduct;
            urlGetProduct = $"{productUrl}/getAllForAdmin";

            HttpResponseMessage responseProduct = await httpClient.GetAsync(urlGetProduct);
            string strDataProduct = await responseProduct.Content.ReadAsStringAsync();
            var optionsProduct = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var products = System.Text.Json.JsonSerializer.Deserialize<List<ProductDTO>>(strDataProduct, optionsProduct);
            foreach (var item in products)
            {
                if (item.CategoryId == id)
                {
                    item.Status = status;
                    item.UpdatedTime = DateTime.Now;
                    string urlProduct = $"{productUrl}/{item.ProductId}";
                    await httpClient.PutAsJsonAsync(urlProduct, item);

                    //Deactive OrderDetail
                    foreach(var items in orderDetails)
                    {
                        if (items.ProductId == item.ProductId)
                        {
                            items.Status = status;
                            items.UpdatedTime = DateTime.Now;
                            string urlOrderDetail = $"{orderDetailUrl}/{items.OrderDetailId}";
                            await httpClient.PutAsJsonAsync(urlOrderDetail, items);
                        }
                    }

                    //Deactive Rate
                    foreach (var items2 in rates)
                    {
                        if (items2.ProductId == item.ProductId)
                        {
                            items2.Status = status;
                            items2.UpdatedTime = DateTime.Now;
                            string urlRate = $"{rateUrl}/{items2.RateId}";
                            await httpClient.PutAsJsonAsync(urlRate, items2);
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
                if (item.CategoryId == id)
                {
                    item.Status = status;
                    item.UpdatedTime = DateTime.Now;
                    string urlBlog = $"{blogUrl}/{item.BlogId}";
                    await httpClient.PutAsJsonAsync(urlBlog, item);
                }
            }

            return null;
        }
        private async Task<IActionResult> ChangeSkateholdersWhenChangeProduct(int id, string status)
        {
            //Deactive OrderDetail
            string urlGetOrderDetail;
            urlGetOrderDetail = $"{orderDetailUrl}/getAllForAdmin";
            HttpResponseMessage responseOrderDetail = await httpClient.GetAsync(urlGetOrderDetail);
            string strDataOrderDetail = await responseOrderDetail.Content.ReadAsStringAsync();
            var optionsOrderDetail = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var orderDetails = System.Text.Json.JsonSerializer.Deserialize<List<OrderDetailDTO>>(strDataOrderDetail, optionsOrderDetail);
            foreach (var item in orderDetails)
            {
                if (item.ProductId == id)
                {
                    item.Status = status;
                    item.UpdatedTime = DateTime.Now;
                    string urlOrderDetail = $"{orderDetailUrl}/{item.OrderDetailId}";
                    await httpClient.PutAsJsonAsync(urlOrderDetail, item);
                }
            }

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
                if (item.ProductId == id)
                {
                    item.Status = status;
                    item.UpdatedTime = DateTime.Now;
                    string urlRate = $"{rateUrl}/{item.RateId}";
                    await httpClient.PutAsJsonAsync(urlRate, item);
                }
            }
            return null;
        }
        private async Task<IActionResult> ChangeSkateholdersWhenChangeColor(int id, string status)
        {
            //Deactive ProductColor
            string urlGetProductColor;
            urlGetProductColor = $"{productColorUrl}/getAll";

            HttpResponseMessage responseProductColor = await httpClient.GetAsync(urlGetProductColor);
            string strDataProductColor = await responseProductColor.Content.ReadAsStringAsync();
            var optionsProductColor = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var productColors = System.Text.Json.JsonSerializer.Deserialize<List<ProductColorDTO>>(strDataProductColor, optionsProductColor);
            foreach (var item in productColors)
            {
                if (item.ColorId == id)
                {
                    item.Status = status;
                    item.UpdatedTime = DateTime.Now;
                    string urlProductColor = $"{productColorUrl}/{item.ProductColorId}";
                    await httpClient.PutAsJsonAsync(urlProductColor, item);
                }
            }
            return null;
        }
        private async Task<IActionResult> ChangeSkateholdersWhenChangeSize(int id, string status)
        {
            //Deactive ProductSize
            string urlGetProductSize;
            urlGetProductSize = $"{productSizeUrl}/getAll";

            HttpResponseMessage responseProductSize = await httpClient.GetAsync(urlGetProductSize);
            string strDataProductSize = await responseProductSize.Content.ReadAsStringAsync();
            var optionsProductSize = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var productSizes = System.Text.Json.JsonSerializer.Deserialize<List<ProductSizeDTO>>(strDataProductSize, optionsProductSize);
            foreach (var item in productSizes)
            {
                if (item.SizeId == id)
                {
                    item.Status = status;
                    item.UpdatedTime = DateTime.Now;
                    string urlProductSize = $"{productSizeUrl}/{item.ProductSizeId}";
                    await httpClient.PutAsJsonAsync(urlProductSize, item);
                }
            }
            return null;
        }
        private async Task<IActionResult> ChangeSkateholdersWhenChangeOrder(int id, string status)
        {
            //Deactive OrderDetail
            string urlGetOrderDetail;
            urlGetOrderDetail = $"{orderDetailUrl}/getAllForAdmin";
            HttpResponseMessage responseOrderDetail = await httpClient.GetAsync(urlGetOrderDetail);
            string strDataOrderDetail = await responseOrderDetail.Content.ReadAsStringAsync();
            var optionsOrderDetail = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var orderDetails = System.Text.Json.JsonSerializer.Deserialize<List<OrderDetailDTO>>(strDataOrderDetail, optionsOrderDetail);
            foreach (var item in orderDetails)
            {
                if (item.OrderId == id)
                {
                    item.Status = status;
                    item.UpdatedTime = DateTime.Now;
                    string urlOrderDetail = $"{orderDetailUrl}/{item.OrderDetailId}";
                    await httpClient.PutAsJsonAsync(urlOrderDetail, item);
                }
            }
            return null; 
        }

    }
}

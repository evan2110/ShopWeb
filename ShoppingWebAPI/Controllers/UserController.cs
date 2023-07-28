using AutoMapper;
using BusinessObject.Models;
using DataAccess.Repository;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using ShoppingWebAPI.Request;
using ShoppingWebAPI.Response;
using System.Net;

namespace ShoppingWebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController
{
    private UserRepository repository;
    private IConfiguration configuration;
    private IMapper _mapper;

    public UserController(UserRepository repository, IConfiguration configuration, IMapper mapper)
    {
        this.repository = repository;
        this.configuration = configuration;
        this._mapper = mapper;
    }
    
    [HttpPost("register")]
    public async Task<ActionResult<CommonResponse>> Register([FromBody] UserRegisterRequest request)
    {
        try
        {
            CommonResponse response = new CommonResponse();
            Console.WriteLine(request.Email);
            Console.WriteLine(request.Password);

            User p = await repository.GetOneAsync(p => p.Email.Equals(request.Email));
            if (p != null)
            {
                response.IsSuccess = false;
                response.ErrorMessages = "Tài khoản đã có người đăng kí";
                return response;
            }
            User user = new User();
            user.Email = request.Email;
            user.Status = "Active";
            user.RoleId = 6;
            user.CreatedTime = DateTime.Now;
            user.Password = BCrypt.Net.BCrypt.HashPassword(request.Password);
            await repository.CreateAsync(user);
            response.IsSuccess = true;
            response.StatusCode = HttpStatusCode.OK;
            return response;
        }
        catch (Exception e)
        {
            return null;
        }
    }
    
    /*private string GenerateToken(UserDTO user)
    {
        var jwtTokenHandler = new JwtSecurityTokenHandler();  
        var secretKeyBytes = Encoding.UTF8.GetBytes(configuration["AppSettings:SecretKey"]);

        var tokenDescription = new SecurityTokenDescriptor 
        {
            Subject = new ClaimsIdentity(new[] //Ở đây, một đối tượng ClaimsIdentity mới được tạo ra, đại diện cho các thông tin trong JWT. Các thông tin này được biểu thị dưới dạng cặp khóa-giá trị. Trong trường hợp này, các thông tin bao gồm ClaimTypes.Name (biểu thị tên người dùng), ClaimTypes.Email (biểu thị email của người dùng), UserName (biểu thị tên đăng nhập), và Id (biểu thị ID của người dùng).
            {
                new Claim(ClaimTypes.Name, nguoiDung.HoTen),
                new Claim(ClaimTypes.Email, nguoiDung.Email),
                new Claim("UserName", nguoiDung.UserName),
                new Claim("Id", nguoiDung.Id.ToString()),

                //hoac co the bo them cac roles
                new Claim("TokenId", Guid.NewGuid().ToString())
            }),
            Expires = DateTime.UtcNow.AddMinutes(1), //Dòng này thiết lập thời gian hết hạn cho token. Trong trường hợp này, token sẽ hết hạn sau 1 phút tính từ thời gian UTC hiện tại.
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKeyBytes), SecurityAlgorithms.HmacSha512Signature) //Ky
        };
        var token = jwtTokenHandler.CreateToken(tokenDescription); //Tao token de tra ve
        return jwtTokenHandler.WriteToken(token);
    }*/
}
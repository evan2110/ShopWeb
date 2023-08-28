using System.IdentityModel.Tokens.Jwt;
using BusinessObject.Models;
using DataAccess.Repository;
using Microsoft.AspNetCore.Mvc;
using ShoppingWebAPI.Request;
using ShoppingWebAPI.Response;
using System.Net;
using System.Security.Claims;
using System.Text;
using DataAccess.DTO;
using Microsoft.IdentityModel.Tokens;
using ShoppingWebAPI.Config;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;

namespace ShoppingWebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private UserRepository repository;
    private IConfiguration configuration;
    private readonly IMapper _mapper;
    public UserController(UserRepository repository, IConfiguration configuration, IMapper mapper)
    {
        this.repository = repository;
        this.configuration = configuration;
        _mapper = mapper;
    }
    
    [HttpPost("login")]
    public async Task<ActionResult<AuthResponse>> Login([FromBody] UserRegisterRequest request)
    {
        try
        {
            AuthResponse response = new AuthResponse();
            User user = await repository.GetOneAsync(p => p.Email.Equals(request.Email), includeProperties:"Role");
            if (user == null)
            {
                response.IsSuccess = false;
                response.ErrorMessages = "User đăng nhập không tồn tại";
                return NotFound(response);
            }
            if (user.Status == "Deactive")
            {
                response.IsSuccess = false;
                response.ErrorMessages = "User bị khoá";
                return Unauthorized(response);
            }
            bool IsPasswordMatch = BCrypt.Net.BCrypt.Verify(request.Password, user.Password);
            if (IsPasswordMatch)
            {
                UserDTO userDTO = _mapper.Map<UserDTO>(user);
                string token = GenerateToken(userDTO);
                if (string.IsNullOrEmpty(token))
                {
                    response.IsSuccess = false;
                    response.ErrorMessages = "Cannot get token because null!";
                    return Unauthorized(response);
                }
                response.IsSuccess = true;
                response.Token = token;
                response.Result = userDTO;
                return Ok(response);
            }
            else
            {
                response.IsSuccess = false;
                response.ErrorMessages = "Sai mật khẩu";
                return Unauthorized(response);
            }
        }
        catch (Exception e)
        {
            return Problem(e.Message);
        }
    }
    
    [HttpPost("register")]
    public async Task<ActionResult<CommonResponse>> Register([FromBody] UserRegisterRequest request)
    {
        try
        {
            CommonResponse response = new CommonResponse();
            if (!ModelState.IsValid)
            {
                response.IsSuccess = false;
                response.ErrorMessages = "Input không hợp lệ";
                return BadRequest(response);
            }
            User p = await repository.GetOneAsync(p => p.Email.Equals(request.Email));
            if (p != null)
            {
                response.IsSuccess = false;
                response.ErrorMessages = "Tài khoản đã có người đăng kí";
                return BadRequest(Response);
            }
            var mapper = AutoMapperConfig.InitializeAutomapper<UserRegisterRequest, User>();
            User user = mapper.Map<User>(request);
            user.Status = "Active";
            user.RoleId = 6;
            user.CreatedTime = DateTime.Now;
            user.Password = BCrypt.Net.BCrypt.HashPassword(request.Password);
            await repository.CreateAsync(user);
            response.IsSuccess = true;
            response.StatusCode = HttpStatusCode.OK;
            return Ok(response);
        }
        catch (Exception e)
        {
            return Problem(e.Message);
        }
    }
    
    private string GenerateToken(UserDTO user)
    {
        var jwtTokenHandler = new JwtSecurityTokenHandler();  
        var secretKeyBytes = Encoding.UTF8.GetBytes(configuration["AppSettings:SecretKey"]);

        var tokenDescription = new SecurityTokenDescriptor 
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, user.FirstName + user.LastName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, GetRoleString(user.RoleId)),
                //hoac co the bo them cac roles
                new Claim("TokenId", Guid.NewGuid().ToString())
            }),
            Expires = DateTime.UtcNow.AddMinutes(20), 
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKeyBytes), SecurityAlgorithms.HmacSha512Signature) //Ky
        };
        var token = jwtTokenHandler.CreateToken(tokenDescription); //Tao token de tra ve
        return jwtTokenHandler.WriteToken(token);
    }
    
    private string GetRoleString(int RoleId)
    {
        if (RoleId == 3)
        {
            return "ADMIN";
        }
        if (RoleId == 6)
        {
            return "NORMAL";
        }
        return "";
    }

	[Authorize]
	[HttpGet("{user_id:int}", Name = "getUser")]
    public async Task<ActionResult<UserDTO>> GetOneUser(int user_id)
    {

        if (user_id == 0)
        {

            return BadRequest();
        }
        var User = await repository.GetOneAsync(x => x.UserId == user_id, includeProperties: "Role");

        if (User == null)
        {

            return NotFound();
        }

        UserDTO userDTO = _mapper.Map<UserDTO>(User);

        return Ok(userDTO);
    }

    [HttpPut("{User_id:int}", Name = "UpdateUser")]
    public async Task<ActionResult<UserDTO>> UpdateUser(int User_id, [FromBody] UserDTO userDTO)
    {

        if (userDTO == null || User_id != userDTO.UserId)
        {
            return BadRequest();
        }

        // Map nguoc tu DTO -> Entity thi dung AutoMapperConfig
        var mapper = AutoMapperConfig.InitializeAutomapper<UserDTO, User>();
        var userCreate = mapper.Map<User>(userDTO);

        // Thực hiện tạo mới Movie
        await repository.UpdateAsync(userCreate);

        return Ok();
    }

    [HttpPost]
    [Authorize]
    public async Task<ActionResult<UserDTO>> CreateUser([FromBody] UserDTO userDTO)
    {

        if (userDTO == null)
        {
            return BadRequest();
        }

        // Map nguoc tu DTO -> Entity thi dung AutoMapperConfig
        var mapper = AutoMapperConfig.InitializeAutomapper<UserDTO, User>();
        var userCreate = mapper.Map<User>(userDTO);

        // Thực hiện tạo mới Movie
        await repository.CreateAsync(userCreate);

        return Ok();
    }


    [HttpGet("getAll")]
    public async Task<ActionResult<UserDTO>> GetAllUsers([FromQuery(Name = "search")] string? search, int pageSize = 0, int pageNumber = 1)
    {
        IEnumerable<User> UserList;
        UserList = await repository.GetAllAsync(e => e.Status == "Active", includeProperties: "Role", pageSize: pageSize, pageNumber: pageNumber);
        if (search != null)
        {
            UserList = await repository.GetAllAsync(e => e.Status == "Active" && e.Email.Contains(search), includeProperties: "Role", pageSize: pageSize, pageNumber: pageNumber);
        }
        List<UserDTO> listDTO = _mapper.Map<List<UserDTO>>(UserList);

        return Ok(listDTO);
    }

    [HttpGet("getAllForAdmin")]
    public async Task<ActionResult<UserDTO>> getAllForAdmin(int pageSize = 0, int pageNumber = 1)
    {
        IEnumerable<User> UserList;
        UserList = await repository.GetAllAsync(e => e.RoleId != 3, includeProperties: "Role", pageSize: pageSize, pageNumber: pageNumber);
        List<UserDTO> listDTO = _mapper.Map<List<UserDTO>>(UserList);

        return Ok(listDTO);
    }
}
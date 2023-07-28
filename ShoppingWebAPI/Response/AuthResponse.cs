namespace ShoppingWebAPI.Response;

public class AuthResponse : BaseResponse<object>
{
    public string? Token { get; set; }
}
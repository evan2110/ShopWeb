using System.Net;

namespace ShoppingWebAPI.Response;

public class BaseResponse<T>
{
    public HttpStatusCode? StatusCode { get; set; }
    public bool? IsSuccess { get; set; } = true;
    public string? ErrorMessages { get; set; }
    public T? Result { get; set; }
}
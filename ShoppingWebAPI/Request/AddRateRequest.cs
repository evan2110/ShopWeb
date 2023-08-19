namespace ShoppingWebAPI.Request
{
    public class AddRateRequest
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public int ProductId { get; set; }
        public string Content { get; set; }
        public string? Image { get; set; }
    }
}

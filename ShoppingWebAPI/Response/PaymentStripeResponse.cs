namespace ShoppingWebAPI.Response
{
    public class PaymentStripeResponse
    {
        public string SessionId { get; set; }
        public string Url { get; set; }
        public string Status { get; set; }
    }
}

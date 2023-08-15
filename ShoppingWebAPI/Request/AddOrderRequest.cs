namespace ShoppingWebAPI.Request
{
    public class AddOrderRequest
    {
        public int UserId { get; set; }
        public string ShipAddress { get; set; }
        public string ShipPhone { get; set; }
        public decimal ToTalPrice { get;set; }

    }
}

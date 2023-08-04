namespace ShoppingWebAPI.Request
{
    public class AddToCartRequest
    {
        public int ProductId { get; set; }
        public int ColorId { get; set; }
        public int SizeId { get; set; }
        public int Quantity { get; set; }

    }
}

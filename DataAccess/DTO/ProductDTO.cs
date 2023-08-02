using BusinessObject.Models;

namespace DataAccess.DTO;

public class ProductDTO
{
    public int ProductId { get; set; }
    public string ProductName { get; set; }
    public string Description { get; set; }
    public string ImageFront { get; set; }
    public string ImageBehind { get; set; }
    public string ImageLeft { get; set; }
    public string ImageRight { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public int CategoryId { get; set; }
    public string CategoryName { get; set; }
    public int Discount { get; set; }
    public List<ProductColorDTO> ProductColorDTOs { get; set; }
    public List<ProductSizeDTO> ProductSizeDTOs { get; set; }
    public string Status { get; set; }
    public DateTime CreatedTime { get; set; }
    public DateTime? UpdatedTime { get; set; }

}
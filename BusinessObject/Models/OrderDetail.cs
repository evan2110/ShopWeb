using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessObject.Models;

[Table("OrderDetail")]
public class OrderDetail: BaseModel
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("order_detail_id")]
    public int OrderDetailId { get; set; }
    
    [Column("product_id")]
    public int ProductId { get; set; }
    
    [ForeignKey("ProductId")]
    public Product Product { get; set; }
    
    [Column("order_id")]
    public int OrderId { get; set; }
    
    [ForeignKey("OrderId")]
    public Order Order { get; set; }
    
    [Required]
    [Column("price", TypeName = "money")]
    public decimal Price { get; set; }
    
    [Required]
    [Column("quantity")]
    public int Quantity { get; set; }
    
    [Required]
    [Column("discount")]
    public int Discount { get; set; }
}
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessObject.Models;

[Table("ProductSize")]
public class ProductSize: BaseModel
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("product_size_id")]
    public int ProductSizeId { get; set; }
    
    [Column("product_id")]
    public int ProductId { get; set; }
    
    [ForeignKey("ProductId")]
    public Product Product { get; set; }
    
    [Column("size_id")]
    public int SizeId { get; set; }
    
    [ForeignKey("SizeId")]
    public Size Size { get; set; }
}
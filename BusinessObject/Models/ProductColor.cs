using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessObject.Models;

[Table("ProductColor")]
public class ProductColor: BaseModel
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("product_color_id")]
    public int ProductColorId { get; set; }
    
    [Column("product_id")]
    public int ProductId { get; set; }
    
    [ForeignKey("ProductId")]
    public Product Product { get; set; }
    
    [Column("color_id")]
    public int ColorId { get; set; }
    
    [ForeignKey("ColorId")]
    public Color Color { get; set; }
}
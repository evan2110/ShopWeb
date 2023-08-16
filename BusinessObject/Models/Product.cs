using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessObject.Models;

[Table("Product")]
public class Product: BaseModel
{
    public Product()
    {
        ProductSizes = new HashSet<ProductSize>();
        ProductColors = new HashSet<ProductColor>();
        OrderDetails = new HashSet<OrderDetail>();
        CartItems = new HashSet<CartItem>();
        Rates = new HashSet<Rate>();
    }
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("product_id")]
    public int ProductId { get; set; }

    [Required]
    [StringLength(50)]
    [Column("product_name", TypeName = "nvarchar(50)")]
    public string ProductName { get; set; }
    
    [Column("description")]
    public string Description { get; set; }
    
    [Column("image_front")]
    public string ImageFront { get; set; }
    
    [Column("image_behind")]
    public string ImageBehind { get; set; }
    
    [Column("image_left")]
    public string ImageLeft { get; set; }
    
    [Column("image_right")]
    public string ImageRight { get; set; }
    
    [Required]
    [Column("price", TypeName = "decimal")]
    public decimal Price { get; set; }
    
    [Required]
    [Column("quantity")]
    public int Quantity { get; set; }
    
    [Column("category_id")]
    public int CategoryId { get; set; }
    
    [ForeignKey("CategoryId")]
    public Category Category { get; set; }
    
    [Required]
    [Column("discount")]
    public int Discount { get; set; }
    
    public virtual ICollection<ProductSize> ProductSizes { get; set; }
    public virtual ICollection<ProductColor> ProductColors { get; set; }
    public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    public virtual ICollection<CartItem> CartItems { get; set; }
    public virtual ICollection<Rate> Rates { get; set; }

}
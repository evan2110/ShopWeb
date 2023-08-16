using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessObject.Models;

[Table("Order")]
public class Order: BaseModel
{
    public Order()
    {
        OrderDetails = new HashSet<OrderDetail>();
    }
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("order_id")]
    public int OrderId { get; set; }
    
    [Column("user_id")]
    public int UserId { get; set; }
    
    [ForeignKey("UserId")]
    public User User { get; set; }
    
    [Required]
    [Column("order_date", TypeName = "datetime")]
    public DateTime OrderDate { get; set; }
    
    [Required]
    [Column("required_date", TypeName = "datetime")]
    public DateTime RequiredDate { get; set; }
    
    [Column("shipped_date", TypeName = "datetime")]
    public DateTime ShippedDate { get; set; }

    [Required]
    [StringLength(100)]
    [Column("ship_phone", TypeName = "nvarchar(100)")]
    public string ShipPhone { get; set; }

    [Required]
    [StringLength(100)]
    [Column("ship_address", TypeName = "nvarchar(100)")]
    public string ShipAddress { get; set; }

    [Required]
    [Column("total_price", TypeName = "decimal")]
    public decimal TotalPrice { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; set; }
}
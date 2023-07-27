using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessObject.Models;

[Table("Order")]
public class Order: BaseModel
{
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
    [Column("freight", TypeName = "money")]
    public decimal Freight { get; set; }
    
    [Required]
    [StringLength(100)]
    [Column("ship_name", TypeName = "nvarchar(100)")]
    public string ShipName { get; set; }
    
    [Required]
    [StringLength(100)]
    [Column("ship_address", TypeName = "nvarchar(100)")]
    public string ShipAddress { get; set; }
    
    [Required]
    [StringLength(100)]
    [Column("ship_city", TypeName = "nvarchar(100)")]
    public string ShipCity { get; set; }
    
    [Required]
    [StringLength(100)]
    [Column("ship_region", TypeName = "nvarchar(100)")]
    public string ShipRegion { get; set; }
    
    [Required]
    [StringLength(100)]
    [Column("ship_postal_code", TypeName = "nvarchar(100)")]
    public string ShipPostalCode { get; set; }
    
    [Required]
    [StringLength(100)]
    [Column("ship_country", TypeName = "nvarchar(100)")]
    public string ShipCountry { get; set; }
    
    [Column("ship_via_id")]
    public int ShipViaId { get; set; }
    
    [ForeignKey("ShipViaId")]
    public ShipVia ShipVia { get; set; }
    
    public virtual List<OrderDetail> OrderDetails { get; set; }
}
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessObject.Models;

[Table("Cart")]
public class Cart: BaseModel
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("cart_id")]
    public int CartId { get; set; }
    
    [Column("user_id")]
    public int UserId { get; set; }
    
    [ForeignKey("UserId")]
    public User User { get; set; }
    
    public virtual List<CartItem> CartItems { get; set; }
}
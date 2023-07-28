using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessObject.Models;

[Table("Cart")]
public class Cart: BaseModel
{
    public Cart()
    {
        CartItems = new HashSet<CartItem>();
    }
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("cart_id")]
    public int CartId { get; set; }
    
    [Column("user_id")]
    public int UserId { get; set; }
    
    [ForeignKey("UserId")]
    public User User { get; set; }
    
    public virtual ICollection<CartItem> CartItems { get; set; }
}
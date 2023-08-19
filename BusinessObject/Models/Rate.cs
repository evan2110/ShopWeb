using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessObject.Models;

[Table("Rate")]
public class Rate: BaseModel
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("rate_id")]
    public int RateId { get; set; }
    
    [Column("user_id")]
    public int UserId { get; set; }
    
    [ForeignKey("UserId")]
    public User User { get; set; }
    
    [Column("product_id")]
    public int ProductId { get; set; }
    
    [ForeignKey("ProductId")]
    public Product Product { get; set; }

    [Column("content")]
    public string Content { get; set; }
    
    [Column("image")]
    public string Image { get; set; }
}
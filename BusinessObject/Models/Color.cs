using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessObject.Models;

[Table("Color")]
public class Color: BaseModel
{
    public Color()
    {
        ProductColors = new HashSet<ProductColor>();
    }
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("color_id")]
    public int ColorId { get; set; }
    
    [Required]
    [StringLength(50)]
    [Column("color_name", TypeName = "nvarchar(50)")]
    public string ColorName { get; set; }
    
    [Column("image")]
    public string Image { get; set; }
    
    public virtual ICollection<ProductColor> ProductColors { get; set; }
}
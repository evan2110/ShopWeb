using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessObject.Models;

[Table("Size")]
public class Size: BaseModel
{
    public Size()
    {
        ProductSizes = new HashSet<ProductSize>();
    }
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("size_id")]
    public int SizeId { get; set; }
    
    [Required]
    [StringLength(50)]
    [Column("size_name", TypeName = "nvarchar(50)")]
    public string SizeName { get; set; }
    
    public virtual ICollection<ProductSize> ProductSizes { get; set; }
}
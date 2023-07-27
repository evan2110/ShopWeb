using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessObject.Models;

[Table("Category")]
public class Category: BaseModel
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("category_id")]
    public int CategoryId { get; set; }

    [Required]
    [StringLength(50)]
    [Column("category_name", TypeName = "nvarchar(50)")]
    public string CategoryName { get; set; }
    
    [Column("description")]
    public string Description { get; set; }
    
    [Column("image")]
    public string Image { get; set; }
    
    public virtual List<Blog> Blogs { get; set; }
    public virtual List<Product> Products { get; set; }
}
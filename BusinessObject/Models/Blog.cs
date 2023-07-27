using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessObject.Models;

[Table("Blog")]
public class Blog: BaseModel
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("blog_id")]
    public int BlogId { get; set; }

    [Required]
    [StringLength(50)]
    [Column("blog_name", TypeName = "nvarchar(50)")]
    public string BlogName { get; set; }
    
    [Column("content")]
    public string Content { get; set; }
    
    [Column("image")]
    public string Image { get; set; }
    
    [Column("user_id")]
    public int UserId { get; set; }
    
    [ForeignKey("UserId")]
    public User User { get; set; }
    
    [Column("category_id")]
    public int CategoryId { get; set; }
    
    [ForeignKey("CategoryId")]
    public Category Category { get; set; }
}
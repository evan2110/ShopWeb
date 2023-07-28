using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace BusinessObject.Models;

[Table("User")]
public class User: BaseModel
{
    public User()
    {
        Blogs = new HashSet<Blog>();
        Carts = new HashSet<Cart>();
        Rates = new HashSet<Rate>();
    }
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("user_id")]
    public int UserId { get; set; }
    
    [StringLength(100)]
    [Column("first_name", TypeName = "nvarchar(100)")]
    public string? FirstName { get; set; }
    
    [StringLength(100)]
    [Column("last_name", TypeName = "nvarchar(100)")]
    public string? LastName { get; set; }
    
    [Required]
    [Column("email", TypeName = "nvarchar(50)")]
    [StringLength(50)]
    [EmailAddress]
    public string Email { get; set; }
    
    [Column("image")]
    public string? Image { get; set; }
    
    [Phone]
    [Column("phone", TypeName = "nvarchar(50)")]
    [StringLength(50)]
    public string? Phone { get; set; }
    
    [StringLength(50)]
    [Column("gender", TypeName = "nvarchar(50)")]
    public string? Gender { get; set; }
    
    [Required]
    [PasswordPropertyText]
    [Column("password")]
    public string Password { get; set; }
    
    [Column("role_id")]
    public int RoleId { get; set; }
    
    [ForeignKey("RoleId")]
    public Role Role { get; set; }
    
    public virtual ICollection<Blog> Blogs { get; set; }
    public virtual ICollection<Cart> Carts { get; set; }
    public virtual ICollection<Rate> Rates { get; set; }

}
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessObject.Models;

[Table(("Role"))]
public class Role: BaseModel
{
    public Role()
    {
        Users = new HashSet<User>();
    }
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("role_id")]
    public int RoleId { get; set; }
    
    [Required]
    [StringLength(50)]
    [Column("role_name", TypeName = "nvarchar(50)")]
    public string RoleName { get; set; }
    
    public virtual ICollection<User> Users { get; set; }
}
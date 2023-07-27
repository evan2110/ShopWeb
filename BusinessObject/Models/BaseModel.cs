using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessObject.Models;

public class BaseModel
{
    [Required]
    [StringLength(50)]
    [Column("status", TypeName = "nvarchar(50)")]
    public string Status { get; set; }
    
    [Required]
    [Column("created_time", TypeName = "datetime")]
    public DateTime CreatedTime { get; set; }
    
    [Column("updated_time", TypeName = "datetime")]
    public DateTime UpdatedTime { get; set; }
}
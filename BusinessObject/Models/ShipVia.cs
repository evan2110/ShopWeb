using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessObject.Models;

[Table("ShipVia")]
public class ShipVia: BaseModel
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("ship_via_id")]
    public int ShipViaId { get; set; }
    
    [Required]
    [StringLength(50)]
    [Column("ship_via_name", TypeName = "nvarchar(50)")]
    public string ShipViaName { get; set; }
    
    public virtual List<Order> Orders { get; set; }
}
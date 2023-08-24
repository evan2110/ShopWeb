using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Models
{
    [Table("Room")]
    public class Room: BaseModel
    {
        public Room()
        {
            Supports = new HashSet<Support>();
        }
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("room_id")]
        public int RoomId { get; set; }

        [Column("user_id")]
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }
        public virtual ICollection<Support> Supports { get; set; }

    }
}

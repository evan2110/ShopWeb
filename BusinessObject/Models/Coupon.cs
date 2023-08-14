using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Models
{
    [Table("Coupon")]
    public class Coupon : BaseModel
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("coupon_id")]
        public int CouponId { get; set; }

        [Required]
        [Column("coupon_serie")]
        public string CouponSerie { get; set; }

        [Required]
        [Column("coupon_discount")]
        public int CouponDiscount { get; set; }

        [Required]
        [Column("expirate_date", TypeName = "datetime")]
        public DateTime ExpirateDate { get; set; }
    }
}

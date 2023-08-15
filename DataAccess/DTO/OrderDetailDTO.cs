using BusinessObject.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTO
{
    public class OrderDetailDTO
    {
        public int OrderDetailId { get; set; }
        public int ProductId { get; set; }
        public int OrderId { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public int Discount { get; set; }
        public string Status { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime? UpdatedTime { get; set; }
    }
}

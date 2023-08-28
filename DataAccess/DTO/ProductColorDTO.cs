using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTO
{
    public class ProductColorDTO
    {
        public int ProductColorId { get; set; }
        public int ProductId { get; set; }
        public string? ProductName { get; set; }
        public int ColorId { get; set; }
        public string? ColorName { get; set; }
        public string? Status { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? UpdatedTime { get; set; }
    }
}

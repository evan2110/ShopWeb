using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTO
{
    public class TopBuyProductDTO
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public string ImageFront { get; set; }
        public string ImageBehind { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public int Discount { get; set; }
        public string Status { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime? UpdatedTime { get; set; }
    }
}

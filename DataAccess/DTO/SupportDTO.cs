using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTO
{
    public class SupportDTO
    {
        public int SupportId { get; set; }
        public int UserId { get; set; }
        public string? UserName { get; set; }
        public string Message { get; set; }
        public string Status { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime? UpdatedTime { get; set; }
    }
}

using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class OrderDetailRepository : Repository<OrderDetail>
    {
        private readonly MyDBContext _db;
        public OrderDetailRepository(MyDBContext db) : base(db)
        {
            _db = db;
        }
    }
}

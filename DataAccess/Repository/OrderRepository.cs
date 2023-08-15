using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class OrderRepository : Repository<Order>
    {
        private readonly MyDBContext _db;
        public OrderRepository(MyDBContext db) : base(db)
        {
            _db = db;
        }
    }
}

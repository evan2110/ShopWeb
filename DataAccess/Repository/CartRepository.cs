using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class CartRepository : Repository<Cart>
    {
        private readonly MyDBContext _db;
        public CartRepository(MyDBContext db) : base(db)
        {
            _db = db;
        }
    }
}

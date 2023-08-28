using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class SizeRepository : Repository<Size>
    {
        private readonly MyDBContext _db;
        public SizeRepository(MyDBContext db) : base(db)
        {
            _db = db;
        }
    }
}

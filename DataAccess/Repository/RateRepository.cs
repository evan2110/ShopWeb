using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class RateRepository : Repository<Rate>
    {
        private readonly MyDBContext _db;
        public RateRepository(MyDBContext db) : base(db)
        {
            _db = db;
        }
    }
}

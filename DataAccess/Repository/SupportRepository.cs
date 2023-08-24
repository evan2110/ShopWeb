using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class SupportRepository : Repository<Support>
    {
        private readonly MyDBContext _db;
        public SupportRepository(MyDBContext db) : base(db)
        {
            _db = db;
        }
    }
}

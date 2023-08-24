using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class RoomRepository : Repository<Room>
    {
        private readonly MyDBContext _db;
        public RoomRepository(MyDBContext db) : base(db)
        {
            _db = db;
        }
    }
}

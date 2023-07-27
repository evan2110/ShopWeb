using BusinessObject.Models;

namespace DataAccess.Repository;

public class UserRepository: Repository<User>
{
    private readonly MyDBContext _db;
    public UserRepository(MyDBContext db) : base(db)
    {
        _db = db;
    }
}
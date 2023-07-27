using BusinessObject.Models;

namespace DataAccess.Repository;

public class ProductRepository: Repository<Product>
{
    private readonly MyDBContext _db;
    public ProductRepository(MyDBContext db) : base(db)
    {
        _db = db;
    }
}
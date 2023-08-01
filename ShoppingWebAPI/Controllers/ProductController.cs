using BusinessObject.Models;
using DataAccess.DTO;
using DataAccess.Repository;
using Microsoft.AspNetCore.Mvc;
using ShoppingWebAPI.Config;

namespace ShoppingWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private ProductRepository repository;
        private IConfiguration configuration;
        public ProductController(ProductRepository repository, IConfiguration configuration)
        {
            this.repository = repository;
            this.configuration = configuration;
        }
        [HttpGet("list")]
        public async Task<ActionResult<ProductDTO>> GetTopProducts()
        {
            IEnumerable<Product> ProductList = await repository.GetAllAsync(includeProperties: "Category");
            var result = ProductList.OrderByDescending(p => p.CreatedTime).Take(10);
            var mapper = AutoMapperConfig.InitializeAutomapper<Product, ProductDTO>();
            List<ProductDTO> listDTO = mapper.Map<List<ProductDTO>>(result);

            return Ok(listDTO);
        }
    }
}

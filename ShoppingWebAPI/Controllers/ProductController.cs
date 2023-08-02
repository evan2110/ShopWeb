using AutoMapper;
using BusinessObject.Models;
using DataAccess.DTO;
using DataAccess.Repository;
using Microsoft.AspNetCore.Mvc;
using ShoppingWebAPI.Config;
using static System.Net.Mime.MediaTypeNames;

namespace ShoppingWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private ProductRepository repository;
        private ProductColorRepository colorRepository;
        private ProductSizeRepository sizeRepository;

        private IConfiguration configuration;
        private readonly IMapper _mapper;

        public ProductController(ProductRepository repository, IConfiguration configuration, IMapper mapper, ProductColorRepository colorRepository, ProductSizeRepository sizeRepository)
        {
            this.repository = repository;
            this.configuration = configuration;
            this._mapper = mapper;
            this.colorRepository = colorRepository;
            this.sizeRepository = sizeRepository;
        }
        [HttpGet("list")]
        public async Task<ActionResult<ProductDTO>> GetTopProducts()
        {
            IEnumerable<Product> ProductList = await repository.GetAllAsync(includeProperties: "Category");
            var result = ProductList.OrderByDescending(p => p.CreatedTime).Take(10);
            List<ProductDTO> listDTO = _mapper.Map<List<ProductDTO>>(result);

            return Ok(listDTO);
        }

        [HttpGet("{product_id:int}", Name = "getProduct")]
        public async Task<ActionResult<ProductDTO>> GetOneProduct(int product_id)
        {

            if (product_id == 0)
            {

                return BadRequest();
            }
            var Product = await repository.GetOneAsync(x => x.ProductId == product_id, includeProperties: "Category");
            IEnumerable<ProductColor> ProductColorlist = await colorRepository.GetAllAsync(e => e.ProductId == product_id, includeProperties: "Color");
            IEnumerable<ProductSize> ProductSizelist = await sizeRepository.GetAllAsync(e => e.ProductId == product_id, includeProperties: "Size");

            if (Product == null)
            {

                return NotFound();
            }

            ProductDTO productDTO = _mapper.Map<ProductDTO>(Product);
            if(ProductColorlist != null)
            {
                List<ProductColorDTO> productColorDTO = _mapper.Map<List<ProductColorDTO>>(ProductColorlist);

                productDTO.ProductColorDTOs = productColorDTO;
            }

            if(ProductSizelist != null)
            {
                List<ProductSizeDTO> productSizeDTO = _mapper.Map<List<ProductSizeDTO>>(ProductSizelist);

                productDTO.ProductSizeDTOs = productSizeDTO;

            }

            return Ok(productDTO);
        }

        
    }
}

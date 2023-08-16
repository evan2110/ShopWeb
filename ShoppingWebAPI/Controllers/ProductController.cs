using AutoMapper;
using BusinessObject.Models;
using DataAccess.DTO;
using DataAccess.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
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

        [HttpGet("getAll")]
        public async Task<ActionResult<ProductDTO>> GetAllProducts(int pageSize = 0, int pageNumber = 1)
        {
            IEnumerable<Product> ProductList;
            ProductList = await repository.GetAllAsync(e => e.Status == "Active", includeProperties: "Category", pageSize: pageSize, pageNumber: pageNumber);
            List<ProductDTO> listDTO = _mapper.Map<List<ProductDTO>>(ProductList);
            foreach (var item in listDTO)
            {
                IEnumerable<ProductColor> ProductColorlist = await colorRepository.GetAllAsync(e => e.ProductId == item.ProductId, includeProperties: "Color");
                IEnumerable<ProductSize> ProductSizelist = await sizeRepository.GetAllAsync(e => e.ProductId == item.ProductId, includeProperties: "Size");
                List<ProductColorDTO> productColorDTO = _mapper.Map<List<ProductColorDTO>>(ProductColorlist);
                List<ProductSizeDTO> productSizeDTO = _mapper.Map<List<ProductSizeDTO>>(ProductSizelist);
                item.ProductColorDTOs = productColorDTO;
                item.ProductSizeDTOs = productSizeDTO;
            }

            return Ok(listDTO);
        }


        [HttpGet("list")]
        public async Task<ActionResult<ProductDTO>> GetTopProducts([FromQuery] int categoryId)
        {
            IEnumerable<Product> ProductList;
            IEnumerable<Product> result;
            if(categoryId != 0)
            {
                ProductList = await repository.GetAllAsync(e => e.CategoryId == categoryId && e.Status == "Active", includeProperties: "Category");
                result = ProductList.OrderBy(p => p.CreatedTime).Take(10);
            }
            else
            {
                ProductList = await repository.GetAllAsync(e => e.Status == "Active" ,includeProperties: "Category");
                result = ProductList.OrderByDescending(p => p.CreatedTime).Take(10);

            }
            List<ProductDTO> listDTO = _mapper.Map<List<ProductDTO>>(result);
            foreach (var item in listDTO)
            {
                IEnumerable<ProductColor> ProductColorlist = await colorRepository.GetAllAsync(e => e.ProductId == item.ProductId, includeProperties: "Color");
                IEnumerable<ProductSize> ProductSizelist = await sizeRepository.GetAllAsync(e => e.ProductId == item.ProductId, includeProperties: "Size");
                List<ProductColorDTO> productColorDTO = _mapper.Map<List<ProductColorDTO>>(ProductColorlist);
                List<ProductSizeDTO> productSizeDTO = _mapper.Map<List<ProductSizeDTO>>(ProductSizelist);
                item.ProductColorDTOs = productColorDTO;
                item.ProductSizeDTOs = productSizeDTO;
            }

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

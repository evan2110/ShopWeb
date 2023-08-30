using AutoMapper;
using BusinessObject.Models;
using DataAccess.DTO;
using DataAccess.Repository;
using Microsoft.AspNetCore.Mvc;
using ShoppingWebAPI.Config;

namespace ShoppingWebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductSizeController : ControllerBase
    {
        private ProductSizeRepository repository;
        private IConfiguration configuration;
        private readonly IMapper _mapper;

        public ProductSizeController(ProductSizeRepository repository, IConfiguration configuration, IMapper mapper)
        {
            this.repository = repository;
            this.configuration = configuration;
            this._mapper = mapper;
        }

        [HttpPost]
        public async Task<ActionResult<ProductSizeDTO>> CreateProductSize([FromBody] ProductSizeDTO productSizeDTO)
        {

            if (await repository.GetOneAsync(x => x.ProductSizeId == productSizeDTO.ProductSizeId) != null)
            {
                return BadRequest();
            }

            // Tạo đối tượng Movie từ MovieCreate và gán Genre
            // Map nguoc tu DTO -> Entity thi dung AutoMapperConfig
            var mapper = AutoMapperConfig.InitializeAutomapper<ProductSizeDTO, ProductSize>();
            var productSizeCreate = mapper.Map<ProductSize>(productSizeDTO);

            // Thực hiện tạo mới Movie
            await repository.CreateAsync(productSizeCreate);

            // Ánh xạ lại đối tượng Movie sau khi tạo thành công
            var resultDTIO = _mapper.Map<ProductSizeDTO>(productSizeCreate);


            return Ok(resultDTIO);
        }

        [HttpGet("getAll")]
        public async Task<ActionResult<ProductSizeDTO>> GetAllProductSizes(int pageSize = 0, int pageNumber = 1)
        {
            var ProductSizeList = await repository.GetAllAsync(includeProperties: "Size,Product", pageSize: pageSize, pageNumber: pageNumber);
            List<ProductSizeDTO> listDTO = _mapper.Map<List<ProductSizeDTO>>(ProductSizeList);

            return Ok(listDTO);
        }

        [HttpPut("{ProductSize_id:int}", Name = "UpdateProductSize")]
        public async Task<ActionResult<ProductSizeDTO>> UpdateProductSize(int ProductSize_id, [FromBody] ProductSizeDTO productSizeDTO)
        {

            if (productSizeDTO == null || ProductSize_id != productSizeDTO.ProductSizeId)
            {
                return BadRequest();
            }

            // Map nguoc tu DTO -> Entity thi dung AutoMapperConfig
            var mapper = AutoMapperConfig.InitializeAutomapper<ProductSizeDTO, ProductSize>();
            var productSizeCreate = mapper.Map<ProductSize>(productSizeDTO);

            // Thực hiện tạo mới Movie
            await repository.UpdateAsync(productSizeCreate);

            return Ok();
        }

        [HttpDelete(Name = "DeleteProductSize")]
        public async Task<ActionResult> DeleteProductSize(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }
            var productSize = await repository.GetOneAsync(x => x.ProductSizeId == id);
            if (productSize == null)
            {
                return NotFound();
            }
            await repository.RemoveAsync(productSize);
            return NoContent();
        }
    }
}

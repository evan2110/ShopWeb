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
    public class ProductColorController : ControllerBase
    {
        private ProductColorRepository repository;
        private IConfiguration configuration;
        private readonly IMapper _mapper;

        public ProductColorController(ProductColorRepository repository, IConfiguration configuration, IMapper mapper)
        {
            this.repository = repository;
            this.configuration = configuration;
            this._mapper = mapper;
        }

        [HttpPost]
        public async Task<ActionResult<ProductColorDTO>> CreateProductColor([FromBody] ProductColorDTO productColorDTO)
        {

            if (await repository.GetOneAsync(x => x.ProductColorId == productColorDTO.ProductColorId) != null)
            {
                return BadRequest();
            }

            // Tạo đối tượng Movie từ MovieCreate và gán Genre
            // Map nguoc tu DTO -> Entity thi dung AutoMapperConfig
            var mapper = AutoMapperConfig.InitializeAutomapper<ProductColorDTO, ProductColor>();
            var productColorCreate = mapper.Map<ProductColor>(productColorDTO);

            // Thực hiện tạo mới Movie
            await repository.CreateAsync(productColorCreate);

            // Ánh xạ lại đối tượng Movie sau khi tạo thành công
            var resultDTIO = _mapper.Map<ProductColorDTO>(productColorCreate);


            return Ok(resultDTIO);
        }

        [HttpGet("getAll")]
        public async Task<ActionResult<ProductColorDTO>> GetAllProductColors(int pageSize = 0, int pageNumber = 1)
        {
            var ProductColorList = await repository.GetAllAsync(includeProperties: "Color,Product", pageSize: pageSize, pageNumber: pageNumber);
            List<ProductColorDTO> listDTO = _mapper.Map<List<ProductColorDTO>>(ProductColorList);

            return Ok(listDTO);
        }

        [HttpPut("{ProductColor_id:int}", Name = "UpdateProductColor")]
        public async Task<ActionResult<ProductColorDTO>> UpdateProductColor(int ProductColor_id, [FromBody] ProductColorDTO productColorDTO)
        {

            if (productColorDTO == null || ProductColor_id != productColorDTO.ProductColorId)
            {
                return BadRequest();
            }

            // Map nguoc tu DTO -> Entity thi dung AutoMapperConfig
            var mapper = AutoMapperConfig.InitializeAutomapper<ProductColorDTO, ProductColor>();
            var productColorCreate = mapper.Map<ProductColor>(productColorDTO);

            // Thực hiện tạo mới Movie
            await repository.UpdateAsync(productColorCreate);

            return Ok();
        }

        [HttpDelete(Name = "DeleteProductColor")]
        public async Task<ActionResult> DeleteProductColor(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }
            var productColor = await repository.GetOneAsync(x => x.ProductColorId == id);
            if (productColor == null)
            {
                return NotFound();
            }
            await repository.RemoveAsync(productColor);
            return NoContent();
        }
    }
}

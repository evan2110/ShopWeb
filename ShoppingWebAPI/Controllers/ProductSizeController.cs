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
        public async Task<ActionResult<ProductSizeDTO>> CreateRate([FromBody] ProductSizeDTO productSizeDTO)
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
    }
}

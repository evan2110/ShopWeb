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
        public async Task<ActionResult<ProductColorDTO>> CreateRate([FromBody] ProductColorDTO productColorDTO)
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
    }
}

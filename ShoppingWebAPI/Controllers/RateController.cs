using AutoMapper;
using BusinessObject.Models;
using DataAccess.DTO;
using DataAccess.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShoppingWebAPI.Config;

namespace ShoppingWebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RateController : ControllerBase
    {
        private RateRepository repository;
        private IConfiguration configuration;
        private readonly IMapper _mapper;

        public RateController(RateRepository repository, IConfiguration configuration, IMapper mapper)
        {
            this.repository = repository;
            this.configuration = configuration;
            this._mapper = mapper;
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<RateDTO>> CreateRate([FromBody] RateDTO rateDTO)
        {

            if (await repository.GetOneAsync(x => x.RateId == rateDTO.RateId) != null)
            {
                return BadRequest();
            }

            // Tạo đối tượng Movie từ MovieCreate và gán Genre
            // Map nguoc tu DTO -> Entity thi dung AutoMapperConfig
            var mapper = AutoMapperConfig.InitializeAutomapper<RateDTO, Rate>();
            var rateCreate = mapper.Map<Rate>(rateDTO);

            // Thực hiện tạo mới Movie
            await repository.CreateAsync(rateCreate);

            // Ánh xạ lại đối tượng Movie sau khi tạo thành công
            var resultDTIO = _mapper.Map<RateDTO>(rateCreate);


            return Ok(resultDTIO);
        }

        [HttpGet("getRateByProductId")]
        public async Task<ActionResult<RateDTO>> GetRateByProductId(int ProductId, int pageSize = 0, int pageNumber = 1)
        {
            IEnumerable<Rate> RateList = await repository.GetAllAsync(e => e.Status == "Active" && e.ProductId == ProductId, includeProperties: "User", pageSize: pageSize, pageNumber: pageNumber);
            List<RateDTO> listDTO = _mapper.Map<List<RateDTO>>(RateList);

            return Ok(listDTO);
        }
    }
}

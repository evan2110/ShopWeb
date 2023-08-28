using AutoMapper;
using BusinessObject.Models;
using DataAccess.DTO;
using DataAccess.Repository;
using Microsoft.AspNetCore.Mvc;
using ShoppingWebAPI.Config;

namespace ShoppingWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SupportController : ControllerBase
    {
        private SupportRepository repository;
        private IConfiguration configuration;
        private readonly IMapper _mapper;

        public SupportController(SupportRepository repository, IConfiguration configuration, IMapper mapper)
        {
            this.repository = repository;
            this.configuration = configuration;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<ActionResult<SupportDTO>> CreateSupport([FromBody] SupportDTO supportDTO)
        {

            if (await repository.GetOneAsync(x => x.SupportId == supportDTO.SupportId) != null)
            {
                return BadRequest();
            }

            // Map nguoc tu DTO -> Entity thi dung AutoMapperConfig
            var mapper = AutoMapperConfig.InitializeAutomapper<SupportDTO, Support>();
            var supportCreate = mapper.Map<Support>(supportDTO);

            // Thực hiện tạo mới Movie
            await repository.CreateAsync(supportCreate);

            // Ánh xạ lại đối tượng Movie sau khi tạo thành công
            var resultDTIO = _mapper.Map<SupportDTO>(supportCreate);


            return Ok(resultDTIO);
        }

        [HttpGet("getAll")]
        public async Task<ActionResult<SupportDTO>> GetAllSupports(int pageSize = 0, int pageNumber = 1)
        {
            var SupportList = await repository.GetAllAsync(e => e.Status == "Active", includeProperties: "User",pageSize: pageSize, pageNumber: pageNumber);
            
            List<SupportDTO> listDTO = _mapper.Map<List<SupportDTO>>(SupportList);

            return Ok(listDTO);
        }

        [HttpGet("getAllForAdmin")]
        public async Task<ActionResult<SupportDTO>> GetAllSupportsForAdmin(int pageSize = 0, int pageNumber = 1)
        {
            var SupportList = await repository.GetAllAsync(includeProperties: "User", pageSize: pageSize, pageNumber: pageNumber);

            List<SupportDTO> listDTO = _mapper.Map<List<SupportDTO>>(SupportList);

            return Ok(listDTO);
        }

        [HttpPut("{Support_id:int}", Name = "UpdateSupport")]
        public async Task<ActionResult<SupportDTO>> UpdateSupport(int Support_id, [FromBody] SupportDTO supportDTO)
        {

            if (supportDTO == null || Support_id != supportDTO.SupportId)
            {
                return BadRequest();
            }

            // Map nguoc tu DTO -> Entity thi dung AutoMapperConfig
            var mapper = AutoMapperConfig.InitializeAutomapper<SupportDTO, Support>();
            var supportCreate = mapper.Map<Support>(supportDTO);

            // Thực hiện tạo mới Movie
            await repository.UpdateAsync(supportCreate);

            return Ok();
        }
    }
}

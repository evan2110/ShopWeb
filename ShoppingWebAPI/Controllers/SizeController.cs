using AutoMapper;
using DataAccess.DTO;
using DataAccess.Repository;
using Microsoft.AspNetCore.Mvc;

namespace ShoppingWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SizeController : ControllerBase
    {
        private SizeRepository repository;

        private IConfiguration configuration;
        private readonly IMapper _mapper;

        public SizeController(SizeRepository repository, IConfiguration configuration, IMapper mapper)
        {
            this.repository = repository;
            this.configuration = configuration;
            this._mapper = mapper;
        }

        [HttpGet("getAll")]
        public async Task<ActionResult<SizeDTO>> GetAllSizes()
        {
            var SizeList = await repository.GetAllAsync(e => e.Status == "Active");
            List<SizeDTO> listDTO = _mapper.Map<List<SizeDTO>>(SizeList);

            return Ok(listDTO);
        }
    }
}

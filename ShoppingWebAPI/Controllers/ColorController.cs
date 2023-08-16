using AutoMapper;
using BusinessObject.Models;
using DataAccess.DTO;
using DataAccess.Repository;
using Microsoft.AspNetCore.Mvc;

namespace ShoppingWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ColorController : ControllerBase
    {
        private ColorRepository repository;

        private IConfiguration configuration;
        private readonly IMapper _mapper;

        public ColorController(ColorRepository repository, IConfiguration configuration, IMapper mapper)
        {
            this.repository = repository;
            this.configuration = configuration;
            this._mapper = mapper;
        }

        [HttpGet("getAll")]
        public async Task<ActionResult<ColorDTO>> GetAllColors()
        {
            IEnumerable<Color> ColorList;
            ColorList = await repository.GetAllAsync(e => e.Status == "Active");
            List<ColorDTO> listDTO = _mapper.Map<List<ColorDTO>>(ColorList);

            return Ok(listDTO);
        }
    }
}

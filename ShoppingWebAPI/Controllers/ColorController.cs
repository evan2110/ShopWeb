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

        [HttpGet("getAllForAdmin")]
        public async Task<ActionResult<ColorDTO>> GetAllColorsForAdmin(int pageSize = 0, int pageNumber = 1)
        {
            IEnumerable<Color> ColorList;
            ColorList = await repository.GetAllAsync(pageSize: pageSize, pageNumber: pageNumber);
            List<ColorDTO> listDTO = _mapper.Map<List<ColorDTO>>(ColorList);

            return Ok(listDTO);
        }

        [HttpGet("{color_id:int}", Name = "getColor")]
        public async Task<ActionResult<ColorDTO>> GetOneColor(int color_id)
        {

            if (color_id == 0)
            {

                return BadRequest();
            }
            var Color = await repository.GetOneAsync(x => x.ColorId == color_id);

            if (Color == null)
            {

                return NotFound();
            }

            ColorDTO colorDTO = _mapper.Map<ColorDTO>(Color);

            return Ok(colorDTO);
        }

        [HttpPost]
        public async Task<ActionResult<ColorDTO>> CreateColor([FromBody] ColorDTO colorDTO)
        {

            if (await repository.GetOneAsync(x => x.ColorId == colorDTO.ColorId) != null)
            {
                return BadRequest();
            }

            // Tạo đối tượng Movie từ MovieCreate và gán Genre
            // Map nguoc tu DTO -> Entity thi dung AutoMapperConfig
            var mapper = AutoMapperConfig.InitializeAutomapper<ColorDTO, Color>();
            var colorCreate = mapper.Map<Color>(colorDTO);

            // Thực hiện tạo mới Movie
            await repository.CreateAsync(colorCreate);

            // Ánh xạ lại đối tượng Movie sau khi tạo thành công
            var resultDTIO = _mapper.Map<ColorDTO>(colorCreate);


            return Ok(resultDTIO);
        }

        [HttpPut("{Color_id:int}", Name = "UpdateColor")]
        public async Task<ActionResult<ColorDTO>> UpdateColor(int Color_id, [FromBody] ColorDTO colorDTO)
        {

            if (colorDTO == null || Color_id != colorDTO.ColorId)
            {
                return BadRequest();
            }

            // Map nguoc tu DTO -> Entity thi dung AutoMapperConfig
            var mapper = AutoMapperConfig.InitializeAutomapper<ColorDTO, Color>();
            var colorCreate = mapper.Map<Color>(colorDTO);

            // Thực hiện tạo mới Movie
            await repository.UpdateAsync(colorCreate);

            return Ok();
        }
    }
}

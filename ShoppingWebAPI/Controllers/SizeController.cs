using AutoMapper;
using BusinessObject.Models;
using DataAccess.DTO;
using DataAccess.Repository;
using Microsoft.AspNetCore.Mvc;
using ShoppingWebAPI.Config;
using System.Drawing.Printing;

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

        [HttpGet("getAllForAdmin")]
        public async Task<ActionResult<SizeDTO>> GetAllSizesForAdmin(int pageSize = 0, int pageNumber = 1)
        {
            var SizeList = await repository.GetAllAsync(pageSize: pageSize, pageNumber: pageNumber);
            List<SizeDTO> listDTO = _mapper.Map<List<SizeDTO>>(SizeList);

            return Ok(listDTO);
        }

        [HttpGet("{size_id:int}", Name = "getSize")]
        public async Task<ActionResult<SizeDTO>> GetOneSize(int size_id)
        {

            if (size_id == 0)
            {

                return BadRequest();
            }
            var Size = await repository.GetOneAsync(x => x.SizeId == size_id);

            if (Size == null)
            {

                return NotFound();
            }

            SizeDTO sizeDTO = _mapper.Map<SizeDTO>(Size);

            return Ok(sizeDTO);
        }

        [HttpPost]
        public async Task<ActionResult<SizeDTO>> CreateSize([FromBody] SizeDTO sizeDTO)
        {

            if (await repository.GetOneAsync(x => x.SizeId == sizeDTO.SizeId) != null)
            {
                return BadRequest();
            }

            // Tạo đối tượng Movie từ MovieCreate và gán Genre
            // Map nguoc tu DTO -> Entity thi dung AutoMapperConfig
            var mapper = AutoMapperConfig.InitializeAutomapper<SizeDTO, Size>();
            var sizeCreate = mapper.Map<Size>(sizeDTO);

            // Thực hiện tạo mới Movie
            await repository.CreateAsync(sizeCreate);

            // Ánh xạ lại đối tượng Movie sau khi tạo thành công
            var resultDTIO = _mapper.Map<SizeDTO>(sizeCreate);


            return Ok(resultDTIO);
        }

        [HttpPut("{Size_id:int}", Name = "UpdateSize")]
        public async Task<ActionResult<SizeDTO>> UpdateSize(int Size_id, [FromBody] SizeDTO sizeDTO)
        {

            if (sizeDTO == null || Size_id != sizeDTO.SizeId)
            {
                return BadRequest();
            }

            // Map nguoc tu DTO -> Entity thi dung AutoMapperConfig
            var mapper = AutoMapperConfig.InitializeAutomapper<SizeDTO, Size>();
            var sizeCreate = mapper.Map<Size>(sizeDTO);

            // Thực hiện tạo mới Movie
            await repository.UpdateAsync(sizeCreate);

            return Ok();
        }
    }
}

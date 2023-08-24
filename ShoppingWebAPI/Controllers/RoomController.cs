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
    public class RoomController : ControllerBase
    {
        private RoomRepository repository;
        private IConfiguration configuration;
        private readonly IMapper _mapper;

        public RoomController(RoomRepository repository, IConfiguration configuration, IMapper mapper)
        {
            this.repository = repository;
            this.configuration = configuration;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<ActionResult<RoomDTO>> CreateRoom([FromBody] RoomDTO roomDTO)
        {

            if (await repository.GetOneAsync(x => x.RoomId == roomDTO.RoomId) != null)
            {
                return BadRequest();
            }

            // Map nguoc tu DTO -> Entity thi dung AutoMapperConfig
            var mapper = AutoMapperConfig.InitializeAutomapper<RoomDTO, Room>();
            var roomCreate = mapper.Map<Room>(roomDTO);

            // Thực hiện tạo mới Movie
            await repository.CreateAsync(roomCreate);

            // Ánh xạ lại đối tượng Movie sau khi tạo thành công
            var resultDTIO = _mapper.Map<RoomDTO>(roomCreate);


            return Ok(resultDTIO);
        }

        [HttpGet("getAll")]
        public async Task<ActionResult<RoomDTO>> GetAllRooms([FromQuery(Name = "search")] string? search, int pageSize = 0, int pageNumber = 1)
        {
            var RoomList = await repository.GetAllAsync(e => e.Status == "Active", includeProperties: "User", pageSize: pageSize, pageNumber: pageNumber);

            List<RoomDTO> listDTO = _mapper.Map<List<RoomDTO>>(RoomList);

            return Ok(listDTO);
        }

        [HttpGet("{room_id:int}", Name = "getRoom")]
        public async Task<ActionResult<RoomDTO>> GetOneRoom(int room_id)
        {

            if (room_id == 0)
            {

                return BadRequest();
            }
            var Room = await repository.GetOneAsync(x => x.RoomId == room_id, includeProperties: "User");

            if (Room == null)
            {

                return NotFound();
            }
            RoomDTO roomDTO = _mapper.Map<RoomDTO>(Room);
            return Ok(roomDTO);
        }

        [HttpGet]
        public async Task<ActionResult<RoomDTO>> GetRoomByUserId([FromQuery] int user_id)
        {

            if (user_id == 0)
            {

                return BadRequest();
            }
            var Room = await repository.GetAllAsync(x => x.UserId == user_id, includeProperties: "User");

            if (Room == null)
            {

                return NotFound();
            }
            List<RoomDTO> roomDTO = _mapper.Map<List<RoomDTO>>(Room);
            return Ok(roomDTO);
        }
    }
}

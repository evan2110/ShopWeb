using AutoMapper;
using BusinessObject.Models;
using DataAccess.DTO;
using DataAccess.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShoppingWebAPI.Config;
using System.Drawing.Printing;

namespace ShoppingWebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private OrderRepository repository;
        private IConfiguration configuration;
        private readonly IMapper _mapper;

        public OrderController(OrderRepository repository, IConfiguration configuration, IMapper mapper)
        {
            this.repository = repository;
            this.configuration = configuration;
            this._mapper = mapper;
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<OrderDTO>> CreateOrder([FromBody] OrderDTO orderDTO)
        {

            if (await repository.GetOneAsync(x => x.OrderId == orderDTO.OrderId) != null)
            {
                return BadRequest();
            }

            // Tạo đối tượng Movie từ MovieCreate và gán Genre
            var orderCreate = _mapper.Map<Order>(orderDTO);


            // Thực hiện tạo mới Movie
            await repository.CreateAsync(orderCreate);

            // Ánh xạ lại đối tượng Movie sau khi tạo thành công
            var resultDTIO = _mapper.Map<OrderDTO>(orderCreate);


            return Ok(resultDTIO);
        }

        [HttpGet("getAll")]
        public async Task<ActionResult<OrderDTO>> GetAllOrders(int pageSize = 0, int pageNumber = 1)
        {
            var OrderList = await repository.GetAllAsync(e => e.Status == "Active", includeProperties: "User", pageSize: pageSize, pageNumber: pageNumber);

            List<OrderDTO> listDTO = _mapper.Map<List<OrderDTO>>(OrderList);

            return Ok(listDTO);
        }

        [HttpGet("getAllForAdmin")]
        public async Task<ActionResult<OrderDTO>> GetAllOrdersForAdmin(int pageSize = 0, int pageNumber = 1)
        {
            var OrderList = await repository.GetAllAsync(includeProperties: "User", pageSize: pageSize, pageNumber: pageNumber);

            List<OrderDTO> listDTO = _mapper.Map<List<OrderDTO>>(OrderList);

            return Ok(listDTO);
        }

        [HttpPut("{Order_id:int}", Name = "UpdateOrder")]
        public async Task<ActionResult<OrderDTO>> UpdateOrder(int Order_id, [FromBody] OrderDTO orderDTO)
        {

            if (orderDTO == null || Order_id != orderDTO.OrderId)
            {
                return BadRequest();
            }

            // Map nguoc tu DTO -> Entity thi dung AutoMapperConfig
            var mapper = AutoMapperConfig.InitializeAutomapper<OrderDTO, Order>();
            var orderCreate = mapper.Map<Order>(orderDTO);

            // Thực hiện tạo mới Movie
            await repository.UpdateAsync(orderCreate);

            return Ok();
        }

        [HttpGet("{order_id:int}", Name = "getOrder")]
        public async Task<ActionResult<OrderDTO>> GetOneOrder(int order_id)
        {

            if (order_id == 0)
            {

                return BadRequest();
            }
            var order = await repository.GetOneAsync(x => x.OrderId == order_id);

            if (order == null)
            {

                return NotFound();
            }

            OrderDTO orderDTO = _mapper.Map<OrderDTO>(order);

            return Ok(orderDTO);
        }
    }
}

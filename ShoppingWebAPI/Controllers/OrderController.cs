using AutoMapper;
using BusinessObject.Models;
using DataAccess.DTO;
using DataAccess.Repository;
using Microsoft.AspNetCore.Mvc;

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
    }
}

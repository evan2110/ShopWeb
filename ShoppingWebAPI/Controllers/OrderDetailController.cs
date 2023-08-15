using AutoMapper;
using BusinessObject.Models;
using DataAccess.DTO;
using DataAccess.Repository;
using Microsoft.AspNetCore.Mvc;

namespace ShoppingWebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderDetailController : ControllerBase
    {
        private OrderDetailRepository repository;
        private IConfiguration configuration;
        private readonly IMapper _mapper;

        public OrderDetailController(OrderDetailRepository repository, IConfiguration configuration, IMapper mapper)
        {
            this.repository = repository;
            this.configuration = configuration;
            this._mapper = mapper;
        }

        [HttpPost]
        public async Task<ActionResult<OrderDTO>> CreateOrderDetail([FromBody] OrderDetailDTO orderDetailDTO)
        {

            if (await repository.GetOneAsync(x => x.OrderDetailId == orderDetailDTO.OrderDetailId) != null)
            {
                return BadRequest();
            }

            // Tạo đối tượng Movie từ MovieCreate và gán Genre
            var orderDetailCreate = _mapper.Map<OrderDetail>(orderDetailDTO);


            // Thực hiện tạo mới Movie
            await repository.CreateAsync(orderDetailCreate);

            // Ánh xạ lại đối tượng Movie sau khi tạo thành công
            var resultDTIO = _mapper.Map<OrderDetailDTO>(orderDetailCreate);


            return Ok(resultDTIO);
        }
    }
}

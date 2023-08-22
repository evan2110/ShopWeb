using AutoMapper;
using BusinessObject.Models;
using DataAccess.DTO;
using DataAccess.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShoppingWebAPI.Config;
using Stripe;
using System.Runtime.InteropServices;

namespace ShoppingWebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderDetailController : ControllerBase
    {
        private OrderRepository orderRepository;
        private OrderDetailRepository repository;
        private IConfiguration configuration;
        private readonly IMapper _mapper;

        public OrderDetailController(OrderDetailRepository repository, OrderRepository orderRepository, IConfiguration configuration, IMapper mapper)
        {
            this.repository = repository;
            this.configuration = configuration;
            this._mapper = mapper;
            this.orderRepository = orderRepository;
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<OrderDetailDTO>> CreateOrderDetail([FromBody] OrderDetailDTO orderDetailDTO)
        {

            if (await repository.GetOneAsync(x => x.OrderDetailId == orderDetailDTO.OrderDetailId) != null)
            {
                return BadRequest();
            }

            // Tạo đối tượng Movie từ MovieCreate và gán Genre
            var mapper = AutoMapperConfig.InitializeAutomapper<OrderDetailDTO, OrderDetail>();
            var orderDetailCreate = mapper.Map<OrderDetail>(orderDetailDTO);


            // Thực hiện tạo mới Movie
            await repository.CreateAsync(orderDetailCreate);

            // Ánh xạ lại đối tượng Movie sau khi tạo thành công
            var resultDTIO = _mapper.Map<OrderDetailDTO>(orderDetailCreate);


            return Ok(resultDTIO);
        }

        [HttpGet("topBuyProduct")]
        public async Task<ActionResult<TopBuyProductDTO>> GetTopBuyProducts()
        {
            IEnumerable<OrderDetail> OrderDetailList;
            OrderDetailList = await repository.GetAllAsync(e => e.Status == "Active", includeProperties: "Product,Order");

            var productGroupsWithTotalQuantity = OrderDetailList
                .GroupBy(od => od.ProductId)
                .Select(group => new TopBuyProductDTO
                {
                    ProductId = group.Key,
                    Quantity = group.Sum(od => od.Quantity),
                    ImageFront = group.First().Product.ImageFront,
                    ImageBehind = group.First().Product.ImageBehind,
                    ProductName = group.First().Product.ProductName,
                    Price = group.First().Product.Price,
                    Discount = group.First().Product.Discount,
                    Status = group.First().Product.Status,
                    CreatedTime = group.First().Product.CreatedTime,
                    UpdatedTime = group.First().Product.UpdatedTime,
                })
                .OrderByDescending(group => group.Quantity)
                .Take(7)
                .ToList();

            return Ok(productGroupsWithTotalQuantity);
           
        }

        [Authorize]
        [HttpGet("{userId:int}", Name = "GetOrderDetailByUserId")]
        public async Task<ActionResult<OrderDetailDTO>> GetOrderDetailByUserId(int userId, int pageSize = 0, int pageNumber = 1)
        {
            List<OrderDetailDTO> OrderDetailList = new List<OrderDetailDTO>();
            List<OrderDetail> OrderDetail = new List<OrderDetail>();
            //Vi o day co mac dinh 1 thang null ban dau cho nen pageSize muon lay dung phai + 1

            var OrderList = await orderRepository.GetAllAsync(e => e.Status == "Active" && e.UserId == userId, includeProperties: "OrderDetails", pageSize: pageSize, pageNumber: pageNumber);
            foreach(var item in  OrderList)
            {
                OrderDetail = await repository.GetAllAsync(e => e.Status == "Active" && e.OrderId == item.OrderId, includeProperties: "Product");
                var orderDetailMap = _mapper.Map<List<OrderDetailDTO>>(OrderDetail);
                OrderDetailList.AddRange(orderDetailMap);
            }

            return Ok(OrderDetailList);

        }

    }
}

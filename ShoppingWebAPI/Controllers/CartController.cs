using AutoMapper;
using BusinessObject.Models;
using DataAccess.DTO;
using DataAccess.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShoppingWebAPI.Response;
using Stripe;
using Stripe.Checkout;

namespace ShoppingWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private CartRepository repository;
        private CartItemRepository itemRepository;

        private IConfiguration configuration;
        private readonly IMapper _mapper;
        private const string StripeSecretKey = "sk_test_51NbbByJOnFaMEUHwIiEpcY8haaIPrVv8sQzV31Bk1A1uPu1fNxO7SgKNzFxf0Mubbge30z1Euv8tWfDe8klpHmmq00A2X9qaoI";

        public CartController(CartRepository repository, IConfiguration configuration, IMapper mapper, CartItemRepository itemRepository)
        {
            this.repository = repository;
            this.configuration = configuration;
            this._mapper = mapper;
            this.itemRepository = itemRepository;
        }

        [HttpGet("{user_id:int}", Name = "getCart")]
        public async Task<ActionResult<CartDTO>> GetOneCart(int user_id)
        {
            if (user_id == 0)
            {

                return BadRequest();
            }
            var Cart = await repository.GetOneAsync(x => x.UserId == user_id, includeProperties: "User");
            IEnumerable<CartItem> CartItemlist = null;
            CartDTO cartDTO = null;
            if (Cart != null)
            {
                CartItemlist = await itemRepository.GetAllAsync(e => e.CartId == Cart.CartId, includeProperties: "Product,Color,Size");
                cartDTO = _mapper.Map<CartDTO>(Cart);
                if (CartItemlist != null)
                {
                    List<CartItemDTO> cartItemDTO = _mapper.Map<List<CartItemDTO>>(CartItemlist);

                    cartDTO.CartItemDTOs = cartItemDTO;
                }
            }

            

            return Ok(cartDTO);
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<CartDTO>> CreateCart([FromBody] CartDTO CartDTO)
        {

            if (await repository.GetOneAsync(x => x.CartId == CartDTO.CartId) != null)
            {
                return BadRequest();
            }

            // Tạo đối tượng Movie từ MovieCreate và gán Genre
            var cartCreate = _mapper.Map<Cart>(CartDTO);


            // Thực hiện tạo mới Movie
            await repository.CreateAsync(cartCreate);

            // Ánh xạ lại đối tượng Movie sau khi tạo thành công
            var resultDTIO = _mapper.Map<CartDTO>(cartCreate);


            return Ok(resultDTIO);
        }

        [HttpPost("create-payment-intent")]
        public ActionResult<PaymentStripeResponse> CreatePaymentIntent([FromBody]List<OrderDetailDTO> lstOrderDetailDTOs)
        {
            StripeConfiguration.ApiKey = StripeSecretKey;

            var successUrl = "http://localhost:5251/PaymentStatus?status=suss";
            var cancelUrl = "http://localhost:5251/PaymentStatus?status=fail";

            var sessionOptions = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                Mode = "payment",
                SuccessUrl = successUrl,
                CancelUrl = cancelUrl,
                LineItems = new List<SessionLineItemOptions>(),
            };

            if (lstOrderDetailDTOs.Count > 0)
            {
                foreach (var item in lstOrderDetailDTOs)
                {
                    var orderItem = new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            UnitAmount = (long?)item.Price * 100, 
                            Currency = "USD",
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = item.ProductName,
                                Images = new List<string>
                                {
                                    item.Image
                                },
                                Description = "This price has not been deducted after entering the coupon code",
                            },
                        },
                        Quantity = item.Quantity, 
                    };
                    sessionOptions.LineItems.Add(orderItem);
                }
            }
            else
            {
                var sessionListItem = new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = 100,
                        Currency = "USD",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = "Ngu",
                        }
                    },
                    Quantity = 1,
                };
                sessionOptions.LineItems.Add(sessionListItem);
            }



            var sessionService = new SessionService();
            var session = sessionService.Create(sessionOptions);
            var sessionId = session.Id;
            return new PaymentStripeResponse { SessionId = sessionId, Url = session.Url , Status = session.PaymentStatus};
        }

        [HttpGet("check-payment-status/{sessionId}")]
        public ActionResult<PaymentStripeResponse> CheckPaymentStatus(string sessionId)
        {
            StripeConfiguration.ApiKey = StripeSecretKey;

            var sessionService = new SessionService();
            var session = sessionService.Get(sessionId);

            // Kiểm tra trạng thái thanh toán
            if (session.PaymentStatus == "paid")
            {

                return new PaymentStripeResponse { Status = "paid" };
            }

            return new PaymentStripeResponse { Status = "Chua" };
        }

    }
}

using AutoMapper;
using BusinessObject.Models;
using DataAccess.DTO;
using DataAccess.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
                CartItemlist = await itemRepository.GetAllAsync(e => e.CartId == Cart.CartId, includeProperties: "Product");
                cartDTO = _mapper.Map<CartDTO>(Cart);
                if (CartItemlist != null)
                {
                    List<CartItemDTO> cartItemDTO = _mapper.Map<List<CartItemDTO>>(CartItemlist);

                    cartDTO.CartItemDTOs = cartItemDTO;
                }
            }

            

            return Ok(cartDTO);
        }
    }
}

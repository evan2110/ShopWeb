using AutoMapper;
using BusinessObject.Models;
using DataAccess.DTO;
using DataAccess.Repository;
using Microsoft.AspNetCore.Mvc;

namespace ShoppingWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartItemController : ControllerBase
    {
        private CartItemRepository itemRepository;
        private CartRepository cartRepository;
        private ProductRepository productRepository;
        private UserRepository userRepository;

        private IConfiguration configuration;
        private readonly IMapper _mapper;
        public CartItemController(IConfiguration configuration, IMapper mapper, CartItemRepository itemRepository, CartRepository cartRepository, ProductRepository productRepository, UserRepository userRepository)
        {
            this.itemRepository = itemRepository;
            this.configuration = configuration;
            this._mapper = mapper;
            this.cartRepository = cartRepository;
            this.productRepository = productRepository;
            this.userRepository = userRepository;
        }

        [HttpGet]
        public async Task<ActionResult<CartDTO>> GetOneCartItem([FromQuery] int? cartId,
            [FromQuery] int? productId, [FromQuery] int? colorId, [FromQuery] int? sizeId)
        {
            var CartItem = await itemRepository.GetOneAsync(x => x.CartId == cartId && x.ProductId == productId && x.ColorId == colorId && x.SizeId == sizeId, includeProperties: "Product,Cart,Color,Size");
            CartItemDTO cartItemDTO = null;
            if (CartItem != null)
            {
                cartItemDTO = _mapper.Map<CartItemDTO>(CartItem);
            }



            return Ok(cartItemDTO);
        }

        //////////////// Create/////////////////////
        [HttpPost]
        public async Task<ActionResult<CartItemDTO>> CreateCartItem([FromBody] CartItemDTO CartItemCreateDTO)
        {

            if (await itemRepository.GetOneAsync(x => x.CartItemId == CartItemCreateDTO.CartItemId) != null)
            {
                return BadRequest();
            }

            // Tạo đối tượng Movie từ MovieCreate và gán Genre
            var cartItemCreate = _mapper.Map<CartItem>(CartItemCreateDTO);

            // Thực hiện tạo mới Movie
            await itemRepository.CreateAsync(cartItemCreate);

            // Ánh xạ lại đối tượng Movie sau khi tạo thành công
            var resultDTIO = _mapper.Map<CartItemDTO>(cartItemCreate);


            return Ok(resultDTIO);
        }


        [HttpPut("{CartItem_id:int}", Name = "UpdateCartItem")]
        public async Task<ActionResult<CartItemDTO>> UpdateCartItem(int CartItem_id, [FromBody] CartItemDTO CartItemDTO)
        {

            if (CartItemDTO == null || CartItem_id != CartItemDTO.CartItemId)
            {
                return BadRequest();
            }
            CartItem newUpdateCartItem = _mapper.Map<CartItem>(CartItemDTO);
            newUpdateCartItem.UpdatedTime = DateTime.Now;
            await itemRepository.UpdateAsync(newUpdateCartItem);
            return Ok();
        }

        [HttpDelete(Name = "DeleteCartItem")]
        public async Task<ActionResult> DeleteCartItem(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }
            var cartItem = await itemRepository.GetOneAsync(x => x.CartItemId == id);
            if (cartItem == null)
            {
                return NotFound();
            }
            await itemRepository.RemoveAsync(cartItem);
            return NoContent();
        }

        // DELETE: api/cart/DeleteCartItemByCartId/{CartId}
        [HttpDelete("DeleteCartItemByCartId/{CartId}", Name = "DeleteCartItemByCartId")]
        public async Task<ActionResult> DeleteCartItemByCartId(int CartId)
        {
            if (CartId == 0)
            {
                return BadRequest();
            }
            var cartItem = await itemRepository.GetAllAsync(x => x.CartId == CartId);
            if (cartItem == null)
            {
                return NotFound();
            }
            foreach(var item in  cartItem)
            {
                await itemRepository.RemoveAsync(item);
            }
            return NoContent();
        }

        // DELETE: api/cart/DeleteCartItemById/{id}
        [HttpDelete("DeleteCartItemById/{id}", Name = "DeleteCartItemById")]
        public async Task<ActionResult> DeleteCartItemById(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }
            var cartItem = await itemRepository.GetOneAsync(x => x.CartItemId == id);
            if (cartItem == null)
            {
                return NotFound();
            }
            
            await itemRepository.RemoveAsync(cartItem);
            return NoContent();
        }
    }
}

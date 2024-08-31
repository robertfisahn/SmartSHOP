using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartShopAPI.Entities;
using SmartShopAPI.Interfaces;
using SmartShopAPI.Models.Dtos.CartItem;

namespace SmartShopAPI.Controllers
{
    [Route("api/cart")]
    [ApiController]
    [Authorize]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;
        public CartController(ICartService cartService) {
            _cartService = cartService;
        }

        [HttpGet("{userId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        public ActionResult<IEnumerable<CartItem>> GetAll([FromRoute]int userId) {
           var cartItems = _cartService.GetById(userId);
            return Ok(cartItems);
        }

        [HttpPost("add")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        public ActionResult AddToCart([FromBody]CreateCartItemDto dto)
        {
            var cartItemId = _cartService.AddItemToCart(dto);
            return Created($"api/cart/{cartItemId}", null);
        }

        [HttpDelete("delete/{cartItemId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        public ActionResult DeleteCartItem([FromRoute]int cartItemId) {
            _cartService.DeleteItemFromCart(cartItemId);
            return NoContent();
        }

        [HttpPut("update/{cartItemId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        public ActionResult UpdateCartItem([FromRoute]int cartItemId, [FromBody]UpdateCartItemDto dto)
        {
            _cartService.UpdateCartItem(cartItemId, dto);
            return Ok();
        }

        [HttpDelete("clear/{userId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        public ActionResult ClearCart([FromRoute]int userId)
        {
            _cartService.ClearCartItems(userId);
            return NoContent();
        }
    }
}

using AgroMind.GP.Core.Entities;
using AgroMind.GP.Core.Repositories.Contract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AgroMind.GP.APIs.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CartController : ControllerBase
	{
		private readonly ICartRepository _cartRepository;
		public CartController(ICartRepository cartRepository)
		{
			_cartRepository = cartRepository;
		}

		//Get or ReCreate Cart
		[HttpGet ("{id}")]
		public async Task<ActionResult<Cart>> GetCart(string id) //<ActionResult<Cart> : will return Cart
		{
			var cart= await _cartRepository.GetCartAsync(id); //Law Null don't have cart with this id
			if (cart is null) //kant mawogode and it deleted because expire date
			return new Cart(id); //reCreate > Same Cart wit same Id el kant Mawgoda
			return Ok(cart);
		}

		//Update or Create New Cart
		[HttpPost("UpdateCart")]
		public async Task<ActionResult<Cart>>UpdateCart(Cart cart) 
		{
		    var CreatedOrUpdatedCart= await _cartRepository.UpdateCarttAsync(cart);
			//if (CreatedOrUpdatedCart is null) return BadRequest(new BadRequestObjectResult(400)); //Frontend Problem /not create and not update
																								  //Return BadRequest() for frontend not User 		    
			return Ok(CreatedOrUpdatedCart);
		}

		//Delete Cart
		[HttpDelete("{Cartid}") ]
		public async Task<ActionResult<bool>> DeleteCart(string Cartid) //Deleted or No
		{ 
		   return await _cartRepository.DeleteCartAsync(Cartid);
		}

		[HttpDelete("RemoveFromCart")]

		public async Task<ActionResult<Cart>> RemoveFromCart(string cartId ,int ItemId)
		{
			var cart=await _cartRepository.RemoveFromCart(cartId,ItemId);
			if(cart is null) 
				return NotFound();
			return Ok(cart);
		}

	}
}

using AgroMind.GP.Core.Contracts.Services.Contract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs;
using System.Security.Claims;

namespace AgroMind.GP.APIs.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CartController : ControllerBase
	{
		private readonly ICartService _cartService; // Inject the service
		public CartController(ICartService cartService)
		{
			_cartService = cartService;
		}



		// --- Get User's Cart ---
		
		// returns The user's CartDto
		[HttpGet] // GET api/cart (implicitly uses logged-in user's ID)
		[Authorize] 
		public async Task<ActionResult<CartDto>> GetCart()
		{
			var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			if (string.IsNullOrEmpty(userId)) return Unauthorized("User ID not found.");

			var cart = await _cartService.GetUserCartAsync(userId);
			return Ok(cart);
		}

		// --- Add Item to Cart / Update Quantity ---
		
		// Adds a product to the cart or updates its quantity.
		
		[HttpPost("items")] // POST api/cart/items
		[Authorize]
		public async Task<ActionResult<CartDto>> AddOrUpdateCartItem(AddToCartDto itemDto)
		{
			var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			if (string.IsNullOrEmpty(userId)) return Unauthorized("User ID not found.");

			try
			{
				var updatedCart = await _cartService.AddItemToCartAsync(userId, itemDto);
				return Ok(updatedCart);
			}
			catch (KeyNotFoundException ex)
			{
				return NotFound(ex.Message);
			}
			catch (Exception ex)
			{
				return StatusCode(500, $"An error occurred while adding/updating item: {ex.Message}");
			}
		}

		// --- Update Item Quantity (Specific endpoint if needed) ---
	
		[HttpPut("items/{productId}/quantity")] // PUT api/cart/items/{productId}/quantity
		[Authorize]
		public async Task<ActionResult<CartDto>> UpdateCartItemQuantity(int productId, [FromQuery] int newQuantity)
		{
			var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			if (string.IsNullOrEmpty(userId)) return Unauthorized("User ID not found.");

			try
			{
				var updatedCart = await _cartService.UpdateItemQuantityAsync(userId, productId, newQuantity);
				return Ok(updatedCart);
			}
			catch (KeyNotFoundException ex)
			{
				return NotFound(ex.Message);
			}
			catch (Exception ex)
			{
				return StatusCode(500, $"An error occurred while updating item quantity: {ex.Message}");
			}
		}

		// --- Remove Item from Cart ---
		
		[HttpDelete("items/{productId}")] // DELETE api/cart/items/{productId}
		[Authorize]
		public async Task<ActionResult<CartDto>> RemoveItemFromCart(int productId)
		{
			var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			if (string.IsNullOrEmpty(userId)) return Unauthorized("User ID not found.");

			try
			{
				var updatedCart = await _cartService.RemoveItemFromCartAsync(userId, productId);
				return Ok(updatedCart);
			}
			catch (Exception ex)
			{
				return StatusCode(500, $"An error occurred while removing item: {ex.Message}");
			}
		}

		// --- Clear User's Cart ---
		
		[HttpDelete]
		[Authorize]
		public async Task<ActionResult<bool>> ClearCart()
		{
			var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			if (string.IsNullOrEmpty(userId)) return Unauthorized("User ID not found.");

			var result = await _cartService.ClearUserCartAsync(userId);
			if (!result) return BadRequest("Failed to clear cart.");
			return Ok(result);
		}
	}

}


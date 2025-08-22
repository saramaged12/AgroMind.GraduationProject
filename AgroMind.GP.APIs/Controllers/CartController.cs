using AgroMind.GP.Core.Contracts.Services.Contract;
using AgroMind.GP.Service.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs;
using Shared.DTOs.CartDtos;
using System.Security.Claims;

namespace AgroMind.GP.APIs.Controllers
{
    [Route("api/[controller]")]
	[ApiController]
	public class CartController : ControllerBase
	{
		private readonly IServiceManager _Servicemanager; // Inject the service
		public CartController(IServiceManager Servicemanager)
			
		{
			_Servicemanager= Servicemanager;
		}

		[HttpGet]
		[Authorize]
		public async Task<ActionResult<CartDto>> GetCart(string id)
		{
			var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			if (string.IsNullOrEmpty(userId)) return Unauthorized("User ID not found.");

			var cart = await _Servicemanager.CartService.GetUserCartAsync(id);
			return Ok(cart);
		}

		


		 //Add or Update Cart Items


		[HttpPost] 
		[Authorize]
		public async Task<ActionResult<CartDto>> AddOrUpdateCart(CartDto cartDto)
		{
		
		
				var updatedCart = await _Servicemanager.CartService.UpdateCartAsync(cartDto);
				return Ok(updatedCart);

		}





		 //--- Clear User's Cart ---

		[HttpDelete]
		[Authorize]
		public async Task<IActionResult> ClearCart(string id)
		{
			

		     await _Servicemanager.CartService.ClearUserCartAsync(id);
			return NoContent();

			
		}
	}

}


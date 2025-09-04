using AgroMind.GP.Core.Contracts.Services.Contract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs.OrderDTOs;
using System.Security.Claims;

namespace AgroMind.GP.APIs.Controllers
{
	[Route("api/[controller]")]
	[ApiController]

	[Authorize]
	public class OrderController(IServiceManager serviceManager) : ControllerBase
	{
		
		[HttpPost] //POST: /api/Order
		public async Task<ActionResult<OrderToReturnDto>> CreateOrder(OrderToCreateDto orderDto)
		{
			var BuerEmail=User.FindFirstValue(ClaimTypes.Email);
			var result = await serviceManager.OrderService.CreateOrderAsync(BuerEmail!,orderDto);
			return Ok(result);
		}

		[HttpGet]
		public async Task<ActionResult<IReadOnlyList<OrderToReturnDto>>> GetOrdersForUser()
		{
			var BuyerEmail = User.FindFirstValue(ClaimTypes.Email);

			var Orders = await serviceManager.OrderService.GetOrdersForUserAsync(BuyerEmail!);
			return Ok(Orders);
			
		}
		[HttpGet("{id}")] //Get api/Order/3
		public async Task<ActionResult<OrderToReturnDto>> GetOrderByIdForUser(int id)
		{
			var BuyerEmail = User.FindFirstValue(ClaimTypes.Email);

			var result = await serviceManager.OrderService.GetOrderByIdAsync( BuyerEmail!,id);
			
			return Ok(result);
		}
		[HttpGet("deliveryMethods")]
		public async Task<ActionResult<IReadOnlyList<DeliveryMethodDto>>> GetDeliveryMethods()
		{
			var result = await serviceManager.OrderService.GetDeliveryMethodsAsync();
			return Ok(result);
		}

	}
}

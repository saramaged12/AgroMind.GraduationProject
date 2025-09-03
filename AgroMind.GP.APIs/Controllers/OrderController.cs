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

	}
}

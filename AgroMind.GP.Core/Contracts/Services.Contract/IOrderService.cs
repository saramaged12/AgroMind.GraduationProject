using Shared.DTOs.OrderDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgroMind.GP.Core.Contracts.Services.Contract
{
	public interface IOrderService
	{
		Task<OrderToReturnDto> CreateOrderAsync(string buyerEmail, OrderToCreateDto order);

		Task<OrderToReturnDto> GetOrderByIdAsync(string buyerEmail,int OrderId);

		Task<IReadOnlyList<OrderToReturnDto>> GetOrdersForUserAsync(string buyerEmail);

		Task<IReadOnlyList<DeliveryMethodDto>> GetDeliveryMethodsAsync();
	}
}

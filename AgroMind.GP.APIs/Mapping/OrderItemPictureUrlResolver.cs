using AgroMind.GP.Core.Entities.Orders;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Shared.DTOs.OrderDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.G04.Core.Mapping.Products
{
	public class OrderItemPictureURLResolver : IValueResolver<OrderItems, OrderItemsDto, string>
	{
		private readonly IConfiguration configuration;

		public OrderItemPictureURLResolver(IConfiguration configuration)
		{
			this.configuration = configuration;
		}

		public string Resolve(OrderItems source, OrderItemsDto destination, string destMember, ResolutionContext context)
		{
			if (!string.IsNullOrEmpty(source.Product.PictureUrl))
			{
				return $"{configuration.GetSection("Urls")["ApiBaseUrl"]}{source.Product.PictureUrl}";
			}
			

				return string.Empty;

		}
	}
}

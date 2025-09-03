using AgroMind.GP.Core.Contracts.Repositories.Contract;
using AgroMind.GP.Core.Contracts.Services.Contract;
using AgroMind.GP.Core.Contracts.UnitOfWork.Contract;
using AgroMind.GP.Core.Entities.Orders;
using AgroMind.GP.Core.Entities.ProductModule;
using AgroMind.GP.Core.Exceptions;
using AgroMind.GP.Core.Specification;
using AgroMind.GP.Repository.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.Mvc.Formatters;
using Shared.DTOs.OrderDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgroMind.GP.Service.Services
{
	public class OrderService(ICartService cartService,IUnitOfWork unitOfWork ,IMapper mapper) : IOrderService
	{
		

		public async Task<OrderToReturnDto> CreateOrderAsync(string buyerEmail, OrderToCreateDto order)
		{
			// 1- Get Basket from the Basket Repo

			var cart= await cartService.GetUserCartAsync(order.CartId);

			// 2- Get Selected items at Basket from the Product Repo
			
			var OrderItems = new List<OrderItems>();
			if(cart.Items.Count() > 0)
			{
				var ProductRepository = unitOfWork.GetRepositories<Product, int>();

				foreach (var item in cart.Items)
				{
					
					var Product = await ProductRepository.GetByIdAsync(item.Id);
					if (Product is not null)
					{
						var productItemOrdered = new ProductItemOrdered
						{

							ProductId = Product.Id,
							ProductName = Product.Name,
							PictureUrl = Product.PictureUrl ?? " "
						};
						var OrderItem = new OrderItems
						{
							Product = productItemOrdered,
							Price = Product.Price,
							Quantity = item.Quantity
						};

						OrderItems.Add(OrderItem);
					}
				}
			}

			// 3- Calculate Subtotal

			var SubTotal = OrderItems.Sum(items => items.Price * items.Quantity);

			//4. Map Address

			var MappedAddress = mapper.Map<Address>(order.ShipToAddress);

			// 5. Get Delivery Method
			var DeliveryMethod = await unitOfWork.GetRepositories<DeliveryMethod, int>().GetByIdAsync(order.DeliveryMethodId);

			// 6- Create Order
			
			var OrderToCreate= new Order
			{
				BuyerEmail = buyerEmail,
				OrderItems = OrderItems,
				ShippingAddress = MappedAddress,
				Subtotal = SubTotal,
				DeliveryMethod=DeliveryMethod
			};

			await unitOfWork.GetRepositories<Order, int>().AddAsync(OrderToCreate);

			// 7- Save to DB
			var Created = await unitOfWork.SaveChangesAsync() > 0 ; // return 2 OrderIems + 1 Order = 3
			if (!Created)
				throw new BadRequestException("an error Has Occured during Creating the Order");
			return mapper.Map<OrderToReturnDto>(OrderToCreate);

		}

		public async Task<IReadOnlyList<OrderToReturnDto>> GetOrdersForUserAsync(string buyerEmail)
		{
			var OrderSpecs = new OrderSpecification(buyerEmail);
			var orders= await unitOfWork.GetRepositories<Order,int>().GetAllWithSpecASync(OrderSpecs);
			return mapper.Map<IReadOnlyList<OrderToReturnDto>>(orders);
		}

		public  async Task<OrderToReturnDto> GetOrderByIdAsync(string buyerEmail, int OrderId)
		{
			var OrderSpec= new OrderSpecification(buyerEmail,OrderId);
			var Order= await unitOfWork.GetRepositories<Order, int>().GetByIdAWithSpecAsync(OrderSpec);
			
			if (Order is null)
				throw new NotFoundException(nameof(Order),OrderId);
			return mapper.Map<OrderToReturnDto>(Order);
		}

		public async Task<IReadOnlyList<DeliveryMethodDto>> GetDeliveryMethodsAsync()
		{
			var DeliveryMethods = await unitOfWork.GetRepositories<DeliveryMethod, int>().GetAllAsync();
			return mapper.Map<IReadOnlyList<DeliveryMethodDto>>(DeliveryMethods);
		}

	

		
	}
}

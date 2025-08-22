using AgroMind.GP.Core.Contracts.Repositories.Contract;
using AgroMind.GP.Core.Contracts.Services.Contract;
using AgroMind.GP.Core.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Shared.DTOs;
using Shared.DTOs.CartDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgroMind.GP.Service.Services
{
    public class CartService:ICartService
	{

		private readonly ICartRepository _cartRepository;
		
		private readonly IMapper _mapper;
		private readonly IConfiguration _configuration;

		public CartService(ICartRepository cartRepository, IMapper mapper,IConfiguration configuration)
		{
			_cartRepository = cartRepository;
		
			_mapper = mapper;
			_configuration = configuration;
		}

		public async Task<CartDto> GetUserCartAsync(string userId)
		{
			var Cart= await _cartRepository.GetCartAsync(userId);
			if(Cart == null)
				throw new KeyNotFoundException($"Cart for user with ID {userId} not found.");
			var Mappedcart=_mapper.Map<CartDto>(Cart);
			return Mappedcart;
		}

		public async Task<CartDto> UpdateCartAsync(CartDto cart)
		{
		  var MappedCart= _mapper.Map<Cart>(cart);
		  var daysToLive= int.Parse (_configuration.GetSection("RedisSettings")["TimeToLiveInDays"]!);
		  var UpdatedCart= await _cartRepository.UpdateCartAsync(MappedCart,TimeSpan.FromDays(daysToLive)); 
		  if(UpdatedCart is null )
				throw new Exception($"An Error has occured,Can't update your basket. Please Try Again.");
			return cart;
		}

		public async Task ClearUserCartAsync(string Id)
		=> await _cartRepository.DeleteCartAsync(Id);


	}
}


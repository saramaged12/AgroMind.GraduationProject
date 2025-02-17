using AgroMind.GP.Core.Entities;
using AgroMind.GP.Core.Repositories.Contract;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Options;
using Pipelines.Sockets.Unofficial;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AgroMind.GP.Repository.Repositories
{
	//JsonSerializer: Built-in .NET tool for handling JSON operations.


   //KeyDeleteAsync: Deletes a Redis key
   //StringGetAsync: Retrieves the value of a key as a string
   //StringSetAsync: Stores a key-value pair, with an optional expiration time
	public class CartRepository : ICartRepository
	{
		private readonly IDatabase _database;

		//IConnectionMultiplexer: This is an interface provided by StackExchange.Redis Package for interacting With redius
		//It manages connections to a Redis .
		public CartRepository(IConnectionMultiplexer redis) //Ask CLR for Object for Class Implement interface IConnectionMultiplexer
		{
			_database = redis.GetDatabase();//gets an interface to a Redis database

		}

		public async Task<bool> DeleteCartAsync(string CartId)
		{
			if (string.IsNullOrWhiteSpace(CartId))
				throw new ArgumentException("Cart ID cannot be null or empty.", nameof(CartId));

			return await _database.KeyDeleteAsync(CartId); //KeyDeleteAsync bta5od rediusKey(CartId)
														   //Redis keys represent the stored cart objects
		}

		//Redis Values >- Stored as Json
		public async Task<Cart?> GetCartAsync(string CartId)
		{
			if (string.IsNullOrWhiteSpace(CartId))
				throw new ArgumentException("Cart ID cannot be null or empty");

			var cart = await _database.StringGetAsync(CartId); //redis Value  //as json
															   // convert json To Cart
															   //if (basket.IsNull) return null; // Law El id el b3toh Not there
															   //else

			//var ReturnedBasket=JsonSerializer.Deserialize<Cart>(cart); //convert json into object or valueType
			try
			{
				return JsonSerializer.Deserialize<Cart>(cart);
			}
			catch (JsonException ex)
			{
				// Log the exception 
				Console.WriteLine($"Error deserializing cart data: {ex.Message}");
				return null;
			}
		}

		public async Task<Cart?> RemoveFromCart(string CartId, int ItemId)
		{
			var Cart= await GetCartAsync(CartId);
			if (Cart is null) return null;
			var itemRemoved=Cart.items.RemoveAll(item => item.Id == ItemId)>0;
			if(!itemRemoved)
				return Cart; //No Changes of Item is not found
			return await UpdateCarttAsync(Cart);

		}

		//Updates cart or creates if it doesn't exist
		public async Task<Cart?> UpdateCarttAsync(Cart cart)
		{
			try
			{
				var jsonCart = JsonSerializer.Serialize(cart); //convert from object to Json to store in redis
				var CreatedOrUpdated = await _database.StringSetAsync(cart.id, jsonCart, TimeSpan.FromDays(5)); //Stores the serialized JSON string in Redis //RedisKey>id bta3 el cart ,jsoncart >"el cart after serialize
																												//expire time for object
																												//StringSetAsync>- b t return boolean
				if (!CreatedOrUpdated)
					return null;
				return await GetCartAsync(cart.id); //get cart after update or create
			}
			catch (Exception ex) 
			{
				Console.WriteLine($"Error updating cart: {ex.Message}");
				return null;
			}
			
		}

	}
}

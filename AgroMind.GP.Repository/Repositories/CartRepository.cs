using AgroMind.GP.Core.Contracts.Repositories.Contract;
using AgroMind.GP.Core.Entities;
using AgroMind.GP.Core.Entities.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Options;
using Pipelines.Sockets.Unofficial;
using Shared.DTOs.CartDtos;
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



		//Redis Values >- Stored as Json
		public async Task<Cart?> GetCartAsync(string Id)
		{

			var cartJson = await _database.StringGetAsync(Id); //redis Value  //as json
																	 // convert json To Cart
			return cartJson.IsNullOrEmpty ? null : JsonSerializer.Deserialize<Cart>(cartJson!);
		}

		
		//Updates cart or creates if it doesn't exist
		
		public async Task<Cart?> UpdateCartAsync(Cart cart, TimeSpan timeToLive)
		{

				var serializedCart = JsonSerializer.Serialize(cart); //convert from object to Json to store in redis
				bool isSaved = await _database.StringSetAsync(cart.Id, serializedCart, timeToLive); //Stores the serialized JSON string in Redis //RedisKey>id bta3 el cart ,jsoncart >"el cart after serialize
																									//expire time for object
				return isSaved ? cart : null;  //get cart after update or create																		//StringSetAsync>- b t return boolean
		
		}

		public async Task DeleteCartAsync(string Id)=>
		
			
			 await _database.KeyDeleteAsync(Id); //KeyDeleteAsync bta5od rediusKey(CartId)
											     //Redis keys represent the stored cart objects
		

	}
}

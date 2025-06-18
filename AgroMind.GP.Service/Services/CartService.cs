using AgroMind.GP.Core.Contracts.Repositories.Contract;
using AgroMind.GP.Core.Contracts.Services.Contract;
using AgroMind.GP.Core.Entities;
using AutoMapper;
using Shared.DTOs;
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
		private readonly IProductService _productService; // To get product details
		private readonly IMapper _mapper;

		public CartService(ICartRepository cartRepository, IProductService productService, IMapper mapper)
		{
			_cartRepository = cartRepository;
			_productService = productService;
			_mapper = mapper;
		}
		
		// Get user's cart  product details for display.
		
		public async Task<CartDto> GetUserCartAsync(string userId)
		{
			var cart = await _cartRepository.GetCartAsync(userId);
			var cartDto = _mapper.Map<CartDto>(cart);

			//Get cart items with current product details for display
			cartDto.TotalPrice = 0;
			foreach (var item in cartDto.Items)
			{
				var product = await _productService.GetProductByIdAsync(item.ProductId); // Get current product details
				if (product != null)
				{
					item.ProductName = product.Name;
					item.PictureUrl = product.PictureUrl;
					item.BrandName = product.BrandName;
					item.CategoryName = product.CategoryName;
					item.Price = product.Price; // Use current price for display
					item.LinePrice = item.Quantity * product.Price;
				}
				else
				{
					//  where product might no longer exist
					item.ProductName = "[Product Not Found]";
					item.PictureUrl = "";
					item.Price = 0;
					item.LinePrice = 0;
				}
				cartDto.TotalPrice += item.LinePrice;
			}

			return cartDto; //returns  CartDto with full product details
		}

		// Add product to the user's cart or update quantity if already present

		public async Task<CartDto> AddItemToCartAsync(string userId, AddToCartDto itemDto)
		{
			var cart = await _cartRepository.GetCartAsync(userId); // Get existing cart or new empty cart

			var product = await _productService.GetProductByIdAsync(itemDto.ProductId);
			if (product == null)
				throw new KeyNotFoundException($"Product with ID {itemDto.ProductId} not found.");

			var existingItem = cart.Items.FirstOrDefault(i => i.ProductId == itemDto.ProductId);

			if (existingItem != null)
			{
				existingItem.Quantity += itemDto.Quantity;
				// Update price and picture URL to current values on quantity change
				existingItem.PriceAtAddition = product.Price;
				existingItem.PictureUrlAtAddition = product.PictureUrl;
			}
			else
			{
				var newItem = _mapper.Map<CartItem>(itemDto);
				newItem.PriceAtAddition = product.Price; // Store current price
				newItem.PictureUrlAtAddition = product.PictureUrl; // Store current picture URL
				cart.Items.Add(newItem);
			}

			
			cart.Items.RemoveAll(item => item.Quantity <= 0);

			await _cartRepository.UpdateCartAsync(cart); 
			return await GetUserCartAsync(userId); 
		}

	
		//Updates the quantity of a specific product in the cart.
		
		public async Task<CartDto> UpdateItemQuantityAsync(string userId, int productId, int newQuantity)
		{
			var cart = await _cartRepository.GetCartAsync(userId);
			var existingItem = cart.Items.FirstOrDefault  (i => i.ProductId == productId) ;

			if (existingItem == null)
				throw new KeyNotFoundException($"Product with ID {productId} not found in cart.");

			if (newQuantity <= 0)
			{
				cart.Items.Remove(existingItem); // Remove item if quantity is zero or less
			}
			else
			{
				existingItem.Quantity = newQuantity;
				
				var product = await _productService.GetProductByIdAsync(productId);
				if (product != null)
					existingItem.PriceAtAddition = product.Price;
			}

			await _cartRepository.UpdateCartAsync(cart);
			return await GetUserCartAsync(userId); //returns The updated CartDto
		}

		
		//Removes a specific product from the cart.
		
	
		public async Task<CartDto> RemoveItemFromCartAsync(string userId, int productId)
		{
			var cart = await _cartRepository.GetCartAsync(userId);
			cart.Items.RemoveAll(item => item.ProductId == productId); // Remove all instances

			await _cartRepository.UpdateCartAsync(cart);
			return await GetUserCartAsync(userId);
		}

		
		// Clears all items from a user's cart.
		
		// returns True if the cart was successfully cleared (deleted from Redis), false otherwise
		public async Task<bool> ClearUserCartAsync(string userId)
		{
			return await _cartRepository.DeleteCartAsync(userId);
		}
	}
}


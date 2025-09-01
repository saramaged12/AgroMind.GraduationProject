using AgroMind.GP.APIs.DTOs;
using AgroMind.GP.Core.Entities.ProductModule;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.G04.Core.Mapping.Products
{
	public class ProductPictureURLResolver : IValueResolver<Product, ProductDTO, string?>
	{
		private readonly IConfiguration configuration;

		public ProductPictureURLResolver(IConfiguration configuration)
		{
			this.configuration = configuration;
		}

		public string? Resolve(Product source, ProductDTO destination, string? destMember, ResolutionContext context)
		{
			if (!string.IsNullOrEmpty(source.PictureUrl))
			{
				return $"{configuration.GetSection("Urls")["ApiBaseUrl"]}{source.PictureUrl}";
			}
			

				return string.Empty;

		}
	}
}

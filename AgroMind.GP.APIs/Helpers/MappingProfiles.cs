using AgroMind.GP.APIs.DTOs;
using AgroMind.GP.Core.Entities;
using AgroMind.GP.Core.Entities.ProductModule;
using AutoMapper;

namespace AgroMind.GP.APIs.Helpers
{
	public class MappingProfiles : Profile
	{
		public MappingProfiles()
		{
			//Mapping From Product to ProductDTO
			CreateMap<Product,ProductDTO>()
				.ForMember(d=>d.BrandName,o=>o.MapFrom(s=>s.Brand))
				.ForMember(d=>d.CategoryName,o=>o.MapFrom(s=>s.Category))
				.ForMember(d=>d.SupplierName,o=>o.MapFrom(s=>s.Supplier));

			// Mapping from ProductDTO to Product
			CreateMap<ProductDTO, Product>()
				.ForMember(d => d.Brand, o => o.Ignore()) // Ignore Brand navigation property
				.ForMember(d => d.Category, o => o.Ignore()) // Ignore Category navigation property
				.ForMember(d => d.Supplier, o => o.Ignore()); // Ignore Supplier navigation property

			
			CreateMap<Crop, CropDto>()
				.ForMember(d => d.FarmerName, o => o.MapFrom(s => s.Farmer));

			CreateMap<CropDto, Crop>()
				.ForMember(d => d.Farmer, o => o.Ignore());
		}
	}
}

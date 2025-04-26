using AgroMind.GP.APIs.DTOs;
using AgroMind.GP.Core.Entities;
using AgroMind.GP.Core.Entities.ProductModule;
using AutoMapper;
using Shared.DTOs;

namespace AgroMind.GP.APIs.Helpers
{
	public class MappingProfiles : Profile
	{
		public MappingProfiles()
		{

				CreateMap<Land, LandDTO>().ReverseMap();


			
				// Mapping From Product to ProductDTO
				CreateMap<Product, ProductDTO>()
					.ForMember(d => d.BrandName, o => o.MapFrom(s => s.Brand != null ? s.Brand.BrandName : string.Empty)) // Handle null Brand
					.ForMember(d => d.CategoryName, o => o.MapFrom(s => s.Category != null ? s.Category.CategoryName : string.Empty)) // Handle null Category
					.ForMember(d => d.SupplierName, o => o.MapFrom(s => s.Supplier != null ? s.Supplier.FName : string.Empty)); // Handle null Supplier

				// Mapping from ProductDTO to Product
				CreateMap<ProductDTO, Product>()
					.ForMember(d => d.Brand, o => o.Ignore()) // Ignore Brand navigation property
					.ForMember(d => d.Category, o => o.Ignore()) // Ignore Category navigation property
					.ForMember(d => d.Supplier, o => o.Ignore()); // Ignore Supplier navigation property

				CreateMap<Category, CategoryDTO>().ReverseMap();

				CreateMap<Brand, BrandDTO>().ReverseMap();

				// Mapping From Crop to CropDTO
				CreateMap<Crop, CropDto>()
					.ForMember(dest => dest.TotalCost, opt => opt.MapFrom(src => src.Stages != null ? src.Stages.Sum(s => s.TotalCost) : 0)) // Map TotalCost dynamically
					.ForMember(dest => dest.Stages, opt => opt.MapFrom(src => src.Stages)); // Map Stages

				CreateMap<CropDto, Crop>()
					.ForMember(dest => dest.Farmer, opt => opt.Ignore()) // Ignore Farmer navigation property
					.ForMember(dest => dest.Stages, opt => opt.MapFrom(src => src.Stages)); // Map Stages

				CreateMap<CropStage, CropStageDto>()
					.ForMember(dest => dest.TotalCost, opt => opt.MapFrom(src => src.Cost + (src.Steps != null ? src.Steps.Sum(step => step.Cost) : 0))) // Avoid null-propagating operator
					.ForMember(dest => dest.Steps, opt => opt.MapFrom(src => src.Steps)); // Map Steps

				CreateMap<CropStageDto, CropStage>()
					.ForMember(dest => dest.TotalCost, opt => opt.Ignore()) // TotalCost is calculated, not mapped
					.ForMember(dest => dest.Steps, opt => opt.MapFrom(src => src.Steps)); // Map Steps

				CreateMap<Step, StepDto>()
					.ForMember(dest => dest.Cost, opt => opt.MapFrom(src => src.Cost))
					.ReverseMap();
			}
		}
	}



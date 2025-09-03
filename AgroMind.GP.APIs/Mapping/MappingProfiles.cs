using AgroMind.GP.APIs.DTOs;
using AgroMind.GP.Core.Entities;
using AgroMind.GP.Core.Entities.ProductModule;
using AutoMapper;
using Shared.DTOs;
using static Azure.Core.HttpHeader;
using System.Text.RegularExpressions;
using Shared.DTOs.CartDtos;
using AgroMind.GP.Core.Entities.Orders;
using Shared.DTOs.OrderDTOs;
using Store.G04.Core.Mapping.Products;

namespace AgroMind.GP.APIs.Mapping
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {

            CreateMap<Land, LandDTO>().ReverseMap();
            CreateMap<Product, ProductDTO>()
                .ForMember(d => d.BrandName, o => o.MapFrom(s => s.Brand != null ? s.Brand.BrandName : string.Empty))
                .ForMember(d => d.CategoryName, o => o.MapFrom(s => s.Category != null ? s.Category.CategoryName : string.Empty))
				.ForMember(d => d.PictureUrl, options => options.MapFrom<ProductPictureURLResolver>());
			
			CreateMap<ProductDTO, Product>()
                .ForMember(d => d.Brand, o => o.Ignore())
                .ForMember(d => d.Category, o => o.Ignore())
                .ForMember(d => d.Supplier, o => o.Ignore());
            CreateMap<Category, CategoryDTO>().ReverseMap();
            CreateMap<Brand, BrandDTO>().ReverseMap();


            CreateMap<Cart, CartDto>().ReverseMap();
            CreateMap<CartItem, CartItemDto>().ReverseMap();


            CreateMap<Crop, CropDto>();
            CreateMap<CropStage, CropStageDto>();
            CreateMap<Step, StepDto>();


            // --- Definition DTO For Add and Update

            CreateMap<CropDefinitionDto, Crop>()
                .ForMember(dest => dest.TotalEstimatedCost, opt => opt.Ignore())
                .ForMember(dest => dest.TotalActualCost, opt => opt.Ignore())

                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.Land, opt => opt.Ignore());


            CreateMap<StageDefinitionDto, CropStage>()
                .ForMember(dest => dest.ActualCost, opt => opt.Ignore())
                .ForMember(dest => dest.TotalEstimatedCost, opt => opt.Ignore())
                .ForMember(dest => dest.TotalActualCost, opt => opt.Ignore())

                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.Crop, opt => opt.Ignore());

            CreateMap<StepDefinitionDto, Step>()
                .ForMember(dest => dest.ActualCost, opt => opt.Ignore())
                .ForMember(dest => dest.ActualStartDate, opt => opt.Ignore())

                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.Stage, opt => opt.Ignore());


            //FullEntity For --> UpdateActualsForCropAsync

            CreateMap<CropDto, Crop>()
                .ForMember(dest => dest.Id, opt => opt.Ignore()) // Id is from route
                .ForMember(dest => dest.TotalEstimatedCost, opt => opt.Ignore()) // Calculated
                .ForMember(dest => dest.TotalActualCost, opt => opt.Ignore()) // Calculated

                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.Land, opt => opt.Ignore())
                .ForMember(dest => dest.PlanType, opt => opt.Ignore())
                .ForMember(dest => dest.LandId, opt => opt.Ignore())
                .ForMember(dest => dest.Stages, opt => opt.Ignore()); // Collections managed manually in service

            CreateMap<CropStageDto, CropStage>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.TotalEstimatedCost, opt => opt.Ignore()) // Calculated
                .ForMember(dest => dest.TotalActualCost, opt => opt.Ignore()) // Calculated

                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore()) // Navigation
                .ForMember(dest => dest.CropId, opt => opt.Ignore())
                .ForMember(dest => dest.Crop, opt => opt.Ignore())
                .ForMember(dest => dest.Steps, opt => opt.Ignore()); // Collections managed manually in service

            CreateMap<StepDto, Step>()
                // ActualCost, ActualStartDate, EstimatedCost, PlannedStartDate Are Inputs
                .ForMember(dest => dest.Id, opt => opt.Ignore())

                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.Stage, opt => opt.Ignore()); // Navigation Property


            // Deep Copy Mapping for (AdoptRecommendedCropAsync) 

            CreateMap<Crop, Crop>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.LandId, opt => opt.Ignore())
                .ForMember(dest => dest.Land, opt => opt.Ignore())
                .ForMember(dest => dest.PlanType, opt => opt.Ignore())

                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.TotalActualCost, opt => opt.Ignore())
                .ForMember(dest => dest.Stages, opt => opt.MapFrom(src => src.Stages));

            CreateMap<CropStage, CropStage>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CropId, opt => opt.Ignore())
                .ForMember(dest => dest.Crop, opt => opt.Ignore())

                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.ActualCost, opt => opt.Ignore())
                .ForMember(dest => dest.TotalEstimatedCost, opt => opt.Ignore())
                .ForMember(dest => dest.TotalActualCost, opt => opt.Ignore())
                .ForMember(dest => dest.Steps, opt => opt.MapFrom(src => src.Steps));

            CreateMap<Step, Step>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.StageId, opt => opt.Ignore())
                .ForMember(dest => dest.Stage, opt => opt.Ignore())

                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())

                .ForMember(dest => dest.ActualCost, opt => opt.Ignore())
                .ForMember(dest => dest.ActualStartDate, opt => opt.Ignore())

                .ConstructUsing((src, context) => new Step { });


            CreateMap<Crop, PlanInfoDto>()
                .ForMember(dest => dest.Crop, opt => opt.MapFrom(src => src))
                .ForMember(dest => dest.CreatorEmail, opt => opt.MapFrom(src => src.CreatedBy))
                .ForMember(dest => dest.CreatorRole, opt => opt.Ignore())
                .ForMember(dest => dest.PlanType, opt => opt.MapFrom(src => src.PlanType.ToString()));



            //Order

            CreateMap<Order, OrderToReturnDto>()
                .ForMember(d => d.DeliveryMethod, o => o.MapFrom(s => s.DeliveryMethod!.ShortName));

            CreateMap<OrderItems, OrderItemsDto>()
                .ForMember(d => d.ProductId, o => o.MapFrom(s => s.Product.ProductId))
                .ForMember(d => d.ProductName, o => o.MapFrom(s => s.Product.ProductName))
				.ForMember(d => d.PictureUrl, options => options.MapFrom<OrderItemPictureURLResolver>());

            CreateMap<Address,AddressDto>().ReverseMap();

            CreateMap<DeliveryMethod, DeliveryMethodDto>();

		}
	}


    //ReverseMap() : For bidirectional mapping when names match.
    //Ignore(): For properties that are calculated, set manually, or are navigation properties.
    //MapFrom(): Only when names differ or complex transformations are needed.
}



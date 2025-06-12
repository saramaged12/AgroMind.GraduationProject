using AgroMind.GP.APIs.DTOs;
using AgroMind.GP.Core.Contracts.Services.Contract;
using AgroMind.GP.Core.Contracts.UnitOfWork.Contract;
using AgroMind.GP.Core.Entities;
using AgroMind.GP.Core.Entities.Identity;
using AgroMind.GP.Core.Entities.ProductModule;
using AgroMind.GP.Core.Specification;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgroMind.GP.Service.Services
{
	public class LandService :ILandService
	{
		// The controller should focus on handling HTTP requests and responses
		//, while the service layer should handle business logic, including validation and null checks.
		
			private readonly IUnitOfWork _unitOfWork;
			private readonly IMapper _mapper;
		private readonly UserManager<AppUser> _userManager;

		//Unit of Work -> If I will use the repository pattern, I will use the unit of work to manage the repositories
		//if i have than more one REpo
		    public LandService(IUnitOfWork unitOfWork, IMapper mapper, UserManager<AppUser> userManager)
		    {
				_unitOfWork = unitOfWork;
				_mapper = mapper;
			    _userManager = userManager;
		    }

			public async Task<LandDTO> AddAsync(LandDTO landDto)
			{
				if (landDto == null)
					throw new ArgumentNullException(nameof(landDto), "Land data cannot be null.");

			// Validate FarmerId : Ensure the FarmerId provided is actually a Farmer (Role)
			var farmer = await _userManager.FindByIdAsync(landDto.FarmerId);
			if (farmer == null || !await _userManager.IsInRoleAsync(farmer, "Farmer"))
			
				throw new UnauthorizedAccessException($"User with ID {landDto.FarmerId} is not a valid Farmer or does not exist.");
			
			// FarmerId in DTO is  assumed to come from the authenticated user in  controller


			var landEntity = _mapper.Map<Land>(landDto);
			// CreatorId for the Land entity itself is automatically  by DbContext, "Is not there in Dto"
			var repo = _unitOfWork.GetRepositories<Land, int>();

				await repo.AddAsync(landEntity);
				await _unitOfWork.SaveChangesAsync();

				return _mapper.Map<LandDTO>(landEntity);
			}
			public async Task DeleteLands(LandDTO landDto, string farmerId)
			{
				if (landDto == null)
					throw new ArgumentNullException(nameof(landDto), "Land data cannot be null.");

				var repo = _unitOfWork.GetRepositories<Land, int>();
				var landEntity = await repo.GetByIdAsync(landDto.Id);
				if (landEntity == null)
					throw new KeyNotFoundException($"Land with ID {landDto.Id} not found.");

			    if (landEntity.FarmerId != farmerId)
				throw new UnauthorizedAccessException($"User {farmerId} is not authorized to delete Land ID {landDto.Id}.");
			
			    repo.SoftDelete(landEntity);
				await _unitOfWork.SaveChangesAsync();


			}

			public async Task<IReadOnlyList<LandDTO>> GetAllLandsAsync()
			{
				
				var Repo = _unitOfWork.GetRepositories<Land, int>();
				var lands = await Repo.GetAllAsync();
				var landsDTO = _mapper.Map<IReadOnlyList<Land>, IReadOnlyList<LandDTO>>(lands);
				return landsDTO;

			}

			public async Task<LandDTO> GetLandByIdAsync(int id)
			{
				
				var land = await _unitOfWork.GetRepositories<Land, int>().GetByIdAsync(id);
				if (land == null)
					throw new KeyNotFoundException($"Land with ID {id} not found.");

				return _mapper.Map<Land, LandDTO>(land);
			}

			public async Task UpdateLands(LandDTO landDto, string modifierFarmerId)
			{
				//Maps the DTO to the entity and updates the Land
				if (landDto == null)
					throw new ArgumentNullException(nameof(landDto), "Land data cannot be null.");

				var repo = _unitOfWork.GetRepositories<Land, int>();
			    var spec = new LandSpecification(landDto.Id, forAuthorization: true);
			    var existingland = await repo.GetByIdAWithSpecAsync(spec);

			   if (existingland == null)
					throw new KeyNotFoundException($"Land with ID {landDto.Id} not found.");

			    // Authorization Check: Ensure the authenticated user is the owner of this land
		    	if (existingland.FarmerId != modifierFarmerId)
		    	
				throw new UnauthorizedAccessException($"User {modifierFarmerId} is not authorized to update Land ID {landDto.Id}.");
			    

			    // Map the updated properties to the existing entity
			    _mapper.Map(landDto, existingland);


				// Update the existing entity
				repo.Update(existingland);
				await _unitOfWork.SaveChangesAsync();
			}

		    public async Task<IReadOnlyList<LandDTO>> GetAllDeletedLandsAsync()
		    {
			   var repo=_unitOfWork.GetRepositories<Land,int>();
			   var deletedLands = await repo.GetAllDeletedAsync();
			   return _mapper.Map<IReadOnlyList<Land>, IReadOnlyList<LandDTO>>(deletedLands);

	       	}

		   // Retrieves all Land records belonging to a specific farmer

		    public async Task<IReadOnlyList<LandDTO>> GetMyLandsAsync(string farmerUserId)
		   {

			  var repo = _unitOfWork.GetRepositories<Land, int>();
			  var spec = new LandSpecification(farmerUserId, forMyLands: true); 
			  var myLands = await repo.GetAllWithSpecASync(spec);
			  return _mapper.Map<IReadOnlyList<Land>, IReadOnlyList<LandDTO>>(myLands);

		   }
	}
}


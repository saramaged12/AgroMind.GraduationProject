using AgroMind.GP.APIs.DTOs;
using AgroMind.GP.Core.Contracts.Services.Contract;
using AgroMind.GP.Core.Contracts.UnitOfWork.Contract;
using AgroMind.GP.Core.Entities;
using AgroMind.GP.Core.Entities.Identity;
using AgroMind.GP.Core.Entities.ProductModule;
using AgroMind.GP.Core.Exceptions;
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
		
			private readonly IUnitOfWork _unitOfWork;
			private readonly IMapper _mapper;
		    private readonly UserManager<AppUser> _userManager;

		
		    public LandService(IUnitOfWork unitOfWork, IMapper mapper, UserManager<AppUser> userManager)
		    {
				_unitOfWork = unitOfWork;
				_mapper = mapper;
			    _userManager = userManager;
		    }

			public async Task<LandDTO> AddAsync(LandDTO landDto)
			{
			

			    var landEntity = _mapper.Map<Land>(landDto);
			
		    	var repo = _unitOfWork.GetRepositories<Land, int>();

				await repo.AddAsync(landEntity);
				await _unitOfWork.SaveChangesAsync();

				return _mapper.Map<LandDTO>(landEntity);
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
				throw new NotFoundException(nameof(Land), id);

				return _mapper.Map<Land, LandDTO>(land);
			}

			public async Task UpdateLands(LandDTO landDto, string modifierFarmerId)
			{
			
				var repo = _unitOfWork.GetRepositories<Land, int>();
			    var spec = new LandSpecification(landDto.Id, forAuthorization: true);
			    var existingland = await repo.GetByIdAWithSpecAsync(spec);

			   
			    _mapper.Map(landDto, existingland);

				repo.Update(existingland);
				await _unitOfWork.SaveChangesAsync();
			}

		    public async Task DeleteLands(int landId, string farmerId)
		    {

			var repo = _unitOfWork.GetRepositories<Land, int>();
			var landEntity = await repo.GetByIdAsync(landId);

			if (landEntity == null)
				throw new NotFoundException(nameof(Land), landId);

			repo.SoftDelete(landEntity);
			await _unitOfWork.SaveChangesAsync();


		    }
		    public async Task<IReadOnlyList<LandDTO>> GetAllDeletedLandsAsync()
		    {
			   var repo=_unitOfWork.GetRepositories<Land,int>();
			   var deletedLands = await repo.GetAllDeletedAsync();
			   return _mapper.Map<IReadOnlyList<Land>, IReadOnlyList<LandDTO>>(deletedLands);

	       	}

		  

		   public async Task<IReadOnlyList<LandDTO>> GetMyLandsAsync(string farmerUserId)
		   {

			var repo = _unitOfWork.GetRepositories<Land, int>();
			var spec = new LandSpecification(farmerUserId, forMyLands: true);
			var myLands = await repo.GetAllWithSpecASync(spec);
			return _mapper.Map<IReadOnlyList<Land>, IReadOnlyList<LandDTO>>(myLands);

		   }
	}
}


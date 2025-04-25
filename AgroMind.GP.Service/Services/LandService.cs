using AgroMind.GP.APIs.DTOs;
using AgroMind.GP.Core.Contracts.Services.Contract;
using AgroMind.GP.Core.Contracts.UnitOfWork.Contract;
using AgroMind.GP.Core.Entities;
using AgroMind.GP.Core.Entities.ProductModule;
using AgroMind.GP.Core.Specification;
using AutoMapper;
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

			//Unit of Work -> If I will use the repository pattern, I will use the unit of work to manage the repositories
			//if i have than more one REpo
			public LandService(IUnitOfWork unitOfWork, IMapper mapper)
			{
				_unitOfWork = unitOfWork;
				_mapper = mapper;
			}

			public async Task<LandDTO> AddAsync(LandDTO landDto)
			{
				if (landDto == null)
					throw new ArgumentNullException(nameof(landDto));

				var landEntity = _mapper.Map<Land>(landDto);
				var repo = _unitOfWork.GetRepositories<Land, int>();

				await repo.AddAsync(landEntity);
				await _unitOfWork.SaveChangesAsync();

				return _mapper.Map<LandDTO>(landEntity);
			}
			public async Task DeleteLands(LandDTO landDto)
			{
				if (landDto == null)
					throw new ArgumentNullException(nameof(landDto), "Land data cannot be null.");

				var repo = _unitOfWork.GetRepositories<Product, int>();
				var landEntity = await repo.GetByIdAsync(landDto.Id);
				if (landEntity == null)
					throw new KeyNotFoundException($"Land with ID {landDto.Id} not found.");

				repo.Delete(landEntity);
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

			public async Task UpdateLands(LandDTO landDto)
			{
				//Maps the DTO to the entity and updates the Land
				if (landDto == null)
					throw new ArgumentNullException(nameof(landDto), "Land data cannot be null.");

				var repo = _unitOfWork.GetRepositories<Land, int>();

				var existingland = await repo.GetByIdAsync(landDto.Id);
				if (existingland == null)
					throw new KeyNotFoundException($"Land with ID {landDto.Id} not found.");

				// Map the updated properties to the existing entity
				_mapper.Map(landDto, existingland);


				// Update the existing entity
				repo.Update(existingland);
				await _unitOfWork.SaveChangesAsync();



			}
		}
	}


using AgroMind.GP.APIs.DTOs;
using AgroMind.GP.Core.Contracts.Services.Contract;
using AgroMind.GP.Core.Contracts.UnitOfWork.Contract;
using AgroMind.GP.Core.Entities;
using AgroMind.GP.Core.Specification;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgroMind.GP.Service.Services
{
	public class StageService : IStageService
	{
		private readonly IMapper _mapper;
		private readonly IUnitOfWork _unitOfWork;

		public StageService(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_mapper = mapper;
			_unitOfWork = unitOfWork;
		}

		public async Task<CropStageDto> AddStageAsync(CropStageDto stageDto)
		{
			if (stageDto == null)
				throw new ArgumentNullException(nameof(stageDto), "Stage data cannot be null.");

			var stageEntity = _mapper.Map<CropStage>(stageDto);
			var repo = _unitOfWork.GetRepositories<CropStage, int>();

			await repo.AddAsync(stageEntity);
			await _unitOfWork.SaveChangesAsync();
			return _mapper.Map<CropStageDto>(stageEntity);
		}

		public async Task DeleteStage(CropStageDto stageDto)
		{
			if (stageDto == null)
				throw new ArgumentNullException(nameof(stageDto), "Stage data cannot be null.");

			var repo = _unitOfWork.GetRepositories<CropStage, int>();
			var existingStage = await repo.GetByIdAsync(stageDto.Id);

			if (existingStage == null)
				throw new KeyNotFoundException($"Stage with ID {stageDto.Id} not found.");

			repo.Delete(existingStage);
			await _unitOfWork.SaveChangesAsync();
		}

		public async Task<IReadOnlyList<CropStageDto>> GetAllStagesAsync()
		{
			var Specification = new StageSpecification();
			var repo = _unitOfWork.GetRepositories<CropStage, int>();
			var stages = await repo.GetAllWithSpecASync(Specification);
			return _mapper.Map<IReadOnlyList<CropStage>, IReadOnlyList<CropStageDto>>(stages);
		}

		public async Task<CropStageDto> GetStageByIdAsync(int id)
		{
			var spec = new StageSpecification(id);
			var repo = _unitOfWork.GetRepositories<CropStage, int>();
			var stage = await repo.GetByIdAWithSpecAsync(spec);

			if (stage == null)
				throw new KeyNotFoundException($"Stage with ID {id} not found.");

			return _mapper.Map<CropStage, CropStageDto>(stage);
		}

		public async Task UpdateStage(CropStageDto stageDto)
		{
			if (stageDto == null)
				throw new ArgumentNullException(nameof(stageDto), "Satge data cannot be null.");

			var repo = _unitOfWork.GetRepositories<CropStage, int>();
			var existingStage = await repo.GetByIdAsync(stageDto.Id);

			if (existingStage == null)
				throw new KeyNotFoundException($"Stage with ID {stageDto.Id} not found.");



			// Map the updated properties to the existing entity
			_mapper.Map(stageDto, existingStage);


			// Update the existing entity
			repo.Update(existingStage);
			await _unitOfWork.SaveChangesAsync();

		}
	}
}
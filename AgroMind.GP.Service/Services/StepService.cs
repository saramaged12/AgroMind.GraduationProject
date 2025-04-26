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
	public class StepService : IStepService
	{
		private readonly IMapper _mapper;
		private readonly IUnitOfWork _unitOfWork;

		public StepService(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_mapper = mapper;
			_unitOfWork = unitOfWork;
		}


		//public async Task<StepDto> AddStepAsync(StepDto stepDto)
		//{
		//	if (stepDto == null)
		//		throw new ArgumentNullException(nameof(stepDto), "Step data cannot be null.");

		//	var stepEntity = _mapper.Map<Step>(stepDto);

		//	var repo = _unitOfWork.GetRepositories<Step, int>();
		//	await repo.AddAsync(stepEntity);

		//	// Update TotalCost of the parent stage
		//	var stageRepo = _unitOfWork.GetRepositories<CropStage, int>();
		//	if (stepEntity.StageId.HasValue)
		//	{
		//		var stage = await stageRepo.GetByIdAsync(stepEntity.StageId.Value);
		//		if (stage != null)
		//		{
		//			stage.TotalCost = stage.Cost + stage.Steps.Sum(step => step.Cost);
		//			stageRepo.Update(stage);
		//		}
		//	}


		//	await _unitOfWork.SaveChangesAsync();
		//	return _mapper.Map<StepDto>(stepEntity);
		//}

		public async Task DeleteStep(StepDto stepDto)
		{
			if (stepDto == null)
				throw new ArgumentNullException(nameof(stepDto), "Step data cannot be null.");

			var repo = _unitOfWork.GetRepositories<Step, int>();
			var existingStep = await repo.GetByIdAsync(stepDto.Id);

			if (existingStep == null)
				throw new KeyNotFoundException($"Crop with ID {stepDto.Id} not found.");

			repo.Delete(existingStep);
			await _unitOfWork.SaveChangesAsync();
		}

		public async Task<IReadOnlyList<StepDto>> GetAllStepsAsync()
		{

			var repo = _unitOfWork.GetRepositories<Step, int>();
			var steps = await repo.GetAllAsync();
			return _mapper.Map<IReadOnlyList<Step>, IReadOnlyList<StepDto>>(steps);
		}

		public async Task<StepDto> GetStepByIdAsync(int id)
		{

			var repo = _unitOfWork.GetRepositories<Step, int>();
			var step = await repo.GetByIdAsync(id);

			if (step == null)
				throw new KeyNotFoundException($"Step with ID {id} not found.");

			return _mapper.Map<Step, StepDto>(step);
		}



		//public async Task UpdateStep(StepDto stepDto)
		//{
		//	if (stepDto == null)
		//		throw new ArgumentNullException(nameof(stepDto), "Step data cannot be null.");

		//	var repo = _unitOfWork.GetRepositories<Step, int>();
		//	var existingStep = await repo.GetByIdAsync(stepDto.Id);

		//	if (existingStep == null)
		//		throw new KeyNotFoundException($"Step with ID {stepDto.Id} not found.");

		//	_mapper.Map(stepDto, existingStep);
		//	repo.Update(existingStep);

		//	// Update TotalCost of the parent stage
		//	var stageRepo = _unitOfWork.GetRepositories<CropStage, int>();

		//	//var stage = await stageRepo.GetByIdAsync(existingStep.StageId); // This is Cuse Error If stageRepo is null should To Check has Value or No
		//	//existingStep.StageId.Value //Is Correct

		//	if (existingStep.StageId.HasValue)
		//	{
		//		var stage = await stageRepo.GetByIdAsync(existingStep.StageId.Value);
		//		if (stage != null)
		//		{
		//			stage.TotalCost = stage.Cost + stage.Steps.Sum(step => step.Cost);
		//			stageRepo.Update(stage);
		//		}
		//	}
		//	await _unitOfWork.SaveChangesAsync();
		//}


		public async Task<StepDto> AddStepAsync(StepDto stepDto)
		{
			if (stepDto == null)
				throw new ArgumentNullException(nameof(stepDto), "Step data cannot be null.");

			var stepEntity = _mapper.Map<Step>(stepDto);

			var repo = _unitOfWork.GetRepositories<Step, int>();
			await repo.AddAsync(stepEntity);

			// Update TotalCost of the parent stage
			if (stepEntity.StageId.HasValue)
			{
				var stageRepo = _unitOfWork.GetRepositories<CropStage, int>();
				var stage = await stageRepo.GetByIdAsync(stepEntity.StageId.Value);
				if (stage != null)
				{
					stage.TotalCost = stage.Cost + stage.Steps.Sum(step => step.Cost);
					stageRepo.Update(stage);

					// Update the parent crop (no explicit assignment to TotalCost)
					if (stage.CropId.HasValue)
					{
						var cropRepo = _unitOfWork.GetRepositories<Crop, int>();
						var crop = await cropRepo.GetByIdAsync(stage.CropId.Value);
						if (crop != null)
						{
							cropRepo.Update(crop); // Save changes to the crop
						}
					}
				}
			}

			await _unitOfWork.SaveChangesAsync();
			return _mapper.Map<StepDto>(stepEntity);
		}

		public async Task UpdateStep(StepDto stepDto)
		{
			if (stepDto == null)
				throw new ArgumentNullException(nameof(stepDto), "Step data cannot be null.");

			var repo = _unitOfWork.GetRepositories<Step, int>();
			var existingStep = await repo.GetByIdAsync(stepDto.Id);

			if (existingStep == null)
				throw new KeyNotFoundException($"Step with ID {stepDto.Id} not found.");

			_mapper.Map(stepDto, existingStep);
			repo.Update(existingStep);

			// Update TotalCost of the parent stage
			if (existingStep.StageId.HasValue)
			{
				var stageRepo = _unitOfWork.GetRepositories<CropStage, int>();
				var stage = await stageRepo.GetByIdAsync(existingStep.StageId.Value);
				if (stage != null)
				{
					stage.TotalCost = stage.Cost + stage.Steps.Sum(step => step.Cost);
					stageRepo.Update(stage);

					// Update the parent crop (no explicit assignment to TotalCost)
					if (stage.CropId.HasValue)
					{
						var cropRepo = _unitOfWork.GetRepositories<Crop, int>();
						var crop = await cropRepo.GetByIdAsync(stage.CropId.Value);
						if (crop != null)
						{
							cropRepo.Update(crop); // Save changes to the crop
						}
					}
				}
			}

			await _unitOfWork.SaveChangesAsync();
		}
	}
}
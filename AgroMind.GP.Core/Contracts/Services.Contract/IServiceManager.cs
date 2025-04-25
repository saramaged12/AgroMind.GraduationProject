using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgroMind.GP.Core.Contracts.Services.Contract
{
	public interface IServiceManager
	{
		//Property For each Service in the Project

		public IProductService ProductService { get; }

		public ICategoryService CategoryService { get; }

		public IBrandService BrandService { get; }

		public ILandService LandService { get; }

		public IStepService StepService { get; }

		public IStageService StageService { get; }

		public ICropService CropService { get; }
	}
}

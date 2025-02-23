using AgroMind.GP.Core.Entities.ProductModule;
using AgroMind.GP.Core.Repositories.Contract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AgroMind.GP.APIs.Controllers
{

	public class ProductController : APIbaseController
	{
		private readonly IGenericRepositories<Product, int> _productrepo;

		public ProductController(IGenericRepositories<Product,int> Productrepo)
		{
			_productrepo = Productrepo;
		}

		//Get All



		//Get By Id
	}
}

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AgroMind.GP.APIs.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class RoleController : ControllerBase
	{
		private readonly RoleManager<IdentityRole> _roleManager;

		//public RoleController(RoleManager<IdentityRole> roleManager)
		//{
		//	_roleManager = roleManager;
		//}

		//[HttpGet("GetRoles")]
		//public IActionResult GetRoles()
		//{
		//	var roles = _roleManager.Roles.Select(r => r.Name).ToList();
		//	return Ok(roles);
		//}
	}
}

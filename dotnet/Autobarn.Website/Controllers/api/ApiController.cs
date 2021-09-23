using Microsoft.AspNetCore.Mvc;

namespace Autobarn.Website.Controllers.api {
	[Route("api")]
	[ApiController]
	public class ApiController : ControllerBase {
		[HttpGet]
		[Produces("application/hal+json")]
		public IActionResult Get() {
			var json = new {
				message = "Welcome to the Autobarn API",
				_links = new {
					models = new {
						href = "/api/models"
					},
					vehicles = new {
						href = "/api/vehicles"
					}
				},
				_actions = new {
					create = new {
						href = "/api/vehicles",
						method = "POST",
						type = "application/json",
						name = "Create a new vehicle"
					}
				}
			};
			return Ok(json);
		}
	}
}
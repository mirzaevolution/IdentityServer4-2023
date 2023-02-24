using Microsoft.AspNetCore.Mvc;

namespace Basics.IntrApi.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DataController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get() => Ok(new
        {
            message = "Hello World"
        });
    }
}
